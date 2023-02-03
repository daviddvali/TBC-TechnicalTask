using System.Text.Json;

namespace TBC.Task.API.ExceptionHandler;

internal record ErrorDetails(int StatusCode, string Message)
{
	public override string ToString() => JsonSerializer.Serialize(this);
}
