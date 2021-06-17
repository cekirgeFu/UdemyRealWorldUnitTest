using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Helpers;
using UdemyRealWorldUnitTest.Web.Models;
using UdemyRealWorldUnitTest.Web.Repository;
using Xunit;

namespace UdemyRealWorldUnitTest.Test
{
    public class ProductApiControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsApiController _controller;
        private readonly Helper _helper;

        private List<Product> products;

        public ProductApiControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsApiController(_mockRepo.Object);
            _helper = new Helper();

            products = new List<Product>() { new Product { Id = 1, Name = "Kalem",
            Price = 100, Stock = 3, Color = "Kırmızı"},
            new Product { Id = 2, Name = "Silgi",
            Price = 23, Stock = 31, Color = "Sarı"}};
        }

        [Theory]
        [MemberData(nameof(TestDataShare.AddTwoNumbersData), MemberType = typeof(TestDataShare))]
        public void Add_SampleValues_ReturnSum(int a, int b, int sum)
        {
            var result = _helper.add(a, b);
            Assert.Equal(sum, result);
        }

        [Fact]
        public async void GetProduct_ActionExecute_ReturnOkResultWithProduct()
        {
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(products);
            var result = await _controller.GetProducts();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>
                (okResult.Value);
            Assert.Equal<int>(2, returnProducts.ToList().Count);
        }

        [Theory]
        [InlineData (0)]
        public async void GetProduct_IdInvalid_ReturnNotFound(int productId)
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);

            var result = await _controller.GetProduct(productId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetProduct_IdValid_ReturnNotFound(int productId)
        {
            Product product = products.First(x=>x.Id == productId);
            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);

            var result = await _controller.GetProduct(productId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType < Product>(okResult.Value);

            Assert.Equal(productId, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
        }

        [Theory]
        [InlineData(1)]
        public void PutProdct_IdIsNotEqualProduct_ReturnBadRequestResult(int productId)
        {
            var product = products.First(x => x.Id == productId);
            var result = _controller.PutProduct(2, product);
            var badRquestResult = Assert.IsType<BadRequestResult>(result);
            //mock yapmadık cunku senaryo badrequestten donuyor repoya gitmiyor
        }

        [Theory]
        [InlineData(1)]
        public void PutPoduct_ActionExecutes_ReturnNoContentResult(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mockRepo.Setup(x => x.Update(product));

            var result = _controller.PutProduct(productId, product);
            _mockRepo.Verify(x => x.Update(product), Times.Once);

             Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void PostPoduct_ActionExecutes_ReturnCreateAtAction()
        {
            var product = products.First();
            _mockRepo.Setup(x => x.Create(product)).Returns
                (Task.CompletedTask); //async bir metodsa return ile beraber mutlaka islemin tamamlandigini da don

            var result = await _controller.PostProduct(product);
            var createdActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetProduct",createdActionResult.ActionName);
            _mockRepo.Verify(x => x.Create(product), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        public async void DeleteProduct_IdInValid_ReturnNotFound(int productId)
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);

            var resultNotFound = await _controller.DeleteProduct(productId);

            Assert.IsType<NotFoundResult>(resultNotFound.Result);

        }

        [Theory]
        [InlineData(1)]
        public async void DeleteProduct_ActionExecture_ReturnNoContent(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            _mockRepo.Setup(x => x.Delete(product));

            var resultNoContent = await _controller.DeleteProduct(productId);

            _mockRepo.Verify(x => x.Delete(product), Times.Once);

            Assert.IsType<NoContentResult>(resultNoContent.Result);
        }


    }
}
