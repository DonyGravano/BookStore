using BookStore.Api.Validators;
using BookStore.Application;
using BookStore.Application.Interfaces;
using BookStore.Models;
using FluentAssertions.Common;
using FluentValidation;

namespace BookStore.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddValidators(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IValidator<Book>, BookValidator>();
        }
    }
}
