using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanCarApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BrandsController : ControllerBase
{
    // Brand använder repository direkt då vi inte har CQRS för Brand
    private readonly IRepository<Brand> _repository;

    public BrandsController(IRepository<Brand> repository)
    {
        _repository = repository;
    }

    // GET api/brands — hämtar alla märken, tillgänglig för alla inloggade
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetAll()
    {
        var brands = await _repository.GetAllAsync();
        return Ok(brands);
    }

    // GET api/brands/5 — hämtar ett märke
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetById(int id)
    {
        var brand = await _repository.GetByIdAsync(id);
        if (brand == null) return NotFound();
        return Ok(brand);
    }

    // POST api/brands — skapar ett märke, endast Admin
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Brand brand)
    {
        await _repository.AddAsync(brand);
        return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
    }

    // PUT api/brands/5 — uppdaterar ett märke, endast Admin
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Brand brand)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return NotFound();
        brand.Id = id;
        await _repository.UpdateAsync(brand);
        return Ok(brand);
    }

    // DELETE api/brands/5 — tar bort ett märke, endast Admin
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var brand = await _repository.GetByIdAsync(id);
        if (brand == null) return NotFound();
        await _repository.DeleteAsync(brand);
        return NoContent();
    }
}