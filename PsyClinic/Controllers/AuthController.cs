using Azure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PsyClinic.Api.Services;
using PsyClinic.Api.ViewModels.Auth;
using PsyClinic.Infrasctructure.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        JwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    [EnableCors("AllowOrigin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RequestUserViewModel request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name,
            PhoneNumber = request.Phone,
            FederalRegistration = request.FederalRegistration
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var response = new ValidationErrorResponse();

            foreach (var error in result.Errors)
            {
                response.Errors.Add(IdentityErrorTranslator.Translate(error.Code));
            }

            return BadRequest(response);
        }

        return Created("", new { Message = "Usuário criado com sucesso!" });
    }

    [EnableCors("AllowOrigin")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Unauthorized("Usuário ou senha inválidos.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized("Usuário ou senha inválidos.");

        var token = _jwtTokenService.GenerateToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(60)
        };

        Response.Cookies.Append("access_token", token, cookieOptions);

        return Ok(new
        {
            Message = "Login realizado com sucesso",
            user.Email,
            user.UserName
        });
    }

    [EnableCors("AllowOrigin")]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return NoContent();
    }
}