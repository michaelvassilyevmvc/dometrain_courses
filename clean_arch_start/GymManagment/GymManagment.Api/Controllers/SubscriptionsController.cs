using GymManagment.Application.Subscriptions.Commands.CreateSubscription;
using GymManagment.Application.Subscriptions.Queries.GetSubscription;
using GymManagment.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DomainSubscriptionType = GymManagment.Domain.Subscriptions.SubscriptionType;

namespace GymManagment.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISender _mediator;

    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{subscriptionId:guid}")]
    public async Task<IActionResult> GetSubscription(Guid subscriptionId)
    {
        var query = new GetSubscriptionQuery(subscriptionId);
        var getSubscriptionResult = await _mediator.Send(query);
        return getSubscriptionResult.MatchFirst(
            subscription =>
                Ok(new SubscriptionResponse(subscription.Id,
                    Enum.Parse<SubcriptionType>(subscription.SubscriptionType.Name))),
            error => Problem());
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
    {
        if (!DomainSubscriptionType.TryFromName(request.SubcriptionType.ToString(), out var subscriptionType))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid Subscription Type");
        }
        var command = new CreateSubscriptionCommand(subscriptionType, request.AdminId);
        var createSubscriptionResult = await _mediator.Send(command);

        return createSubscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubcriptionType)),
            error => Problem());
    }
}