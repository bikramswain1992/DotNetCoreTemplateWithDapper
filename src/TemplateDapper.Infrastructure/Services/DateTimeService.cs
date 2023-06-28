using TemplateDapper.Application.Interfaces;

namespace TemplateDapper.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;
}
