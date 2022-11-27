using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Services;
using FluentValidation;

namespace BuberDinner.Application.Breakfast.Commands;

public class CreateBreakfastCommandValidator : AbstractValidator<CreateBreakfastCommand>
{
	public CreateBreakfastCommandValidator(IDateTimeProvider dateTimeProvider)
	{
		RuleFor(x => x.Name).Length(3, 10);
		RuleFor(x => x.StartDateTime).Must(x => x > dateTimeProvider.UtcNow);
		RuleFor(x => x.EndDateTime).GreaterThan(x => x.StartDateTime);

		RuleForEach(x => x.Savory)
			.Must(NotBeEmpty)
			.WithMessage("Savory must have at least one item");

		RuleFor(x => x.StartDateTime).AfterSunrise(dateTimeProvider);
	}

	private static bool NotBeEmpty(string savoryItem)
	{
		return savoryItem.Length > 0;
	}
}

public static class DateTimeValidators
{
	public static IRuleBuilderOptions<T, DateTime> AfterSunrise<T>(
		this IRuleBuilder<T, DateTime> ruleBuilder,
		IDateTimeProvider dateTimeProvider)
	{
		var sunrise = TimeOnly.MinValue.AddHours(6);

		return ruleBuilder
			.Must((objectRoot, dateTime, context) =>
			{
				TimeOnly providedTime = TimeOnly.FromDateTime(dateTime);

				context.MessageFormatter.AppendArgument("Sunrise", sunrise);
				context.MessageFormatter.AppendArgument("ProvidedTime", providedTime);

				return providedTime > sunrise;
			})
			.WithMessage("{PropertyName} must be after {Sunrise}. You provided {ProvidedTime}");
	}
}


