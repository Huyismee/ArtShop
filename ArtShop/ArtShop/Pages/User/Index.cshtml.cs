using ArtShop.Models;
using ArtShopAPI.Models;
using Flurl.Http.Newtonsoft;
using Flurl.Http;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace ArtShop.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5111/api";
        public string Message { get; set; }
        public List<Art> arts { get; set; } = new List<Art>();
        public UserDTO user { get; set; } = new UserDTO();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var userIdFromCookie = Request.Cookies["UserId"];
                var token = Request.Cookies["Token"];
                if (string.IsNullOrEmpty(userIdFromCookie)) return RedirectToPage("/login/Index", new { Message = "You must login first" });
                var serializer = new NewtonsoftJsonSerializer();
                arts = await link
                    .AppendPathSegment($"Arts/User/{userIdFromCookie}")
                    .WithOAuthBearerToken(token)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<Art>>()
                    ;
                user = await link
                    .AppendPathSegment($"Users/{userIdFromCookie}")
                    .WithOAuthBearerToken(token)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<UserDTO>();

                return Page();

            }
            catch (FlurlHttpException ex)
            {
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}");
            }
            return RedirectToPage("/index", new { Message = "You must login first" });

        }
    }
}
