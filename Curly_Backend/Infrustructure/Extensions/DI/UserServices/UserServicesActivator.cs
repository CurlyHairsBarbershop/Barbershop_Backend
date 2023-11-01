using BLL.Services.Users;
using BLL.Services.Users.Readers;
using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrustructure.Extensions.DI.UserServices;

public static class UserServicesActivator
{
    public static IServiceCollection AddUserReaders(this IServiceCollection collection)
    {
        collection.TryAddScoped<IUserReader<Barber>, BarberReader>();
        collection.TryAddScoped<BarberService>();
        
        return collection;
    }
}