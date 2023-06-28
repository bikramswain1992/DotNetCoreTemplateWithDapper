using System.Text.Json.Serialization;

namespace TemplateDapper.Api.Extensions;

public static class JsonSerializationSettingsExtension
{
    public static IMvcBuilder ConfigureSerialization(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return mvcBuilder;
    }
}
