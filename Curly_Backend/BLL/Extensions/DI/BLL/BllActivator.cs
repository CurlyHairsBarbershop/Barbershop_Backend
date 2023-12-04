using BLL.Extensions.DI.UserServices;
using BLL.Services.Appointments;
using BLL.Services.Favor;
using BLL.Services.Reviews;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BLL.Extensions.DI.BLL;

public static class BllActivator 
{
    public static IServiceCollection AddBll(this IServiceCollection collection)
    {
        collection.TryAddScoped<ReviewService>();
        collection.TryAddScoped<IAppointmentService, AppointmentService>();
        collection.TryAddScoped<FavorService>();
        collection.AddUserReaders();

        return collection;
    }
}