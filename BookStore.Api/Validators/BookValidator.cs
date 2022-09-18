using BookStore.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Validators
{
    public class BookValidator: AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Id).Empty();
            RuleFor(b => b.Author).NotEmpty();
            RuleFor(b => b.Title).NotEmpty();
            RuleFor(b => b.Price).NotEmpty().GreaterThanOrEqualTo(0).ScalePrecision(2,1000);
        }
    }
}
