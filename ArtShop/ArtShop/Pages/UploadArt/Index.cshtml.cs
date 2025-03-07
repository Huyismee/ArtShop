using ArtShop.Models;
using ArtShopAPI.Models;
using Flurl.Http.Newtonsoft;
using Flurl.Http;
using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Newtonsoft.Json.Linq;
using System;

namespace ArtShop.Pages.UploadArt
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string link = "http://localhost:5111/api";
        private readonly string _uploadDirectory = "wwwroot/uploads";
        public string Message { get; set; }
        [BindProperty]
        public Art artInput { get; set; } = new Art();
        [BindProperty]
        public string des { get; set; }
        [BindProperty]
        public float price { get; set; }
        public UserDTO user { get; set; } = new UserDTO();
        public IFormFile imageUpload { get; set; }
        Random random = new Random();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }
        public IActionResult OnGet()
        {
            var userIdFromCookie = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdFromCookie)) return RedirectToPage("/login/Index", new { Message = "You must login first" });
            return Page();
        }

        public async Task<IActionResult> OnPostUpload()
        {
            var userIdFromCookie = Request.Cookies["UserId"];
            var token = Request.Cookies["Token"];
            if (string.IsNullOrEmpty(userIdFromCookie)) return RedirectToPage("/login/Index", new { Message = "You must login first" });

            var rentalDirectory = Path.Combine(_uploadDirectory, userIdFromCookie.ToString());
            var imageDirectory = Path.Combine(rentalDirectory, "img");
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            string imagePath;
            if (imageUpload.Length > 0 && imageUpload.ContentType.StartsWith("image/"))
            {
                string newFileName = $"{RandomString(random.Next(2,8))}{Path.GetExtension(imageUpload.FileName)}";
                string filePath = Path.Combine(imageDirectory, newFileName);
                imagePath = (Path.Combine("uploads", userIdFromCookie.ToString(), "img", newFileName));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    using (var image = Image.Load(imageUpload.OpenReadStream()))
                    {
                        var encoder = new JpegEncoder { Quality = 75 }; // Set chất lượng ảnh, giá trị từ 1-100
                        image.Save(stream, encoder);
                    }
                }
                artInput.Image = imagePath;
                artInput.UserId = Int32.Parse(userIdFromCookie.ToString());
                artInput.Price = price;
                artInput.Description = des;
                try
                {
                    var serializer = new NewtonsoftJsonSerializer();
                    var result = await link
                        .AppendPathSegment($"Arts")
                        .WithOAuthBearerToken(token)
                        .WithSettings(s => s.JsonSerializer = serializer)
                        .PostJsonAsync(artInput)
                        .ReceiveJson<Art>();
                    return RedirectToPage("/User/index");

                }
                catch (FlurlHttpException ex)
                {
                    var exx = await ex.GetResponseJsonAsync<Response>();
                    Console.WriteLine($"Error returned from {ex.Call.Request.Url}");
                    Message = $"Error returned from {ex.Call.Request.Url} : {exx.Message}";
                    return Page();
                }
            }


            Message = "You must login first";
            return Page();
        }

        public string RandomString(int length)
        {
            
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
