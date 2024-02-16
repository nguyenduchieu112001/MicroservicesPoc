using FluentValidation;
using MediatR;
using PricingService.Api.Commands.Dtos;
using System;
using System.Collections.Generic;

namespace PricingService.Api.Commands;

public class CalculatePriceCommand : IRequest<CalculatePriceResult>
{
    public string ProductCode { get; set; }
    public DateTimeOffset PolicyFrom { get; set; }
    public DateTimeOffset PolicyTo { get; set; }
    public IList<string> SelectedCovers { get; set; }
    public IList<QuestionAnswer> Answers { get; set; }
}

public class CalculatePriceCommandValidator : AbstractValidator<CalculatePriceCommand>
{
    public CalculatePriceCommandValidator()
    {
        RuleFor(m => m.ProductCode).NotEmpty();
        RuleFor(m => m.SelectedCovers).NotNull();
        RuleFor(m => m.Answers).NotNull();
    }
}