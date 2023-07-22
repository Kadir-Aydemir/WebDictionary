using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class AboutValidator : AbstractValidator<About>
    {
        public AboutValidator()
        {
            RuleFor(x => x.Details1).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Details1).MaximumLength(1000).WithMessage("Detail 1 cannot exceed 1000 characters!");
            RuleFor(x => x.Details1).MinimumLength(3).WithMessage("Detail 1 cannot be shorter than 3 characters!");

            RuleFor(x => x.Details2).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Details2).MaximumLength(1000).WithMessage("Detail 2 cannot exceed 1000 characters!");
            RuleFor(x => x.Details2).MinimumLength(3).WithMessage("Detail 2 cannot be shorter than 3 characters!");
        }
    }
}
