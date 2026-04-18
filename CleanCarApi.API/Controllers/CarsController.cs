using CleanCarApi.Application.Cars.Commands;
using CleanCarApi.Application.Cars.Queries;
using CleanCarApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanCarApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// Alla endpoints kräver inloggning som standard
[Authorize]
public class CarsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET api/cars — hämtar alla bilar, tillgänglig för alla inloggade
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _mediator.Send(new GetAllCarsQuery());
        return Ok(cars);
    }

    // GET api/cars/5 — hämtar en bil, tillgänglig för alla inloggade
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _mediator.Send(new GetCarByIdQuery(id));
        return Ok(car);
    }

    // POST api/cars — skapar en bil, endast Admin
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCarDto dto)
    {
        var car = await _mediator.Send(new CreateCarCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    // PUT api/cars/5 — uppdaterar en bil, endast Admin
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateCarDto dto)
    {
        var car = await _mediator.Send(new UpdateCarCommand(id, dto));
        return Ok(car);
    }

    // DELETE api/cars/5 — tar bort en bil, endast Admin
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteCarCommand(id));
        return NoContent();
    }
}