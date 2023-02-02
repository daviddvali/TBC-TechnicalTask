﻿using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TBC.Task.API.Swagger;

public class AcceptLanguageFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		operation.Parameters ??= new List<OpenApiParameter>();

		operation.Parameters.Add(new OpenApiParameter
		{
			Name = "Accept-Language",
			In = ParameterLocation.Header,
			Required = false,
			Schema = new OpenApiSchema
			{
				Type = "string",
				Default = new OpenApiString("en-US")
			}
		});
	}
}
