using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.CategoryName).MaximumLength(50).WithMessage("Category name cannot exceed 50 characters!");
            RuleFor(x => x.CategoryName).MinimumLength(3).WithMessage("Category name cannot be shorter than 3 characters!");

            RuleFor(x => x.CategoryDescription).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.CategoryDescription).MaximumLength(200).WithMessage("Category description cannot exceed 200 characters!");
            RuleFor(x => x.CategoryDescription).MinimumLength(3).WithMessage("Category description cannot be shorter than 3 characters!");
        }
    }
}
