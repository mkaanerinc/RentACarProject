using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;

CarManager carManager = new CarManager(new InMemoryCarDal());
BrandManager brandManager = new BrandManager(new EFBrandDal());

// For EF Core

brandManager.Add(new Brand
{
    Name = "Test Brand",
});

Console.WriteLine(brandManager.GetById(1).Name);

List<Brand> brands = brandManager.GetAll();

foreach (Brand brand in brands)
{
    Console.WriteLine(brand.BrandId + " ==>" + brand.Name);
}


// For InMemory

carManager.Add(new Car
{
    CarId = 6,
    BrandId = 1,
    ColorId = 1,
    Name = "Test Car",
    DailyPrice = 33,
    ModelYear = 1998,
    Description = "Test Car"
});

Console.WriteLine(carManager.GetById(6).Description);

carManager.Update(new Car
{
    CarId = 6,
    BrandId = 1,
    ColorId = 1,
    Name = "Test Car",
    DailyPrice = 33,
    ModelYear = 1998,
    Description = "Updated Test Car"
});

Console.WriteLine(carManager.GetById(6).Description);

carManager.Delete(new Car
{
    CarId = 6,
    BrandId = 1,
    ColorId = 1,
    Name = "Test Car",
    DailyPrice = 33,
    ModelYear = 1998,
    Description = "Updated Test Car"
});

List<Car> cars = carManager.GetAll();

foreach (var car in cars)
{
    Console.WriteLine(car.Description);
}

