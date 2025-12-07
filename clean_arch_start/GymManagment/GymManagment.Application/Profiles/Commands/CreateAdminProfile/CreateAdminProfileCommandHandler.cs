using MediatR;
using ErrorOr;
using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Admins;

namespace GymManagment.Application.Profiles.Commands.CreateAdminProfile;

public class CreateAdminProfileCommandHandler(
    IUsersRepository _usersRepository,
    IAdminsRepository _adminsRepository,
    IUnitOfWork _unitOfWork,
    ICurrentUserProvider _currentUserProvider
) : IRequestHandler<CreateAdminProfileCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateAdminProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(command.UserId);
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser.Id != command.UserId)
        {
            return Error.Unauthorized("User not found");
        }

        if (user is null)
        {
            return Error.NotFound("User not found");
        }

        var createAdminProfileResult = user.CreateAdminProfile();
        var admin = new Admin(user.Id, createAdminProfileResult.Value);

        await _usersRepository.UpdateAsync(user);
        await _adminsRepository.AddAdminAsync(admin);
        await _unitOfWork.CommitChangesAsync();

        return createAdminProfileResult;
    }
}