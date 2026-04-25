using LanguageBackend.Application.Features.Auth.Commands.UpdateUser;
using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LanguageBackend.Application.Features.Auth.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly UserManager<AppUser> _userManager;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            //  Gelen paketteki ID ile kullanıcıyı veritabanından bul  bu ıd swagger arayuzunde  [JsonIgnore] dolayı gorulmeyecek 
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return false;

            //  Kullanıcı bilgilerini yeni gelenlerle değiştirdik istekten geleni user a at 
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.Level = request.Level;

            // Değişiklikleri kaydet
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }

}