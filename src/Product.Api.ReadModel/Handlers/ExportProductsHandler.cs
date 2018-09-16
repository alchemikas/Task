using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Api.DomainCore.Repository;
using Product.Api.DomainCore.Services;
using Product.Api.ReadModel.Queries;
using Product.Api.ReadModel.Queries.Responses;

namespace Product.Api.ReadModel.Handlers
{
    public class ExportProductsHandler : IQueryHandler<ExportProductQuery, File>
    {
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;
        private readonly IProductExportService _productExportService;

        public ExportProductsHandler(IReadOnlyProductRepository readOnlyProductRepository,
            IProductExportService productExportService)
        {
            _readOnlyProductRepository = readOnlyProductRepository;
            _productExportService = productExportService;
        }

        public async Task<File> Handle(ExportProductQuery query)
        {
            List<DomainCore.Models.Product> products = await _readOnlyProductRepository.GetProductsAsync();
            byte[] fileBytes = _productExportService.Export(products);

            return new File {Bytes = fileBytes, Name = $"Products_{DateTime.UtcNow}.xls"};
        }
    }
}
