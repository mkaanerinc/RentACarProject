using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.Dtos;

//CarManager carManager = new CarManager(new InMemoryCarDal());
CarManager carManager = new CarManager(new EFCarDal());
BrandManager brandManager = new BrandManager(new EFBrandDal());
ColorManager colorManager = new ColorManager(new EFColorDal());

//For EF Core

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

colorManager.Add(new Color
{
    Name = "Test Color",
});

Console.WriteLine(colorManager.GetById(1).Name);

List<Color> colors = colorManager.GetAll();

foreach (Color color in colors)
{
    Console.WriteLine(color.ColorId + " ==>" + color.Name);
}

carManager.Add(
    new Car { BrandId = 1, ColorId = 1, Name = "Test Car", DailyPrice = 10, ModelYear = 1998, Description = "My Test Car"}
);

List<CarDetailDto> carDetails = carManager.GetCarDetails();
foreach (var carDetail in carDetails)
{
    Console.WriteLine(carDetail.CarId + " => " + carDetail.CarName + " => " + carDetail.BrandName + " => " + carDetail.ColorName);
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

