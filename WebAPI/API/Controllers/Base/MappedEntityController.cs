using AutoMapper;
using Interfaces.Entities;
using Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

[ApiController, Route("api/[controller]")]
public abstract class MappedEntityController<T,TBase> : ControllerBase 
    where T : IEntity
    where TBase : IEntity
{
    #region Fields
    private readonly IRepository<TBase> _repository;

    private readonly IMapper _mapper;
    #endregion

    #region Constructors
    public MappedEntityController(IRepository<TBase> repository, IMapper mapper)
    {
        _repository = repository;
        
        _mapper = mapper;
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
            Ok(ToItem(result)) : 
            NotFound();
    }
    
    protected record PageItem (IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : IPageItem<T>
    {
        public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    protected IPageItem<T> ToItem(IPageItem<TBase?> pageItem) => new PageItem(
        ToItem(pageItem.Items),
        pageItem.TotalCount,
        pageItem.PageIndex,
        pageItem.PageSize);
    #endregion
    #endregion
    
    #region Collection iteractons
    #region items
    [HttpGet("items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<T>>> GetMany() =>
        Ok(ToItem(await _repository.GetMany()));
    #endregion

    #region items[[{skip}:{count}]]
    [HttpGet("items[[{skip:int}:{count:int}]]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<T>>> GetMany(int skip, int count) =>
        Ok(ToItem(await _repository.GetMany(skip, count)));
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
        
        if (await _repository.Add(ToBase(item)) is not { } result)
            return BadRequest(item);
        
        return CreatedAtAction(nameof(Get), new { id = result.Id }, ToItem(result));
        
    }
    #endregion

    #region Update
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(T item)
    {
        if (!ModelState.IsValid)
            return BadRequest(item);
        
        if (await _repository.Update(ToBase(item)) is not { } result)
            return NotFound(item);

        return AcceptedAtAction(nameof(Get), new { id = result.Id },  ToBase(item));
    }
    #endregion

    #region Delete

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(T item)
    {
        if (await _repository.Delete(ToBase(item)) is not { } result)
            return NotFound(item);

        return Ok(ToItem(result));
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

        return Ok(ToItem(result));
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
    public async Task<IActionResult> Exist(T item) =>
        await _repository.Exist(ToBase(item)) ? 
            Ok(true) : 
            NotFound(false);
    #endregion
    
    #region count
    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> Count() => Ok(await _repository.Count());
    #endregion
    #endregion

    #region Helpers : private methods
    protected virtual TBase? ToBase(T item) => _mapper.Map<TBase>(item);
    
    protected virtual IEnumerable<TBase> ToBase(IEnumerable<T> items) => _mapper.Map<IEnumerable<TBase>>(items);

    protected virtual T? ToItem(TBase item) => _mapper.Map<T>(item);
    
    protected virtual IEnumerable<T> ToItem(IEnumerable<TBase?> items) => _mapper.Map<IEnumerable<T>>(items);
    #endregion

    #endregion
}