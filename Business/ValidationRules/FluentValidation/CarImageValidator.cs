using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CarImageValidator : AbstractValidator<CarImage>
    {
        public CarImageValidator()
        {
            RuleFor(c => c.CarId)
                .NotEmpty().WithMessage("Car Id is required.");
            RuleFor(c => c.ImagePath)
                .NotEmpty().WithMessage("ImagePath is required.");
            RuleFor(c => c.Date)
                .NotEmpty().WithMessage("Date is required.");
        }
    }
}
