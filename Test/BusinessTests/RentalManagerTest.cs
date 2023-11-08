using Business.Concrete;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.BusinessTests
{
    public class RentalManagerTest
    {
        private readonly Mock<IRentalDal> _moqService;
        private readonly RentalManager _rentalManager;
        private List<Rental> rentals;

        public RentalManagerTest()
        {
            _moqService = new Mock<IRentalDal>();
            _rentalManager = new RentalManager(_moqService.Object);
            rentals = new List<Rental>()
            {
                new Rental() {RentalId = 1, CustomerId = 1, CarId = 1, RentDate = new DateTime(2022,06,01,18,46,00), ReturnDate = new DateTime(2022,06,03,08,46,00)},
                new Rental() {RentalId = 2, CustomerId = 2, CarId = 1, RentDate = new DateTime(2022,06,03,10,46,00), ReturnDate = new DateTime(2022,06,06,18,46,00)},
                new Rental() {RentalId = 3, CustomerId = 2, CarId = 2, RentDate = new DateTime(2022,06,03,10,46,00)}
            };
        }

        [Fact]
        public void GetAll_ActionExecutes_ReturnSuccessDataResultWithRentals()
        {
            _moqService.Setup(x => x.GetAll(null)).Returns(rentals);

            // Important: When you're using Moq with optional parameters like GetAll().
            // You will get error. To fix this you should pass in a value for the optional
            // parameter.

            var result = _rentalManager.GetAll();

            var returnDataResult = Assert.IsType<SuccessDataResult<List<Rental>>>(result);

            _moqService.Verify(m => m.GetAll(null), Times.Once);
            Assert.True(returnDataResult.Success);
            Assert.Equal("Kiralık Arabalar listelendi", returnDataResult.Message);
            Assert.Equal(3, returnDataResult.Data.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetById_ActionExecutes_ReturnSuccessDataResultWithRental(int rentalId)
        {
            var rental = rentals.First(r => r.RentalId == rentalId);

            _moqService.Setup(x => x.Get(r => r.RentalId == rentalId)).Returns(rental);

            var result = _rentalManager.GetById(rentalId);

            var returnDataResult = Assert.IsType<SuccessDataResult<Rental>>(result);

            _moqService.Verify(m => m.Get(r => r.RentalId == rentalId), Times.Once);
            Assert.True(returnDataResult.Success);
            Assert.Equal("Kiralık Araba listelendi", returnDataResult.Message);
            Assert.Equal(rentalId, returnDataResult.Data.RentalId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public void GetById_IdIsInvalid_ReturnErrorDataResult(int rentalId)
        {
            Rental rental = null;

            _moqService.Setup(x => x.Get(r => r.RentalId == rentalId)).Returns(rental);

            var result = _rentalManager.GetById(rentalId);

            var returnDataResult = Assert.IsType<ErrorDataResult<Rental>>(result);

            _moqService.Verify(m => m.Get(r => r.RentalId == rentalId), Times.Once);
            Assert.False(returnDataResult.Success);
            Assert.Equal("Geçersiz kiralık araba numarası", returnDataResult.Message);
            Assert.Null(returnDataResult.Data);
        }

        [Theory]
        [MemberData(nameof(RentalTestDataForValidAdd))]
        public void Add_ActionExecutes_ReturnSuccessResult(Rental rental)
        {
            var rentalsById = rentals.Where(x => x.CarId == rental.CarId).ToList();

            _moqService.Setup(x => x.GetAll(r => r.CarId == rental.CarId)).Returns(new List<Rental>(rentalsById));

            _moqService.Setup(x => x.Add(rental));

            var result = _rentalManager.Add(rental);

            var returnResult = Assert.IsType<SuccessResult>(result);

            _moqService.Verify(m => m.Add(rental), Times.Once);
            Assert.True(returnResult.Success);
            Assert.Equal("Kiralık Araba eklendi", returnResult.Message);
        }

        [Theory]
        [MemberData(nameof(RentalTestDataForInValidReturnDateAdd))]
        public void Add_ReturnDateIsInvalid_ReturnErrorResult(Rental rental)
        {
            var rentalsById = rentals.Where(x => x.CarId == rental.CarId).ToList();

            _moqService.Setup(x => x.GetAll(r => r.CarId == rental.CarId)).Returns(new List<Rental>(rentalsById));

            _moqService.Setup(x => x.Add(rental));

            var result = _rentalManager.Add(rental);

            var returnResult = Assert.IsType<ErrorResult>(result);

            _moqService.Verify(m => m.Add(rental), Times.Never);
            Assert.False(returnResult.Success);
            Assert.Equal("Seçili kiralık araba kiralanmıştır", returnResult.Message);
        }

        public static IEnumerable<object[]> RentalTestDataForInValidReturnDateAdd()
        {
            yield return new object[] { new Rental { RentalId = 4, CustomerId = 3, CarId = 2, RentDate = new DateTime(2022, 07, 03, 10, 46, 00) } };
        }

        public static IEnumerable<object[]> RentalTestDataForValidAdd()
        {
            yield return new object[] { new Rental { RentalId = 4, CustomerId = 3, CarId = 1, RentDate = new DateTime(2022, 07, 03, 10, 46, 00) } };
        }
    }
}
