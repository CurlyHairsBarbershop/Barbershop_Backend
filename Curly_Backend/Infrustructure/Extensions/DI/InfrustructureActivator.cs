using Infrustructure.Services.Emails;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrustructure.Extensions.DI;

public static class InfrustructureActivator
{
    public static IServiceCollection AddEmailProcessing(this IServiceCollection collection)
    {
        collection.TryAddScoped<EmailSender>();

        return collection;
    }
}