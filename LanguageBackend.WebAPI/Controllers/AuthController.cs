using LanguageBackend.Application.Features.Auth.Commands.DeleteUser;
using LanguageBackend.Application.Features.Auth.Commands.Login;
using LanguageBackend.Application.Features.Auth.Commands.Register;
using LanguageBackend.Application.Features.Auth.Commands.UpdateUser;
using LanguageBackend.Application.Features.Auth.Commands.VerifyEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // conrroller base den mıras aldık bu bıze view dwegil json verı donmemızı saglar 
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Dependency Injection ile sadece MediatR kuryesini içeri alıyoruz
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Kullanıcı kayıt kısmı 
        [HttpPost("register")]
        //istekler registerCommand Uzerınden yapılacak 
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            //kullanıcı bılgılerı _mediatr  a verdık bu RegisterCommand>RegisterCommandHandler.. dıye gezecek
            var result = await _mediator.Send(command);


            if (result)
                return Ok(new { message = "Kayıt başarılı. Lütfen e-postanızı kontrol edin." });

            return BadRequest(new { message = "Kayıt işlemi başarısız oldu." });
        }

        // e posta dogrulama
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { message = "E-posta başarıyla onaylandı." });

            return BadRequest(new { message = "Geçersiz veya süresi dolmuş onay kodu." });
        }

        //kullanıcı ğiriş kısmı 
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);

            // Eğer dönen sonuç "Hata:" ile başlıyorsa başarısızdır
            if (result.StartsWith("Hata:"))
                return Unauthorized(new { message = result });

            // Başarılıysa JWT Token'ı döndürüyoruz
            return Ok(new { token = result });
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
    }
}
