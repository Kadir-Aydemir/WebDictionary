using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class WriterValidator : AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(x => x.WriterName).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.WriterName).MaximumLength(50).WithMessage("Writer name cannot exceed 50 characters!");
            RuleFor(x => x.WriterName).MinimumLength(3).WithMessage("Writer name cannot be shorter than 3 characters!");

            RuleFor(x => x.WriterSurname).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.WriterSurname).MaximumLength(50).WithMessage("Writer surname cannot exceed 50 characters!");
            RuleFor(x => x.WriterSurname).MinimumLength(2).WithMessage("Writer surname cannot be shorter than 2 characters!");

            RuleFor(x => x.WriterMail).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.WriterMail).MaximumLength(50).WithMessage("Writer mail cannot exceed 50 characters!");
            RuleFor(x => x.WriterMail).MinimumLength(10).WithMessage("Writer mail cannot be shorter than 10 characters!");
            RuleFor(x => x.WriterMail).EmailAddress().WithMessage("Please enter an email address in a valid format!");

            RuleFor(x => x.WriterPassword).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.WriterPassword).MaximumLength(50).WithMessage("Writer password cannot exceed 50 characters!");
            RuleFor(x => x.WriterPassword).MinimumLength(6).WithMessage("Writer password cannot be shorter than 6 characters!");

            //RuleFor(x => x.WriterAbout).NotEmpty().WithMessage("You cannot leave this field blank!");
            //RuleFor(x => x.WriterAbout).MaximumLength(100).WithMessage("Writer about cannot exceed 100 characters!");
            //RuleFor(x => x.WriterAbout).MinimumLength(3).WithMessage("Writer about cannot be shorter than 3 characters!");

            RuleFor(x => x.WriterTitle).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.WriterTitle).MaximumLength(50).WithMessage("Writer title cannot exceed 50 characters!");
            RuleFor(x => x.WriterTitle).MinimumLength(3).WithMessage("Writer title cannot be shorter than 3 characters!");
        }
    }
}
