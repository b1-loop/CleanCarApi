using CleanCarApi.Application.DTOs;
using MediatR;

namespace CleanCarApi.Application.Auth;

// Command för registrering — returnerar bekräftelsemeddelande
public record RegisterCommand(RegisterDto Dto) : IRequest<string>;