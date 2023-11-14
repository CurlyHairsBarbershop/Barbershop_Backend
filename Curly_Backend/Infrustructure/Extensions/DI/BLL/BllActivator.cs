using BLL.Services.Appointments;
using BLL.Services.Favor;
using BLL.Services.Reviews;
using Infrustructure.Extensions.DI.UserServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrustructure.Extensions.DI.BLL;

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