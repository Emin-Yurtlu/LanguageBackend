using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LanguageBackend.Application.Features.Users.Oueries.GetUser
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserByIdQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
                return null!;

            return new GetUserResult
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName!,
                Email = user.Email!,
                Level = user.Level
            };
        }
    }
}