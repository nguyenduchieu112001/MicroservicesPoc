using MediatR;
using Microsoft.AspNetCore.Mvc;
using PricingService.Api.Commands;
using PricingService.Api.Queries;
using System;
using System.Threading.Tasks;

namespace PricingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PricingController : ControllerBase
{
    private readonly IMediator bus;

    public PricingController(IMediator bus)
    {
        this.bus = bus;
    }

    [HttpGet("{code}")]
    public async Task<ActionResult> GetByCode([FromRoute] string code)
    {
        var result = await bus.Send(new FindCalculateByCodeQuery { ProductCode = code });
        return new JsonResult(result);
    }

    // POST api/values
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CalculatePriceCommand cmd)
    {
        var result = await bus.Send(cmd);
        return new JsonResult(result);
    }

    [HttpPost("create")]
    public async Task<ActionResult> PostCalculate([FromBody] CreateCalculatePriceCommand cmd)
    {
        var result = await bus.Send(cmd);
        if (result.PriceId == Guid.Empty)
        {
            return BadRequest(new { message = $"Price have exists with code {cmd.Code}" });
        }
        return new JsonResult(result);
    }

    [HttpPost("update")]
    public async Task<ActionResult> PostUpdate([FromBody] UpdateCalculatePriceCommand cmd)
    {
        var result = await bus.Send(cmd);
        if (result.PriceId == Guid.Empty)
        {
            return BadRequest(new { message = $"Not found price with code {cmd.Code}" });
        }
        return new JsonResult(result);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> Delete([FromQuery] DeleteCalculatePriceCommand cmd)
    {
        var result = await bus.Send(cmd);
        if (result.PriceId != Guid.Empty)
        {
            return NoContent();
        }
        return BadRequest(new { message = $"Not found price with code {cmd.Code}" });
    }
}