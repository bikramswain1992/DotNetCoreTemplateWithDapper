namespace TemplateDapper.Application.Common.Constants;

public class DatabaseQueries
{
    public const string GetUserByEmailQuery = "select * from User where Email = @Email";
}
