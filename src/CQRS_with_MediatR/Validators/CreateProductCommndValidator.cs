using CQRS_with_MediatR.Features.ProductFeatures.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS_with_MediatR.Validators 
{
    public class CreateProductCommndValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommndValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Barcode).NotEmpty();
        }
    }
}
