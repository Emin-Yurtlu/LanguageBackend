using LanguageBackend.Application.Features.Word.Commands.SubmitAnswer;
using LanguageBackend.Application.Features.Word.Queries.GetWord;
using LanguageBackend.Application.Features.Word.Queries.StartQuiz;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageBackend.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Seviyeye uygun yeni kelime getirme  getı 
        [HttpGet("get-word")]
        public async Task<IActionResult> GetWord()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });

            var result = await _mediator.Send(new GetWordQuery { UserId = userId });
            return Ok(result);
        }

        // Quiz sorularını getir
        [HttpGet("start-quiz")]
        public async Task<IActionResult> StartQuiz()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });

            var result = await _mediator.Send(new StartQuizQuery { UserId = userId });

            if (!result.Any())
                return Ok(new { message = "Tebrikler! Tüm kelimeleri öğrendiniz." });

            return Ok(result);
        }

        // Quiz cevabı gönder
        [HttpPost("submit-answer")]
        public async Task<IActionResult> SubmitAnswer(SubmitAnswerCommand command)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });

            command.UserId = userId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}