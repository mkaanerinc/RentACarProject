using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFCarDal : EFEntityRepository<Car, RentACarProjectContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails()
        {
            using (RentACarProjectContext context = new RentACarProjectContext())
            {
                var result = from car in context.Cars
                             join brand in context.Brands
                             on car.BrandId equals brand.BrandId
                             join color in context.Colors
                             on car.ColorId equals color.ColorId
                             select new CarDetailDto
                             {
                                 CarId = car.CarId,
                                 CarName = car.Name,
                                 BrandName = brand.Name,
                                 ColorName = color.Name,
                                 DailyPrice = car.DailyPrice
                             };

                return result.ToList();
            }
        }
    }
}
