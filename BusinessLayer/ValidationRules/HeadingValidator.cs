using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class HeadingValidator : AbstractValidator<Heading>
    {
        public HeadingValidator()
        {
            RuleFor(x => x.HeadingName).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.HeadingName).MaximumLength(50).WithMessage("Heading name cannot exceed 50 characters!");
            RuleFor(x => x.HeadingName).MinimumLength(3).WithMessage("Heading name cannot be shorter than 3 characters!");

            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("You cannot leave this field blank!");

            RuleFor(x => x.WriterID).NotEmpty().WithMessage("You cannot leave this field blank!");
        }
    }
}
