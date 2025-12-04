using GymManagment.Application.Subscriptions.Commands.CreateSubscription;
using GymManagment.Application.Subscriptions.Queries.GetSubscription;
using GymManagment.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
                    Enum.Parse<SubcriptionType>(subscription.SubcriptionType))),
            error => Problem());
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
    {
        var command = new CreateSubscriptionCommand(request.SubcriptionType.ToString(), request.AdminId);
        var createSubscriptionResult = await _mediator.Send(command);

        return createSubscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubcriptionType)),
            error => Problem());
    }
}