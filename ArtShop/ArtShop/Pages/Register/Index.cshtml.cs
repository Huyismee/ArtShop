using ArtShopAPI.Models;
using Flurl.Http.Newtonsoft;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArtShop.Models;
using Flurl;

namespace ArtShop.Pages.Register
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5111/api";
        public string Message { get; set; }
        [BindProperty]
        public RegisterModel RegisterInput { get; set; } = new RegisterModel();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet(string? message)
        {
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("Expiration");
            Response.Cookies.Delete("UserId");
            Message = message;
        }
        public async Task<IActionResult> OnPostRegister()
        {
            try
            {
                var serializer = new NewtonsoftJsonSerializer();
                var result = await link
                    .AppendPathSegment("Users/register")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PostJsonAsync(RegisterInput)
                    .ReceiveJson<Response>();
                Message = result.Message;
                return RedirectToPage("/Login/index", new { message = Message});

            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseJsonAsync<Response>();
                if (err == null)
                {
                    Message = $"Unexpected error happened";
                    return Page();
                }
                Message = $"Error during register: {err.Message}";
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err.Message}");
                return Page();
            }

        }
    }
}
