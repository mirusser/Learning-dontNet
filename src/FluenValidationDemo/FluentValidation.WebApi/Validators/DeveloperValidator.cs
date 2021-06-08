using ExternalModels.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidation.WebApi.Validators
{
    public class DeveloperValidator : AbstractValidator<Developer>
    {
        public DeveloperValidator()
        {
            RuleFor(p => p.FirstName)
                .Cascade(CascadeMode.Stop) //to show only the first error
                .NotEmpty().WithMessage("{PropertyName} should be not empty.")
                .Length(2, 25)
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(p => p.LastName)
                .Cascade(CascadeMode.Stop) //to show only the first error
                .NotEmpty().WithMessage("{PropertyName} should be not empty.")
                .Length(2, 25)
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(p => p.Email)
               .EmailAddress();
        }

        private bool IsValidName(string name)
        {
            return name.All(Char.IsLetter);
        }
    }
}
