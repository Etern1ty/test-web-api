using Test.Web.Api.Data;
using Test.Web.Api.Infrastructure;
using Test.Web.Api.Infrastructure.Models;
using Test.Web.Api.Models;

namespace Test.Web.Api.Domain;

public interface IObjectManager
{
    Task<int> AddObjects(List<ObjectInsertModel> items);
    Task<PagedListModel<ObjectItemModel>> GetObjects(ObjectFilterModel filter);
}

public class ObjectManager : IObjectManager
{
    private readonly ILogger<ObjectManager> _logger;
    private readonly IObjectRepository _objectRepository;

    public ObjectManager(ILogger<ObjectManager> logger, IObjectRepository objectRepository)
    {
        _logger = logger;
        _objectRepository = objectRepository;
    }

    public async Task<int> AddObjects(List<ObjectInsertModel> items)
    {
        try
        {
            if (items.Count == 0)
            {
                _logger.LogWarning("Empty array in {methodName}. Items count: {items}", nameof(_objectRepository.AddObjects), items.Count);
                throw new Exception();
            }
            
            items.Sort((x, y) => x.Code.CompareTo(y.Code));
            return await _objectRepository.AddObjects(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Invalid {methodName} response. Items count: {items}", nameof(_objectRepository.AddObjects), items.Count);
            throw;
        }
    }

    public async Task<PagedListModel<ObjectItemModel>> GetObjects(ObjectFilterModel filter)
    {
        try
        {
            return await _objectRepository.GetObjects(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Invalid {methodName} response. Filters: {filter}", nameof(_objectRepository.GetObjects), filter.ToString());
            throw;
        }
    }
}