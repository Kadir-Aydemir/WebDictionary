using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class DraftMessageValidator:AbstractValidator<DraftMessage>
    {
        public DraftMessageValidator()
        {
            RuleFor(x => x.DraftReceiverMail).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.DraftReceiverMail).MaximumLength(50).WithMessage("Receiver Mail cannot exceed 50 characters!");
            RuleFor(x => x.DraftReceiverMail).MinimumLength(3).WithMessage("Receiver Mail cannot be shorter than 3 characters!");

            //RuleFor(x => x.SenderMail).NotEmpty().WithMessage("You cannot leave this field blank!");
            //RuleFor(x => x.SenderMail).MaximumLength(50).WithMessage("Sender Mail cannot exceed 50 characters!");
            //RuleFor(x => x.SenderMail).MinimumLength(3).WithMessage("Sender Mail cannot be shorter than 3 characters!");

            RuleFor(x => x.DraftSubject).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.DraftSubject).MaximumLength(100).WithMessage("Subject cannot exceed 100 characters!");
            RuleFor(x => x.DraftSubject).MinimumLength(3).WithMessage("Subject cannot be shorter than 3 characters!");

            RuleFor(x => x.DraftMessageContent).NotEmpty().WithMessage("You cannot leave this field blank!");
            //RuleFor(x => x.DraftMessageContent).MaximumLength(1000).WithMessage("Message Content cannot exceed 1000 characters!");
            RuleFor(x => x.DraftMessageContent).MinimumLength(3).WithMessage("Message Content cannot be shorter than 3 characters!");
        }
    }
}
