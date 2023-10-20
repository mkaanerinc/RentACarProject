using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryCarDal : ICarDal
    {
        private List<Car> _cars;

        public InMemoryCarDal()
        {
            _cars = new List<Car> {
                new Car { CarId = 1, BrandId = 1, ColorId = 1, Name = "Car", DailyPrice = 10, ModelYear = 1998, Description = "Car"},
                new Car { CarId = 2, BrandId = 1, ColorId = 2, Name = "Avarage Car", DailyPrice = 15, ModelYear = 1999, Description = "Avarage Car"},
                new Car { CarId = 3, BrandId = 2, ColorId = 1, Name = "Super Car", DailyPrice = 10, ModelYear = 1998, Description = "Super Car"},
                new Car { CarId = 4, BrandId = 1, ColorId = 1, Name = "Amazing Car", DailyPrice = 20, ModelYear = 2005, Description = "Amazing Car"},
                new Car { CarId = 5, BrandId = 3, ColorId = 4, Name = "Flying Car", DailyPrice = 25, ModelYear = 2006, Description = "Flying Car"},
            };
        }

        public void Add(Car car)
        {
            _cars.Add(car);
        }

        public void Delete(Car car)
        {
            Car carToDelete = _cars.SingleOrDefault(c => c.CarId == car.CarId);
            _cars.Remove(carToDelete);
        }

        public List<Car> GetAll()
        {
            return _cars;
        }

        public Car GetById(int carId)
        {
            return _cars.SingleOrDefault(c => c.CarId == carId);
        }

        public void Update(Car car)
        {
            Car carToUpdate = _cars.SingleOrDefault(c => c.CarId == car.CarId);
            carToUpdate.BrandId = car.BrandId;
            carToUpdate.ColorId = car.ColorId;
            carToUpdate.DailyPrice = car.DailyPrice;
            carToUpdate.ModelYear = car.ModelYear;
            carToUpdate.Description = car.Description;
        }
    }
}
