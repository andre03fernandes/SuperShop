using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SuperShop.Helpers
{
    public interface IImageHelper
    {
        Task<string> UpLoadImageAsync(IFormFile imageFile, string folder);
    }
}