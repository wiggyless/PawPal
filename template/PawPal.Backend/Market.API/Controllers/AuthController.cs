using Azure.Core;
using MediatR;
using PawPal.Application.Modules.Auth.Commands.Login;
using PawPal.Application.Modules.Auth.Commands.Logout;
using PawPal.Application.Modules.Auth.Commands.Refresh;
using PawPal.Application.Modules.Users.Commands.ConfirmEmail;
using System.Text.Json.Serialization;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginCommandDto>> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var isHuman = await VerifyRecaptchaAsync(command.RecaptchaToken);
        if (!isHuman)
            return BadRequest("CAPTCHA verification failed.");

        return Ok(await mediator.Send(command, ct));
    }

    private async Task<bool> VerifyRecaptchaAsync(string token)
    {
        using var client = new HttpClient();
        var response = await client.PostAsync(
            "https://www.google.com/recaptcha/api/siteverify",
            new FormUrlEncodedContent(new Dictionary<string, string> {
            { "secret", configuration["RecaptchaSettings:SecretKey"] },
            { "response", token }
            })
        );
        var json = await response.Content.ReadFromJsonAsync<RecaptchaResponse>();
        return json?.Success == true;
    }

    public class RecaptchaResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginCommandDto>> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, CancellationToken ct)
    {
        var result = await mediator.Send(new ConfirmEmailCommand { Token = token }, ct);
        return Ok(result);
    }


    [Authorize]
    [HttpPost("logout")]
    public async Task Logout([FromBody] LogoutCommand command, CancellationToken ct)
    {
        await mediator.Send(command, ct);
    }
}