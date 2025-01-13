using System.Security.Claims;
using CarRepairReservation.Web.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRepairReservation.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public ProfileController(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var token = User.FindFirst("AccessToken")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}api/profile");
            if (response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadFromJsonAsync<ProfileViewModel>();
                return View(userInfo);
            }

            TempData["ErrorMessage"] = "Profil bilgileri alınamadı.";
            return View(new ProfileViewModel());
        }
        catch
        {
            TempData["ErrorMessage"] = "Bir hata oluştu.";
            return View(new ProfileViewModel());
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        try
        {
            var token = User.FindFirst("AccessToken")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}api/profile", new
            {
                model.FirstName,
                model.LastName
            });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Profil bilgileriniz güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View("Index", model);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Profil güncellenirken bir hata oluştu.");
            return View("Index", model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ProfileViewModel model)
    {
        if (string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmNewPassword))
        {
            ModelState.AddModelError(string.Empty, "Tüm şifre alanlarını doldurun.");
            return View("Index", model);
        }

        if (model.NewPassword != model.ConfirmNewPassword)
        {
            ModelState.AddModelError(string.Empty, "Yeni şifreler eşleşmiyor.");
            return View("Index", model);
        }

        try
        {
            var token = User.FindFirst("AccessToken")?.Value;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}api/profile/change-password", new
            {
                model.CurrentPassword,
                model.NewPassword,
                ConfirmPassword = model.ConfirmNewPassword
            });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction(nameof(Index));
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View("Index", model);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Şifre değiştirilirken bir hata oluştu.");
            return View("Index", model);
        }
    }
} 