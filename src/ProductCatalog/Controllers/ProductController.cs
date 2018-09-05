using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Contract.Request;
using Product.Api.Contract.Response;
using Product.Api.Handlers;
using Product.Api.Infrastructure;

namespace Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispacher _commandDispacher;

        public ProductController(
            IQueryDispatcher queryDispatcher,
            ICommandDispacher commandDispacher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispacher = commandDispacher;
        }
        
        [HttpGet]
        public async Task<ActionResult<ProductsResponse>> Get([FromQuery] string searchTerm)
        {
            var query = new GetProductsRequest {SearchTerm = searchTerm};
            ProductsResponse result = await _queryDispatcher.Execute<GetProductsRequest, ProductsResponse>(query).ConfigureAwait(false);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> Get(int id)
        {
            var query = new GetProductRequest { ProductId = id};
            ProductResponse result = await _queryDispatcher.Execute<GetProductRequest, ProductResponse>(query).ConfigureAwait(false);
            return result;
        }

        
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] CreateProductRequest createProductRequest)
        {
            await _commandDispacher.Execute(createProductRequest).ConfigureAwait(false);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        
        [HttpPut("{id}")]
        public async Task<HttpResponseMessage> Put(int id, [FromBody] UpdateProductRequest updateProductRequest)
        {
            updateProductRequest.Product.Id = id;
            await _commandDispacher.Execute(updateProductRequest).ConfigureAwait(false);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        
        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            var deleteRequest = new DeleteProductRequest(){ ProductId = id};
            await _commandDispacher.Execute(deleteRequest).ConfigureAwait(false);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [HttpGet("/export")]
        public async Task<HttpResponseMessage> Export()
        {
//            var deleteRequest = new DeleteProductRequest() { ProductId = id };
//            await _commandDispacher.Execute(deleteRequest).ConfigureAwait(false);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
