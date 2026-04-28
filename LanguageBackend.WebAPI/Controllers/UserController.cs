using LanguageBackend.Application.Features.Users.Commands.ChangePassword;
using LanguageBackend.Application.Features.Users.Commands.DeleteUser;
using LanguageBackend.Application.Features.Users.Commands.UpdateUser;
using LanguageBackend.Application.Features.Users.Oueries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;

        // Dependency Injection ile sadece MediatR kuryesini içeri alıyoruz
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpDelete("delete-profile")]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });


            var command = new DeleteUserCommand { UserId = userId };

            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Hesabınız başarıyla silindi." });
            }
            return BadRequest(new { message = "Kullanıcı silme işlemi sırasında bir hata oluştu." });
        }

        [Authorize] // Önce Anahtarın (Token) var mı kontrolü yap
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserCommand command)
        {
            //  Token içine gomdugumuz ıd bılgısını aldık 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //  Okuduğun ID'yi paketin (Command) içine yerleştir
            command.UserId = userId;

            //  Paketi MediatR ile Handler'a gönder
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { message = "Profiliniz güncellendi." });

            return BadRequest(new { message = "Güncelleme başarısız." });
        }

        [Authorize]
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });

            var result = await _mediator.Send(new GetUserByIdQuery { UserId = userId });

            if (result == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            return Ok(result);
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });

            command.UserId = userId;

            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { message = "Şifreniz başarıyla değiştirildi." });

            return BadRequest(new { message = "Şifre değiştirme başarısız. Eski şifrenizi kontrol edin." });
        }
    }
}
