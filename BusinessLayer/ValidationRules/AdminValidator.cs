using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class AdminValidator:AbstractValidator<Admin>
    {
        public AdminValidator()
        {
            RuleFor(x => x.AdminUserName).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.AdminUserName).MaximumLength(50).WithMessage("User Name cannot exceed 50 characters!");
            RuleFor(x => x.AdminUserName).MinimumLength(3).WithMessage("User Name cannot be shorter than 3 characters!");

            RuleFor(x => x.AdminPassword).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.AdminPassword).MaximumLength(50).WithMessage("Password cannot exceed 50 characters!");
            RuleFor(x => x.AdminPassword).MinimumLength(3).WithMessage("Password Name cannot be shorter than 3 characters!");

            RuleFor(x => x.AdminRole).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.AdminRole).MaximumLength(1).WithMessage("Role cannot exceed 1 characters!");

        }
    }
}
