using Microsoft.AspNetCore.Mvc;
using Product.Interface;
using Product.Model;

namespace Product.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly IProductCaller _productCaller;

        public ProductController(IProductCaller productCaller)
        {
            _productCaller = productCaller;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productCaller.GetAll();
            return Json(response);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProducts([FromBody] Products products)
        {
            var response = await _productCaller.Add(products);
            return Json(response);
        }

        [HttpGet("GetByCode/{product_code}")]
        public async Task<IActionResult> GetByCode(string product_code)
        {
            if (string.IsNullOrEmpty(product_code)) { return BadRequest(new { message = "Invalid product data" }); }

            var result = await _productCaller.Get(product_code);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateStudent([FromBody] Products products)
        {
            if (products == null) { return BadRequest(new { message = "Invalid product data" }); }

            var result = await _productCaller.Edit(products);
            return Json(result);
        }

        [HttpGet("Delete/{product_code}")]
        public async Task<IActionResult> DeleteStudent(string product_code)
        {
            if (string.IsNullOrEmpty(product_code)) { return BadRequest(new { message = "Invalid product data" }); }

            var result = await _productCaller.Delete(product_code);
            return Json(result);
        }

        // Change
    }
}
