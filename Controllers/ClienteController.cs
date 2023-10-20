using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace APIREST.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    IClienteDAO clienteDAO;

    public ClienteController(IClienteDAO clienteDAO)
    {
        this.clienteDAO = clienteDAO;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(clienteDAO.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Cliente cliente)
    {
        return Ok(clienteDAO.Insert(cliente));
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Cliente cliente)
    {
        return Ok(clienteDAO.Update(id,cliente));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(clienteDAO.Delete(id));
    }
}