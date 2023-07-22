using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator:AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.ReceiverMail).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.ReceiverMail).MaximumLength(50).WithMessage("Receiver Mail cannot exceed 50 characters!");
            RuleFor(x => x.ReceiverMail).MinimumLength(3).WithMessage("Receiver Mail cannot be shorter than 3 characters!");

            //RuleFor(x => x.SenderMail).NotEmpty().WithMessage("You cannot leave this field blank!");
            //RuleFor(x => x.SenderMail).MaximumLength(50).WithMessage("Sender Mail cannot exceed 50 characters!");
            //RuleFor(x => x.SenderMail).MinimumLength(3).WithMessage("Sender Mail cannot be shorter than 3 characters!");

            RuleFor(x => x.Subject).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.Subject).MaximumLength(100).WithMessage("Subject cannot exceed 100 characters!");
            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Subject cannot be shorter than 3 characters!");

            RuleFor(x => x.MessageContent).NotEmpty().WithMessage("You cannot leave this field blank!");
            //RuleFor(x => x.MessageContent).MaximumLength(1000).WithMessage("Message Content cannot exceed 1000 characters!");
            RuleFor(x => x.MessageContent).MinimumLength(3).WithMessage("Message Content cannot be shorter than 3 characters!");
        }
    }
}
