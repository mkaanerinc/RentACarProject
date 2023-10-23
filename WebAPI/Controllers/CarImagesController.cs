using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImagesController : ControllerBase
    {
        private ICarImageService _carImageService;

        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll() 
        {
            var result = _carImageService.GetAll();

            if(result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int carImageId)
        {
            var result = _carImageService.GetById(carImageId);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromForm] IFormFile file, [FromForm] CarImage carImage)
        {
            var result = _carImageService.Add(file, carImage);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update([FromForm] IFormFile file, [FromForm] CarImage carImage)
        {
            var result = _carImageService.Update(file, carImage);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(CarImage carImage)
        {
            var carDeleteImage = _carImageService.GetById(carImage.CarImageId).Data;
            var result = _carImageService.Delete(carDeleteImage);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
