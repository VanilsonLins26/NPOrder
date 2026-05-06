using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NP_Encomendas_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet]
    public ActionResult Ping()
    {
        return Ok("O ping funcionou com sucesso");
    }
}
