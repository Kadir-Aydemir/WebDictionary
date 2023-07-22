using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class SkillValidator : AbstractValidator<Skill>
    {
        public SkillValidator()
        {
            RuleFor(x => x.SkillName).NotEmpty().WithMessage("You cannot leave this field blank!");
            RuleFor(x => x.SkillName).MaximumLength(100).WithMessage("Skill Name cannot exceed 100 characters!");
            RuleFor(x => x.SkillName).MinimumLength(3).WithMessage("Skill Name cannot be shorter than 3 characters!");

            RuleFor(x => x.SkillColor).NotEmpty().WithMessage("You cannot leave this field blank!");

        }
    }
}
