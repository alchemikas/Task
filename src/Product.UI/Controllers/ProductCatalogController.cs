using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Client;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using ProductUI.Models;

namespace ProductUI.Controllers
{
    public class ProductCatalogController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IMapper _mapper;

        public ProductCatalogController(IProductApiClient productApiClient,
            IMapper mapper)
        {
            _productApiClient = productApiClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchTerm = null)
        {
            GetProductsResponse response = await _productApiClient.GetAll(searchTerm);
            List<ProductViewModel> viewModel = _mapper.Map<List<ProductViewModel>>(response.Products);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ExportFile()
        {
            FileExport file = await _productApiClient.Export();
            return  File(file.Bytes, file.ContentType, file.FileName);
        }

        public IActionResult Create()
        {
            return View("Create", new ProductEditModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductEditModel product)
        {
            if (ModelState.IsValid)
            {
                if (!await IsCodeUnique(product))
                {
                    ModelState.AddModelError("code", "Code is not unique.");
                    return View(product);
                }

                CreateProduct apiModel = _mapper.Map<CreateProduct>(product);
                if (product.Image != null)
                {
                    MapImageFile(product.Image, apiModel.Photo);
                }

                Response response = product.Id == default(int)?
                      await _productApiClient.Create(apiModel) 
                    : await _productApiClient.Update(apiModel, product.Id);

                if (response.Errors != null && response.Errors.Any())
                {
                    foreach (var error in response.Errors)
                    {
                        ModelState.AddModelError(error.Reason, error.Message);
                    }
                }
                return RedirectToAction("Index");
            }

            return View(product);
        }


        public async Task<IActionResult> Edit(int id)
        {
            GetProductDetailsResponse response = await _productApiClient.Get(id);
            var editModel = _mapper.Map<ProductEditModel>(response.Product);

            return View("Create", editModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            GetProductDetailsResponse response = await _productApiClient.Get(id);
            ProductViewModel viewModel = _mapper.Map<ProductViewModel>(response.Product);

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productApiClient.Delete(id);
            return RedirectToAction("Index");
        }

        private void MapImageFile(IFormFile formFile, ImageFile imageFile)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                imageFile.Content = Convert.ToBase64String(fileBytes);
            }
        }

        private async Task<bool> IsCodeUnique(ProductEditModel product)
        {
            bool isEditAction = product.Id != default(int);
            if (isEditAction)
            {
                bool doesCodeWasChanged = product.Code != product.CodeBeforeEdit;
                if (doesCodeWasChanged)
                {
                    return await _productApiClient.IsCodeUnique(product.Code);
                }
                return true;
            }

            return await _productApiClient.IsCodeUnique(product.Code);
        }
    }
}
