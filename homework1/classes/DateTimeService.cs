namespace homework1.classes;
using homework1.interfaces;

public class DateTimeService : IDateTimeService
{
    public DateTime Time {get;} = DateTime.Now;
}