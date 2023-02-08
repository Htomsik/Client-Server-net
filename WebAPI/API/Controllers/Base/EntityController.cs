using Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.Base;

namespace API.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public abstract class EntityController<T> : ControllerBase where T : Entity
{
    #region Fields
    private readonly IRepository<T> _repository;
    #endregion

    #region Constructors
    protected EntityController(IRepository<T> repository)
    {
        _repository = repository;
    }
    #endregion

    #region Methods
    #region Page interactons
    #region page[[{pageIndex}:{pageSize}]
    [HttpGet("page[[{pageIndex:int}:{pageSize:int}]]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IPageItem<T>>> GetPage(int pageIndex, int pageSize)
    {
        var result = await _repository.GetPage(pageIndex, pageSize);

        return result.Items.Any() ?
            Ok(result) : 
            NotFound(result);
    }
    #endregion
    #endregion
    
    #region Collection iteractons
    #region items

    [HttpGet("items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<T>>> GetMany() =>
        Ok(await _repository.GetMany());

    #endregion

    #region items[[{skip}:{count}]]

    [HttpGet("items[[{skip:int}:{count:int}]]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<T>>> GetMany(int skip, int count) =>
        Ok(await _repository.GetMany(skip, count));

    #endregion
    #endregion
    
    #region Item iteractons
    #region Add
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Add(T item)
    {
        if (!ModelState.IsValid)
            return BadRequest(item);
        
        if (await _repository.Update(item) is not { } result)
           return BadRequest(item);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    #endregion

    #region Update
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(T item)
    {
        if (!ModelState.IsValid)
            return BadRequest(item);
        
        if (await _repository.Update(item) is not { } result)
            return NotFound(item);

        return AcceptedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    #endregion

    #region Delete

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(T item)
    {
        if (await _repository.Delete(item) is not { } result)
            return NotFound(item);

        return Ok(result);
    }
    #endregion

    #region Delete{id}

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _repository.Delete(id) is not { } result)
            return NotFound(id);

        return Ok(result);
    }
   
    
    #endregion
    
    #region Get
    [HttpGet("{id:int}")]
    [ActionName("Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id) =>
        await _repository.Get(id) is { } item?
            Ok(item) :
            NotFound();
    #endregion
    #endregion

    #region Extensions
    #region exist/id
    [HttpGet("exist/id/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public async Task<IActionResult> Exist(int id) => 
        await _repository.Exist(id) ? 
            Ok(true) : 
            NotFound(false);
    #endregion

    #region exist
    [HttpPost("exist")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
    public async Task<IActionResult> Exist(T? item) =>
        await _repository.Exist(item) ? 
            Ok(true) : 
            NotFound(false);
    #endregion
    
    #region count

    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> Count() => Ok(await _repository.Count());

    #endregion
    #endregion
    #endregion
}