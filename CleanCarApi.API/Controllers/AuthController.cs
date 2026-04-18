using CleanCarApi.Application.Auth;
using CleanCarApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanCarApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // MediatR används för att skicka commands till rätt handler
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST api/auth/register — registrerar en ny användare
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await _mediator.Send(new RegisterCommand(dto));
        return Ok(result);
    }

    // POST api/auth/login — loggar in och returnerar JWT-token
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _mediator.Send(new LoginCommand(dto));
        return Ok(new { token });
    }
}