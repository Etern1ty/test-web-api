namespace Test.Web.Api.Interfaces;

public interface IPagingModel
{
    int Take { get; set; }

    int Skip { get; set; }
}