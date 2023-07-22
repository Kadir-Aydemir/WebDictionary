using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ImageFileValidator:AbstractValidator<ImageFile>
    {
        public ImageFileValidator()
        {
            RuleFor(x => x.ImageName).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.ImageName).MaximumLength(100).WithMessage("Image Name cannot exceed 50 characters!");
            RuleFor(x => x.ImageName).MinimumLength(3).WithMessage("Image Name cannot be shorter than 3 characters!");
        }    
    }
}
