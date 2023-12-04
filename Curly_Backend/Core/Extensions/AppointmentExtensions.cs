namespace Core.Extensions;

public static class AppointmentExtensions
{
    public static void Cancel(this Appointment source)
    {
        source.IsCancelled = true;
    }
}