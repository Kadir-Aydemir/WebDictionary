using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Contact Name cannot exceed 50 characters!");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Contact Name cannot be shorter than 3 characters!");

            RuleFor(x => x.Mail).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Mail).MaximumLength(50).WithMessage("Contact Mail cannot exceed 50 characters!");
            RuleFor(x => x.Mail).MinimumLength(3).WithMessage("Contact Mail cannot be shorter than 3 characters!");

            RuleFor(x => x.Subject).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Subject).MaximumLength(50).WithMessage("Subject cannot exceed 50 characters!");
            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Subject cannot be shorter than 3 characters!");

            RuleFor(x => x.Message).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Message).MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters!");
            RuleFor(x => x.Message).MinimumLength(3).WithMessage("Message cannot be shorter than 3 characters!");
        }
    }
}
