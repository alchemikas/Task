using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Product.Api.Client;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using Product.Api.IntergrationTest.Models;

namespace Product.Api.IntergrationTest
{
    [TestFixture]
    public class GetProductTest
    {
        private readonly IProductApiClient _productApiClient;
        
        public GetProductTest()
        {
            _productApiClient = new ProductApiClient();
        }

        CreateProduct productModel = ProductModels.GetCreateModel();
        private int _createdProductId = 0;

        [SetUp]
        public async Task Setup()
        {
//            ProductDetailsResponse response = await _productApiClient.Create(productModel);
//            _createdProductId = response.Product.Id;
        }

        [TearDown]
        public async Task TearDown()
        {
            await _productApiClient.Delete(_createdProductId);
        }

        [Test]
        public async Task WhenGetingList_ThenListIsReturned()
        {
            var productsList = await _productApiClient.GetAll(string.Empty);

            Assert.That(productsList.Products.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(productsList.Products.Any(x => x.Name == productModel.Name), Is.True);
            Assert.That(productsList.Errors.Any(), Is.False);
        }

        [Test]
        public async Task WhenGetingListWithSearchTerm_ThenProductsWithSearchTermReturned()
        {
            var searchCode = productModel.Code;

            var productsList = await _productApiClient.GetAll(searchCode);

            Assert.That(productsList.Products.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(productsList.Products.Any(x => x.Code == searchCode), Is.True);
            Assert.That(productsList.Errors.Any(), Is.False);
        }

        [Test]
        public async Task WhenGetingSingleProduct_ThenProductIsReturned()
        {
            int productId = _createdProductId;

            var product = await _productApiClient.Get(productId);

            Assert.That(product.Product, Is.Not.Null);
            Assert.That(product.Product.Id, Is.EqualTo(productId));
            Assert.That(product.Product.Name, Is.EqualTo(productModel.Name));
            Assert.That(product.Product.Code, Is.EqualTo(productModel.Code));
            Assert.That(product.Product.Price, Is.EqualTo(productModel.Price));
        }
    }
}
