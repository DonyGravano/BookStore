using BookStore.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IBookService, BookService>();
    }
}