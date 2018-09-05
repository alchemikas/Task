using System;
using System.Collections.Generic;
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

        public ProductController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
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
        public void Post([FromBody] CreateProductRequest value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
