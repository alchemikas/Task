using System.Collections.Generic;

namespace Product.Api.DomainCore.Services
{
    public interface IProductExportService
    {
        byte[] Export(List<Models.Product> products);
    }
}
