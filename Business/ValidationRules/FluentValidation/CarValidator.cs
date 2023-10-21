using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(c => c.BrandId)
                .NotEmpty().WithMessage("Brand Id is required.")
                .GreaterThan(0).WithMessage("Brand Id must be greater than zero.");

            RuleFor(c => c.ColorId)
                .NotEmpty().WithMessage("Color Id is required.")
                .GreaterThan(0).WithMessage("Color Id must be greater than zero.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Car name is required.")
                .MinimumLength(2).WithMessage("Car name minimum length must be two.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Car description is required.")
                .MinimumLength(2).WithMessage("Car description minimum length must be two.");

            RuleFor(c => c.ModelYear)
                .NotEmpty().WithMessage("Model Year is required.")
                .GreaterThan(0).WithMessage("Model Year must be greater than zero.");

            RuleFor(c => c.DailyPrice)
                .NotEmpty().WithMessage("Daily Price is required.")
                .GreaterThan(0).WithMessage("Daily Price must be greater than zero.");
        }
    }
}
