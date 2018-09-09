using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            ApiResponse<GetProductsResponse> apiResponse = await _productApiClient.GetAll(searchTerm);
            List<ProductViewModel> viewModel = _mapper.Map<List<ProductViewModel>>(apiResponse.Response.Products);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ExportFile()
        {
            ApiResponse<FileExport> apiResponse = await _productApiClient.Export();
            return  File(apiResponse.Response.Bytes, apiResponse.Response.ContentType, apiResponse.Response.FileName);
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



                ApiResponse<ViewProduct> apiResponse = product.Id != default(int) ?
                    await _productApiClient.Update(apiModel, product.Id)
                    : await _productApiClient.Create(apiModel);

                if (apiResponse.HttpStatusCode != HttpStatusCode.OK && apiResponse.HttpStatusCode != HttpStatusCode.Created)
                {
                    foreach (var error in apiResponse.Response.Errors)
                    {
                        ModelState.AddModelError(error.Reason, error.Message);
                    }
                    return View(product);
                }
                
                return RedirectToAction("Index");
            }

            return View(product);
        }


        public async Task<IActionResult> Edit(int id)
        {
            ApiResponse<ProductDetailsResponse> apiResponse = await _productApiClient.Get(id);
            var editModel = _mapper.Map<ProductEditModel>(apiResponse.Response.Product);

            return View("Create", editModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            ApiResponse<ProductDetailsResponse> response = await _productApiClient.Get(id);
            ProductViewModel viewModel = _mapper.Map<ProductViewModel>(response.Response.Product);

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
                    ApiResponse apiResponse = await _productApiClient.IsCodeUnique(product.Code);
                    return apiResponse.HttpStatusCode == HttpStatusCode.NotFound;
                }
                return true;
            }

            ApiResponse response = await _productApiClient.IsCodeUnique(product.Code);
            return response.HttpStatusCode == HttpStatusCode.NotFound;
        }
    }
}
