using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SuperShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Bloqueia a api e só entra quem tiver acesso ao token (JwtBearer)
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAllWithUsers()); // Busca os produtos todos através do repositório - Ok - embrulha tudo num json, testar no postman
        }
    }
}