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
            for (int i = 0, row = 2; i < products.Count; i++, row++)
            {
                worksheet.Cells[row, 1].Value = products[i].Code;
                worksheet.Cells[row, 2].Value = products[i].Name;
                worksheet.Cells[row, 3].Value = products[i].Price;
                worksheet.Cells[row, 4].Value = products[i].LastUpdated.ToString("yyyy-MM-dd H:mm:ss");

                if (products[i].Image != null)
                {

                    var picture = worksheet.Drawings.AddPicture($"{products[i].Image.Title}_{DateTime.Now.Millisecond}",
                        Image.FromStream(new MemoryStream(products[i].Image.Content)));
                    picture.From.Column = 4;
                    picture.From.Row = row - 1;
                    picture.SetSize(80, 80);
                    worksheet.Row(row).Height = 81;
                    worksheet.Column(5).Width = 30;
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

            worksheet.Column(4).Width = 20;
        }
    }
}
