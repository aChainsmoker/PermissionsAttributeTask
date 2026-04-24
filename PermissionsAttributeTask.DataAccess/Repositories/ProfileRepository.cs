using Microsoft.EntityFrameworkCore;
using PermissionsAttributeTask.DataAccess.DbContext;
using PermissionsAttributeTask.DataAccess.Entities;
using PermissionsAttributeTask.DataAccess.Repositories.Abstractions;

namespace PermissionsAttributeTask.DataAccess.Repositories;

public class ProfileRepository : IProfilesRepository
{
    private readonly PermissionsAttributeTaskDbContext _dbContext;

    public ProfileRepository(PermissionsAttributeTaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<UserProfile>> GetAllProfilesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserProfiles.ToListAsync(cancellationToken);
    }

    public async Task<UserProfile?> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserProfiles.FindAsync([id], cancellationToken);
    }

    public async Task AddProfileAsync(UserProfile profile, CancellationToken cancellationToken = default)
    {
        await _dbContext.UserProfiles.AddAsync(profile, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken = default)
    {
        _dbContext.UserProfiles.Update(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProfileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _dbContext.UserProfiles.Where(x=>x.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}