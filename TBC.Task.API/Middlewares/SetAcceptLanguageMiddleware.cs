namespace TBC.Task.API.Middlewares;

internal sealed class SetAcceptLanguageMiddleware
{
	private readonly RequestDelegate _next;

	public SetAcceptLanguageMiddleware(RequestDelegate next) => _next = next;

	public async System.Threading.Tasks.Task InvokeAsync(HttpContext context)
	{
		context.Request.Headers.TryAdd("Accept-Language", "en-US");
		await _next(context);
	}
}
