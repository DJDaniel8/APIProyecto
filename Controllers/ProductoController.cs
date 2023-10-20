using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace APIREST.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductoController : ControllerBase
{
    IProductoDAO productoDAO;

    public ProductoController(IProductoDAO productoDAO)
    {
        this.productoDAO = productoDAO;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(productoDAO.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Producto producto)
    {
        return Ok(productoDAO.Insert(producto));
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Producto producto)
    {
        return Ok(productoDAO.Update(id,producto));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(productoDAO.Delete(id));
    }
}