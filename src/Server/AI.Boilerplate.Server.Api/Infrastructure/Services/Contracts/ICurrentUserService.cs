namespace AI.Boilerplate.Server.Api.Infrastructure.Services.Contracts;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
}
