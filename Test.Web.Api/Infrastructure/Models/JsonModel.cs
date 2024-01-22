using System.Text.Json;

namespace Test.Web.Api.Infrastructure;

public abstract class JsonModel
{
    public override string ToString() => JsonSerializer.Serialize(this);
}