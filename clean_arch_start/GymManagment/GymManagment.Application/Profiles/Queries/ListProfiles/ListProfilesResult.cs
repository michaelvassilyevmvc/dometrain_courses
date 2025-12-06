namespace GymManagment.Application.Profiles.Queries.ListProfiles;

public record ListProfilesResult(
    Guid? AdminId,
    Guid? ParticipantId,
    Guid? TrainerId);