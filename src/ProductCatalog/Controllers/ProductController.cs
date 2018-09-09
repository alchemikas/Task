using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Queries;
using Product.Api.DomainCore.Queries.Responses;
using Product.Api.DomainCore.Services;
using Product.Api.LocalInfrastructure.Dispachers;

namespace Product.Api.Controllers
{

    [Route("api/[controller]")]
    [Produces("application/json", "application/vnd.ms-excel.")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispacher _commandDispacher;
        private readonly IMapper _mapper;

        private const string ACCEPT_XLS = "application/vnd.ms-excel.";

        public ProductController(
            IQueryDispatcher queryDispatcher,
            ICommandDispacher commandDispacher,
            IMapper mapper)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispacher = commandDispacher;
            _mapper = mapper;
        }

        [HttpGet("")]
        [Consumes("application/json", "application/vnd.ms-excel.")]
        public async Task<IActionResult> Get([FromQuery] string searchTerm)
        {
            HttpContext.Request.Headers.TryGetValue("Accept", out var acceptHeader);

            if (acceptHeader == ACCEPT_XLS)
            {
                File excelFile = await _queryDispatcher.Execute<ExportProductQuery, File>(new ExportProductQuery()).ConfigureAwait(false);

                return File(excelFile.Bytes, ACCEPT_XLS, excelFile.Name);
            }

            var query = new GetProductsQuery {SearchTerm = searchTerm};
            var response = await _queryDispatcher.Execute<GetProductsQuery, GetProductsResponse>(query).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProductQuery { ProductId = id};
            var response = await _queryDispatcher.Execute<GetProductQuery, ProductDetailsResponse>(query).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpHead("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var query = new GetProductQuery { Code = code };
            var response = await _queryDispatcher.Execute<GetProductQuery, ProductDetailsResponse>(query).ConfigureAwait(false);
            if (response.Product != null)
            {
                return Ok();
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Contract.CreateProduct product)
        {
            var createCmd = _mapper.Map<CreateProductCommand>(product);
           
            await _commandDispacher.Execute(createCmd);

            var query = new GetProductQuery { ProductId = createCmd .ProductId };
            var response = await _queryDispatcher.Execute<GetProductQuery, ProductDetailsResponse>(query).ConfigureAwait(false);

            return CreatedAtRoute("Get", new {id = createCmd.ProductId}, response.Product);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Contract.CreateProduct product)
        {
            var request = new GetProductQuery {ProductId = id};
            bool doesProductExist = await _queryDispatcher.Execute<GetProductQuery, bool>(request).ConfigureAwait(false);

            if (doesProductExist)
            {
           
                var updateCmd = _mapper.Map<UpdateProductCommand>(product);
                updateCmd.ProductId = id;

                await _commandDispacher.Execute(updateCmd).ConfigureAwait(false);
                return Ok();
            }

            var createCmd = _mapper.Map<CreateProductCommand>(product);

            await _commandDispacher.Execute(createCmd).ConfigureAwait(false);

            var query = new GetProductQuery { ProductId = createCmd.ProductId };
            var response = await _queryDispatcher.Execute<GetProductQuery, ProductDetailsResponse>(query).ConfigureAwait(false);

            return CreatedAtRoute("Get", new { id = createCmd.ProductId }, response.Product);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteRequest = new DeleteProductCommand { ProductId = id};
            await _commandDispacher.Execute(deleteRequest).ConfigureAwait(false);
            return NoContent();
        }
    }
}
