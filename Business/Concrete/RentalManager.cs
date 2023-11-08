using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        private IRentalDal _rentalDal;

        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            IResult result = BusinessRules.Run(CheckIfRentalReturnDateIsValid(rental));

            if (result is not null)
                return result;

            _rentalDal.Add(rental);

            return new SuccessResult(Messages.RentalAdded);
            
        }

        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);

            return new SuccessResult(Messages.RentalDeleted);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(), Messages.RentalsListed);
        }

        public IDataResult<Rental> GetById(int rentalId)
        {
            Rental rental = _rentalDal.Get(r => r.RentalId == rentalId);

            if (rental is null)
                return new ErrorDataResult<Rental>(Messages.InvalidRentalId);

            return new SuccessDataResult<Rental>(rental, Messages.RentalListed);
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);

            return new SuccessResult(Messages.RentalUpdated);
        }

        private IResult CheckIfRentalReturnDateIsValid(Rental rental)
        {
            List<Rental> rentedCars = _rentalDal.GetAll(r => r.CarId == rental.CarId);

            foreach (Rental rentedCar in rentedCars)
            {
                if (rentedCar.ReturnDate is null)
                    return new ErrorResult(Messages.InvalidRental);
            }

            return new SuccessResult();
        }
    }
}
