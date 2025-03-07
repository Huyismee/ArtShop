using ArtShopAPI.Models;
using Flurl.Http.Newtonsoft;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArtShop.Models;
using Flurl;

namespace ArtShop.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5111/api";
        public string Message { get; set; }
        public List<Art> arts { get; set; } = new List<Art>();
        public LoginResponse loginResponse { get; set; }
        public UserDTO user { get; set; } = new UserDTO();
        [BindProperty]
        public LoginModel LoginInput { get; set; } = new LoginModel();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet(string? message)
        {
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("Expiration");
            Response.Cookies.Delete("UserId");

            loginResponse = null;
            user = new UserDTO();
            Message = message;
        }
        public async Task<IActionResult> OnPostLogin()
        {
            try
            {
                var serializer = new NewtonsoftJsonSerializer();
                loginResponse = await link
                    .AppendPathSegment("Users/login")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PostJsonAsync(LoginInput)
                    .ReceiveJson<LoginResponse>();

                if (loginResponse != null)
                {
                    Response.Cookies.Append("Token", loginResponse.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = loginResponse.Expiration
                    });
                    Response.Cookies.Append("Expiration", loginResponse.Expiration.ToString("o"), new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = loginResponse.Expiration
                    });
                    Response.Cookies.Append("UserId", loginResponse.UserId, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = loginResponse.Expiration
                    });

                    user = await link
                        .AppendPathSegment($"Users/{loginResponse.UserId}")
                        .WithSettings(s => s.JsonSerializer = serializer)
                        .GetJsonAsync<UserDTO>();

                    Message = "Login successful!";
                    return RedirectToPage("/index");
                }
                else
                {
                    Message = "Login failed. Please check your credentials.";
                    return Page();
                }
            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseJsonAsync<Response>();
                Message = $"Error during login: {err.Message}";
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err.Message} {ex.Message}");
            }

            return Page();
        }
    }
}
