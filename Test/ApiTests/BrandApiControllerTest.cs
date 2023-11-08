using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace Test.ApiTests
{
    public class BrandApiControllerTest
    {
        private readonly Mock<IBrandService> _moqService;
        private readonly BrandsController _brandsController;
        private List<Brand> brands;

        public BrandApiControllerTest()
        {
            _moqService = new Mock<IBrandService>();
            _brandsController = new BrandsController(_moqService.Object);
            brands = new List<Brand>()
            {
                new Brand{BrandId = 1, Name = "Aston Martin"}, 
                new Brand{BrandId = 2, Name = "Bentley"}, 
                new Brand{BrandId = 3,Name = "Dodge"}, 
                new Brand{BrandId = 4,Name = "Volvo"},
                new Brand{BrandId = 5,Name = "Skoda"}, 
                new Brand{BrandId = 6,Name = "Lexus"}, 
                new Brand{BrandId = 7,Name = "Hyundai"}, 
                new Brand{BrandId = 8,Name = "Ford"}
            };
        }

        [Fact]
        public void GetAll_ActionExecutes_ReturnSuccessDataResultWithBrands()
        {
            _moqService.Setup(x => x.GetAll()).Returns(new SuccessDataResult<List<Brand>>(brands,"Markalar listelendi."));

            var result = _brandsController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnDataResult = Assert.IsAssignableFrom<IDataResult<List<Brand>>>(okResult.Value);

            _moqService.Verify(x => x.GetAll(), Times.Once);
            Assert.True(returnDataResult.Success);
            Assert.Equal("Markalar listelendi.", returnDataResult.Message);
            Assert.NotNull(returnDataResult.Data);
            Assert.Equal(8, returnDataResult.Data.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public void GetById_ActionExecutes_ReturnSuccessDataResultWithBrand(int brandId)
        {
            Brand brand = brands.First(b => b.BrandId == brandId);

            _moqService.Setup(x => x.GetById(brandId)).Returns(new SuccessDataResult<Brand>(brand));

            var result = _brandsController.GetById(brandId);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnDataResult = Assert.IsAssignableFrom<IDataResult<Brand>>(okResult.Value);

            _moqService.Verify(x => x.GetById(brandId), Times.Once);
            Assert.True(returnDataResult.Success);
            Assert.NotNull(returnDataResult.Data);
            Assert.Equal(brandId, returnDataResult.Data.BrandId);
            Assert.Equal(brand.Name, returnDataResult.Data.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(9)]
        public void GetById_IdIsInvalid_ReturnErrorDataResult(int brandId)
        {
            Brand brand = null;

            _moqService.Setup(x => x.GetById(brandId)).Returns(new ErrorDataResult<Brand>(brand,"Geçersiz marka numarası."));

            var result = _brandsController.GetById(brandId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var returnDataResult = Assert.IsAssignableFrom<IDataResult<Brand>>(badRequestResult.Value);

            _moqService.Verify(x => x.GetById(brandId), Times.Once);
            Assert.False(returnDataResult.Success);
            Assert.Null(returnDataResult.Data);
            Assert.Equal("Geçersiz marka numarası.", returnDataResult.Message);
        }

        [Theory]
        [MemberData(nameof(BrandTestDataForValidUpdate))]
        public  void Update_ActionExecutes_ReturnSuccessResult(Brand brand) 
        {
            _moqService.Setup(x => x.Update(brand)).Returns(new SuccessResult("Marka güncellendi."));

            var result = _brandsController.Update(brand);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnResult = Assert.IsAssignableFrom<SuccessResult>(okResult.Value);

            _moqService.Verify(m => m.Update(brand), Times.Once);
            Assert.True(returnResult.Success);
            Assert.Equal("Marka güncellendi.", returnResult.Message);
        }

        [Theory]
        [MemberData(nameof(BrandTestDataForInValidUpdate))]
        public void Update_IdIsInvalid_ReturnErrorResult(Brand brand)
        {
            _moqService.Setup(x => x.Update(brand)).Returns(new ErrorResult("Düzenlenen marka bulunamadı."));

            var result = _brandsController.Update(brand);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var returnResult = Assert.IsAssignableFrom<ErrorResult>(badRequestResult.Value);

            Assert.False(returnResult.Success);
            Assert.Equal("Düzenlenen marka bulunamadı.", returnResult.Message);
        }

        public static IEnumerable<object[]> BrandTestDataForValidUpdate()
        {
            yield return new object[] { new Brand {BrandId = 1, Name = "Lotus" } };
        }

        public static IEnumerable<object[]> BrandTestDataForInValidUpdate()
        {
            yield return new object[] { new Brand { BrandId = 9, Name = "Lotus" } };
        }

    }
}
