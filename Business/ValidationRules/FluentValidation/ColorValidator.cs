using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class ColorValidator : AbstractValidator<Color>
    {
        public ColorValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Color name is required.")
                .MinimumLength(2).WithMessage("Color name minimum length must be two.");
        }
    }
}
