using BLL.Extensions.DI.UserServices;
using BLL.Services.Appointments;
using BLL.Services.Favor;
using BLL.Services.Reviews;
using BLL.Services.Users.Barbers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BLL.Extensions.DI;

public static class BllActivator 
{
    public static IServiceCollection AddBll(this IServiceCollection collection)
    {
        collection.TryAddScoped<ReviewService>();
        collection.TryAddScoped<IAppointmentService, AppointmentService>();
        collection.TryAddScoped<FavorService>();

        collection.AddBarbersManagement();
        collection.AddUserReaders();

        return collection;
    }

    private static IServiceCollection AddBarbersManagement(this IServiceCollection collection)
    {
        collection.TryAddScoped<BarberService>();
        collection.TryAddScoped<BarberMediaService>();

        return collection;
    }
}