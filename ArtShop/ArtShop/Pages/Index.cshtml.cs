using ArtShop.Models;
using Flurl.Http.Newtonsoft;
using Flurl.Http;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArtShopAPI.Models;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;

namespace ArtShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5111/api";
        [BindProperty(SupportsGet = true)]
        public string Message { get; set; }
        public List<Art> arts { get; set; } = new List<Art>();
        public UserDTO user { get; set; } = new UserDTO();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string? message)
        {
            if (message != null)
            {
                Message = message;
            }
            try
            {
                var serializer = new NewtonsoftJsonSerializer();
                arts = await link
                    .AppendPathSegment("Arts")
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .GetJsonAsync<List<Art>>();
                var userIdFromCookie = Request.Cookies["UserId"];
                if (!string.IsNullOrEmpty(userIdFromCookie))
                {
                    user = await link
                        .AppendPathSegment($"Users/{userIdFromCookie}")
                        .WithSettings(s => s.JsonSerializer = serializer)
                        .GetJsonAsync<UserDTO>();
                }
                return Page();

            }
            catch (FlurlHttpException ex)
            {
                var err = await ex.GetResponseJsonAsync<Response>();
                Console.WriteLine($"Error returned from {ex.Call.Request.Url}: {err.Message}");
            }
            return Page();

        }

        public async Task<IActionResult> OnPostBuyArt(int artId)
        {
            var userIdFromCookie = Request.Cookies["UserId"];
            var token = Request.Cookies["Token"];
            if (string.IsNullOrEmpty(userIdFromCookie)) return RedirectToPage("/login/Index", new { Message = "You must login first" });
            try
            {
                var serializer = new NewtonsoftJsonSerializer();
                var result = await link
                    .AppendPathSegment($"Arts/buyArt/{artId}")
                    .WithOAuthBearerToken(token)
                    .WithSettings(s => s.JsonSerializer = serializer)
                    .PostAsync()
                    .ReceiveJson<UserDTO>();
                return RedirectToAction("/index");

            }
            catch (FlurlHttpException ex)
            {
                var exx = await ex.GetResponseJsonAsync<Response>();
                if (exx == null)
                {
                    Message = "something happened";
                    return RedirectToAction("/index", Message);
                }
                Console.WriteLine($"Error returned from {ex.Call.Request.Url} : {exx.Message}");
                Message = exx.Message;
                return RedirectToAction("/index", new {message = Message});
            }
   
        }
    }
}
