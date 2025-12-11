using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SubscriptionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler: IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly IAdminsRepository _adminsRepository;

    public CreateSubscriptionCommandHandler(IAdminsRepository adminsRepository)
    {
        _adminsRepository = adminsRepository;
    }

    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var admin = await _adminsRepository.GetByIdAsync(command.AdminId);

        if (admin is null)
        {
            return Error.NotFound("Admin not found");
        }

        if (admin.SubscriptionId is not  null)
        {
            return Error.Conflict("Admin already has active subscription");
        }
        
        var subscription = new Subscription(command.SubscriptionType, command.AdminId);
        admin.SetSubscription(subscription);
        
        await _adminsRepository.UpdateAsync(admin);

        return subscription;
    }
}