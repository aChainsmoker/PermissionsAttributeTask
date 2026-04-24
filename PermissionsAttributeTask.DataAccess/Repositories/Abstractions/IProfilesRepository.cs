using Microsoft.AspNetCore.Identity;
using PermissionsAttributeTask.DataAccess.Entities;

namespace PermissionsAttributeTask.DataAccess.Repositories.Abstractions;

public interface IProfilesRepository
{
    Task<List<UserProfile>> GetAllProfilesAsync(CancellationToken cancellationToken = default);
    Task<UserProfile?> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddProfileAsync(UserProfile profile, CancellationToken cancellationToken = default);
    Task UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken = default);
    Task DeleteProfileAsync(Guid id, CancellationToken cancellationToken = default);
}