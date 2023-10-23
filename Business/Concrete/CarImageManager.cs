using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Helpers.FileHelper.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        private ICarImageDal _carImageDal;
        private IFileHelper _fileHelper;

        public CarImageManager(ICarImageDal carImageDal, IFileHelper fileHelper)
        {
            _carImageDal = carImageDal;
            _fileHelper = fileHelper;
        }

        public IResult Add(IFormFile file, CarImage carImage)
        {
            IResult result = BusinessRules.Run(CheckIfCarImagesLimitExceded(carImage.CarId));

            if (result is not null)
                return result;

            carImage.ImagePath = _fileHelper.Upload(file, Paths.ImagesPath).Message;
            carImage.Date = DateTime.Now;

            _carImageDal.Add(carImage);

            return new SuccessResult();
        }

        public IResult Delete(CarImage carImage)
        {
            _fileHelper.Delete(Paths.ImagesPath + carImage.ImagePath);

            _carImageDal.Delete(carImage);

            return new SuccessResult();
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll());
        }

        public IDataResult<CarImage> GetById(int carImageId)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.CarImageId == carImageId));
        }

        public IDataResult<List<CarImage>> GetCarImagesByCarId(int carId)
        {
            IResult result = BusinessRules.Run(CheckIfCarImagesExists(carId));

            if (!result.Success)
            {
                return new SuccessDataResult<List<CarImage>>(GetCarImagesByDefaultCarImage(carId).Data);
            }
            else
            {
                return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(c => c.CarId == carId));
            }
        }

        public IResult Update(IFormFile file, CarImage carImage)
        {
            carImage.ImagePath = _fileHelper.Update(file, Paths.ImagesPath + carImage.ImagePath, Paths.ImagesPath).Message;

            _carImageDal.Update(carImage);

            return new SuccessResult();
        }

        private IResult CheckIfCarImagesLimitExceded(int carId)
        {
            if (_carImageDal.GetAll(c => c.CarId == carId).Count() == 5)
            {
                return new ErrorResult();
            }

            return null;
        }

        private IResult CheckIfCarImagesExists(int carId)
        {
            if (_carImageDal.GetAll(c => c.CarId == carId).Count() == 0)
            {
                return new ErrorResult();
            }

            return new SuccessResult();
        }

        private IDataResult<List<CarImage>> GetCarImagesByDefaultCarImage(int carId)
        {
            List<CarImage> carImage = new List<CarImage>();
            carImage.Add(new CarImage { CarId = carId, Date = DateTime.Now, ImagePath = "DefaultImage.jpeg" });
            return new SuccessDataResult<List<CarImage>>(carImage);
        }
    }
}
