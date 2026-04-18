
using CleanCarApi.Application.DTOs;
using MediatR;

namespace CleanCarApi.Application.Auth;

// Command för inloggning — returnerar JWT-token som sträng
public record LoginCommand(LoginDto Dto) : IRequest<string>;