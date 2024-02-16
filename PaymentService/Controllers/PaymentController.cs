using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Api.Queries;
using PolicyService.Api.Events;
using System.Threading.Tasks;

namespace PaymentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IMediator bus;

    public PaymentController(IMediator bus)
    {
        this.bus = bus;
    }

    [HttpGet("accounts/{policyNumber}")]
    public async Task<ActionResult> AccountBalance(string policyNumber)
    {
        var result = await bus.Send(new GetAccountBalanceQuery { PolicyNumber = policyNumber });
        return new JsonResult(result);
    }

    [HttpPost("accounts/create")]
    public async Task<ActionResult> PolicyCreated([FromBody] PolicyCreated policy)
    {
        await bus.Publish(policy);
        return Ok("Policy created successfully.");
    }
}