using BookStore.Api.Validators;
using BookStore.Models;
using FluentValidation;

namespace BookStore.Api;

public static class ServiceCollectionExtensions
{
    public static void AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IValidator<Book>, BookValidator>();
    }
}