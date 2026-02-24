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

            return BadRequest(new ResponseUserViewModel
            {
                Status = true,
                Code = 400,
                Errors = response.Errors
            });
        }

        return Ok(new ResponseUserViewModel
        {
            Message = "Usuário criado com sucesso!",
            Status = true,
            Code = 200
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Unauthorized(
                new ResponseUserViewModel
                {
                    Message = "Usuário ou senha inválidos.",
                    Status = true,
                    Code = 401
                });

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ResponseUserViewModel
            {
                Message = "Usuário ou senha inválidos.",
                Status = true,
                Code = 401
            });

        var token = _jwtTokenService.GenerateToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.Now.AddMinutes(60),
            Path = "/"
        };

        Response.Cookies.Append("access_token", token, cookieOptions);

        return Ok(new ResponseUserViewModel
        {
            Message = "Login realizado com sucesso!",
            Status = true,
            Code = 200,
            Email = user.Email,
            UserName = user.UserName
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");

        return Ok(new ResponseUserViewModel
        {
            Message = "Você saiu do sistema!",
            Status = true,
            Code = 200
        });
    }
}