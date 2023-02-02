using Microsoft.OpenApi.Models;

namespace TBC.Task.API.Swagger;

internal static class SwaggerConfigureExtensions
{
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(c =>
        {
	        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TBC Task API", Version = "v1" });
	        c.OperationFilter<AcceptLanguageFilter>();
        });
    }
}
