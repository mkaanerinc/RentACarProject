using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("User Id is required.")
                .GreaterThan(0).WithMessage("User Id must be greater than zero.");

            RuleFor(c => c.CompanyName)
                .NotEmpty().WithMessage("Company Name is required.")
                .MinimumLength(2).WithMessage("Company Name minimum length must be two.");

        }
    }
}
