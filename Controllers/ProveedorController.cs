using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace APIREST.Controllers;

[ApiController]
[Route("[controller]")]
public class ProveedorController : ControllerBase
{
    IProveedorDAO proveedorDAO;

    public ProveedorController(IProveedorDAO proveedorDAO)
    {
        this.proveedorDAO = proveedorDAO;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(proveedorDAO.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Proveedor proveedor)
    {
        return Ok(proveedorDAO.Insert(proveedor));
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Proveedor proveedor)
    {
        return Ok(proveedorDAO.Update(id,proveedor));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(proveedorDAO.Delete(id));
    }
}