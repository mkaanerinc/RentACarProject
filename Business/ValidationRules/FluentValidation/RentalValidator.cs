using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class RentalValidator : AbstractValidator<Rental>
    {
        public RentalValidator()
        {
            RuleFor(r => r.CarId)
                .NotEmpty().WithMessage("Car Id is required.")
                .GreaterThan(0).WithMessage("Car Id must be greater than zero.");

            RuleFor(r => r.CustomerId)
                .NotEmpty().WithMessage("Customer Id is required.")
                .GreaterThan(0).WithMessage("Customer Id must be greater than zero.");

            RuleFor(r => r.RentDate)
                .NotEmpty().WithMessage("Rent Date is required.");

        }
    }
}
