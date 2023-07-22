using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class BlockedValidator : AbstractValidator<Blocked>
    {
        public BlockedValidator()
        {
            RuleFor(x => x.Reason).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Reason).MaximumLength(200).WithMessage("Reason cannot exceed 200 characters!");
            RuleFor(x => x.Reason).MinimumLength(3).WithMessage("Reason cannot be shorter than 3 characters!");
        }

    }
}
