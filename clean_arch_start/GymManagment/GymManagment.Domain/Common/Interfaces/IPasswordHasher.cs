namespace GymManagment.Domain.Common.Interfaces;

using ErrorOr;

public interface IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password);
    bool IsCorrectPassword(string password, string hash);
}