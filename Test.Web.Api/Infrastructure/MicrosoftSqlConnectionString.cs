namespace Test.Web.Api.Infrastructure;

public class MicrosoftSqlConnectionString
{
  public string Value { get; }

  public MicrosoftSqlConnectionString(string value) => this.Value = value;
}