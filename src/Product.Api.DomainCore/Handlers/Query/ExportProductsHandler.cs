using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.DomainCore.Querys;
using Product.Api.DomainCore.Querys.Responses;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;

namespace Product.Api.DomainCore.Handlers.Query
{
    public class ExportProductsHandler : IQueryHandler<ExportProductQuery, File>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductExportService _productExportService;

        public ExportProductsHandler(IProductRepository productRepository,
            IProductExportService productExportService)
        {
            _productRepository = productRepository;
            _productExportService = productExportService;
        }

        public async Task<File> Handle(ExportProductQuery query)
        {
            List<Models.Product> products = await _productRepository.GetProducts();
            byte[] fileBytes = _productExportService.Export(products);

            return new File {Bytes = fileBytes, Name = $"Products_{DateTime.UtcNow}.xls"};
        }
    }
}
