using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Test.Web.Api.Domain;
using Test.Web.Api.Models;

namespace Test.Web.Api.Controllers;

[ApiController]
[Route("api/v1/objects")]
public class ObjectsController : Controller
{
    private readonly IObjectManager _objectManager;

    public ObjectsController(IObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Добавляет список объектов")]
    [ProducesResponseType(typeof(int),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddObjects([FromBody]List<ObjectInsertModel> items)
    {
        try
        {
            var response = await _objectManager.AddObjects(items);
            return Content(JsonSerializer.Serialize(response));
        }
        catch
        {
            return BadRequest("Не удалось добавить список объектов.");
        }
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Возвращает список объектов по заданным фильтрам")]
    [ProducesResponseType(typeof(List<ObjectItemModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetObjects([FromQuery]ObjectFilterModel filter)
    {
        try
        {
            var response = await _objectManager.GetObjects(filter);
            return Content(JsonSerializer.Serialize(response));
        }
        catch
        {
            return BadRequest("Не удалось получить список объектов.");
        }
    }
}