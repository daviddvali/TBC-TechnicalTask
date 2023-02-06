using System.Text.Json;

namespace TBC.Task.API.ExceptionHandler;

internal sealed record ErrorDetails(int StatusCode, string Message)
{
	public override string ToString() => JsonSerializer.Serialize(this);
}
