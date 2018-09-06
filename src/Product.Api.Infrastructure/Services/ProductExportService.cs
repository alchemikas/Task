using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using Product.Api.DomainCore.Services;

namespace Product.Api.Infrastructure.Services
{
    public class ProductExportService : IProductExportService
    {
        public byte[] Export(List<DomainCore.Models.Product> products)
        {
            byte[] fileContents;

            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");

                CreateHeader(worksheet);

                AddProductsData(worksheet, products);

                fileContents = package.GetAsByteArray();
            }

            return fileContents;
        }

        private void AddProductsData(ExcelWorksheet worksheet, List<DomainCore.Models.Product> products)
        {
            for (int i = 0, row = 2; i < products.Count - 1; i++, row++)
            {
                int collumn = 1;
                worksheet.Cells[row, collumn].Value = products[i].Code;
                worksheet.Cells[row, collumn++].Value = products[i].Name;
                worksheet.Cells[row, collumn++].Value = products[i].Price;
                worksheet.Cells[row, collumn++].Value = products[i].LastUpdated;

                if (products[i].Photo != null)
                {

                    var picture = worksheet.Drawings.AddPicture($"{products[i].Photo.Title}_{DateTime.Now.Millisecond}",
                        Image.FromStream(new MemoryStream(Convert.FromBase64String(products[i].Photo.Content))));
                    picture.From.Column = collumn++;
                    picture.From.Row = row -1;
//                    picture.To.Column = collumn;//end cell value
//                    picture.To.Row = row;//end cell value
                    picture.SetSize(80, 80);
                    worksheet.Row(row).Height = 81;
                    worksheet.Column(collumn).Width = 30;
                }

            }
        }

        private void CreateHeader(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "Code";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Price";
            worksheet.Cells[1, 4].Value = "LastUpdated";
            worksheet.Cells[1, 5].Value = "Photo";
        }
    }
}
