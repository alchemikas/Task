using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using Product.Api.DomainCore;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Exceptions.ClientErrors;
using Product.Api.DomainCore.Querys;
using Product.Api.DomainCore.Querys.Responses;
using Product.Api.DomainCore.Services;
using Product.Api.Infrastructure.Dispachers;

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
        private readonly IFileValidationService _fileValidationService;

        private const string ACCEPT_XLS = "application/vnd.ms-excel.";

        public ProductController(
            IQueryDispatcher queryDispatcher,
            ICommandDispacher commandDispacher,
            IMapper mapper,
            IFileValidationService fileValidationService)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispacher = commandDispacher;
            _mapper = mapper;
            _fileValidationService = fileValidationService;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProductQuery { ProductId = id};
            var response = await _queryDispatcher.Execute<GetProductQuery, GetProductDetailsResponse>(query).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpHead("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var query = new GetProductQuery { Code = code };
            var response = await _queryDispatcher.Execute<GetProductQuery, GetProductDetailsResponse>(query).ConfigureAwait(false);
            if (response.Product != null)
            {
                return Ok();
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Contract.CreateProduct product)
        {
            ValidateFile(product.Photo);
            var cmd = new CreateProductCommand
            {
                Product = _mapper.Map<DomainCore.Models.Product>(product)
            };

            await _commandDispacher.Execute(cmd);
            return NoContent();
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Contract.CreateProduct product)
        {
            ValidateFile(product.Photo);
            var request = new GetProductQuery {ProductId = id};
            bool doesProductExist = await _queryDispatcher.Execute<GetProductQuery, bool>(request).ConfigureAwait(false);

            var domainModel = _mapper.Map<DomainCore.Models.Product>(product);
            if (doesProductExist)
            {
                var updateCmd = new UpdateProductCommand
                {
                    Product = domainModel
                };
                updateCmd.Product.Id = id;

                await _commandDispacher.Execute(updateCmd).ConfigureAwait(false);
                return Ok();
            }

            CreateProductCommand createCmd = new CreateProductCommand
            {
                Product = domainModel
            };
            await _commandDispacher.Execute(createCmd).ConfigureAwait(false);
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteRequest = new DeleteProductCommand { ProductId = id};
            await _commandDispacher.Execute(deleteRequest).ConfigureAwait(false);
            return NoContent();
        }


        //TODO think about moving this from here and more cleaner way to implement validation
        private void ValidateFile(ImageFile file)
        {
            if (file != null)
            {
                if (!_fileValidationService.IsValidBase64String(file.Content))
                {
                    throw new ValidationException(new List<Fault>(){new Fault(){Reason = "InvalidFile", Message = "File base64string is invalid."}});
                }
            }
            
        }
    }
}
