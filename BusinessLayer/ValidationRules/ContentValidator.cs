using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ContentValidator:AbstractValidator<Content>
    {
        public ContentValidator()
        {
            RuleFor(x => x.ContentCaption).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.ContentCaption).MaximumLength(1000).WithMessage("Content caption cannot exceed 1000 characters!");
            RuleFor(x => x.ContentCaption).MinimumLength(3).WithMessage("Content caption cannot be shorter than 3 characters!");

        }
    }
}
