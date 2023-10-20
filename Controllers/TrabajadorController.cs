using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace APIREST.Controllers;

[ApiController]
[Route("[controller]")]
public class TrabajadorController : ControllerBase
{
    ITrabajadorDAO trabajadorDAO;

    public TrabajadorController(ITrabajadorDAO trabajadorDAO)
    {
        this.trabajadorDAO = trabajadorDAO;
    }

    [HttpPost]
    [Route("RequestAccess")]
    public IActionResult RequestAccess([FromBody] Trabajador trabajador){
        return Ok(trabajadorDAO.HasAccess(trabajador.usuario, trabajador.contraseña));
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(trabajadorDAO.Get());
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(trabajadorDAO.Delete(id));
    }
}