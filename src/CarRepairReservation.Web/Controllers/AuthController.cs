using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRepairReservation.Application.DTOs.Auth;
using CarRepairReservation.Web.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CarRepairReservation.Web.Controllers;

public class AuthController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public AuthController(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");
            
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}api/auth/login", new { Email = model.Email, Password = model.Password });
            
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<TokenDto>();
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, token.UserName),
                    new Claim(ClaimTypes.Role, token.UserRole),
                    new Claim("AccessToken", token.AccessToken)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");
            
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}api/auth/register", model);
            
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<TokenDto>();
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, token.UserName),
                    new Claim(ClaimTypes.Role, token.UserRole),
                    new Claim("AccessToken", token.AccessToken)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> VerifyEmail(string userId, string token)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}api/auth/verify-email?userId={userId}&token={token}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Email adresiniz başarıyla doğrulandı. Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "Email doğrulama başarısız oldu. Lütfen tekrar deneyin.";
            return RedirectToAction("Login");
        }
        catch
        {
            TempData["ErrorMessage"] = "Email doğrulama sırasında bir hata oluştu.";
            return RedirectToAction("Login");
        }
    }

    [HttpGet]
    public IActionResult ResendVerification()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResendVerification(string email)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}api/auth/resend-verification", new { Email = email });
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Doğrulama emaili tekrar gönderildi. Lütfen email kutunuzu kontrol edin.";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "Doğrulama emaili gönderilemedi. Lütfen tekrar deneyin.";
            return View();
        }
        catch
        {
            TempData["ErrorMessage"] = "Doğrulama emaili gönderilirken bir hata oluştu.";
            return View();
        }
    }
} 