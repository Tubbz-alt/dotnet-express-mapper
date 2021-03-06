using dotnet_express_mapper.Models;
using dotnet_express_mapper.Services;
using ExpressMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace dotnet_express_mapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> Get()
        {
            IEnumerable<Product> products = await _productService.GetProducts();
            IEnumerable<ProductViewModel> productViewModels = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
            return new ObjectResult(productViewModels);
        }

        // GET api/products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> Get(int id)
        {
            Product product = await _productService.GetProduct(id);
            if (product == null)
                return new NotFoundResult();

            ProductViewModel productViewModel = Mapper.Map<Product, ProductViewModel>(product);
            return new OkObjectResult(productViewModel);
        }

        [Route("{id:int}/sizes")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizesOfProduct(int id)
        {
            object sizes = await _productService.GetSizeOfProduct(id);

            if (sizes == null)
                return new NotFoundResult();

            return new OkObjectResult(sizes);
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] ProductViewModel productViewModel)
        {
            Product product = await _productService.Create(productViewModel);
            return new OkObjectResult(product);
        }

        // PUT api/products/1
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id, [FromBody] ProductViewModel productViewModel)
        {
            Product product = await _productService.UpdateProductAsync(productViewModel);

            if (product == null)
                return new NotFoundResult();

            return new OkObjectResult(product);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return true;
        }
    }


}