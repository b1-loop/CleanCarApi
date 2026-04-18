using CleanCarApi.Application.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanCarApi.Application.Auth.Handlers
{
   
        // Hanterar registrering av ny användare via ASP.NET Identity
        public class RegisterHandler : IRequestHandler<RegisterCommand, string>
        {
            // UserManager hanterar skapande och hantering av användare
            private readonly UserManager<IdentityUser> _userManager;

            // RoleManager hanterar roller i systemet
            private readonly RoleManager<IdentityRole> _roleManager;

            public RegisterHandler(
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<string> Handle(
                RegisterCommand request,
                CancellationToken cancellationToken)
            {
                // Skapar ett nytt Identity-användarkonto
                var user = new IdentityUser { UserName = request.Dto.Username };
                var result = await _userManager.CreateAsync(user, request.Dto.Password);

                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

                // Skapar rollen om den inte redan finns
                if (!await _roleManager.RoleExistsAsync(request.Dto.Role))
                    await _roleManager.CreateAsync(new IdentityRole(request.Dto.Role));

                // Tilldelar användaren den angivna rollen
                await _userManager.AddToRoleAsync(user, request.Dto.Role);

                return $"User {request.Dto.Username} registered with role {request.Dto.Role}";
            }
        }
    }

