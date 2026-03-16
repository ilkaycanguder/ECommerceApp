using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Log.Application.Features.Logs.Commands.CreateLog
{
    public sealed class CreateLogCommandValidator : AbstractValidator<CreateLogCommand>
    {
        public CreateLogCommandValidator()
        {
            RuleFor(x => x.Level)
                .NotEmpty().WithMessage("Log level is required.")
                .Must(x => new[] { "INFO", "WARNING", "ERROR", "CRITICAL" }.Contains(x))
                .WithMessage("Log level must be INFO, WARNING, ERROR or CRITICAL.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Log message is required.")
                .MaximumLength(1000).WithMessage("Log message must not exceed 1000 characters.");

            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Log source is required.")
                .MaximumLength(100).WithMessage("Log source must not exceed 100 characters.");
        }
    }
}
