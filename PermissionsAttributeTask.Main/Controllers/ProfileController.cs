using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PermissionsAttributeTask.DataAccess.Entities;
using PermissionsAttributeTask.DataAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using PermissionsAttributeTask.DataAccess.Repositories.Abstractions;
using PermissionsAttributeTask.Main.Attributes;
using PermissionsAttributeTask.Main.Models;

namespace PermissionsAttributeTask.Main.Controllers;

[Controller]
public class ProfileController : Controller
{
    private readonly IProfilesRepository _profileRepository;

    public ProfileController(IProfilesRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    [HasPermission(Permissions.GetProfiles)]
    public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
    {
        var userProfiles = await _profileRepository.GetAllProfilesAsync(cancellationToken);

        return View("Index", userProfiles);
    }
    
    [HasPermission(Permissions.GetProfileById)]
    public async Task<IActionResult> GetUserProfile(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Id is required");
        }
    
        var userProfile = await _profileRepository.GetProfileByIdAsync(id, cancellationToken);
        if (userProfile == null)
        {
            return NotFound($"User profile with ID {id} not found");
        }
    
        return View("Details", userProfile);
    }
    
    [HasPermission(Permissions.AddUserProfile)]
    public IActionResult Create()
    {
        return View("Create");
    }
    
    [HttpPost]
    [HasPermission(Permissions.AddUserProfile)]
    public async Task<IActionResult> Create(UserProfile newUserProfile, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Create", newUserProfile);
        }
        newUserProfile.Id = Guid.NewGuid();
        await _profileRepository.AddProfileAsync(newUserProfile, cancellationToken);
    
        return RedirectToAction("GetAllProfiles");
    }
    
    [HasPermission(Permissions.UpdateUserProfile)]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("User profile ID is required");
        }
    
        var userProfile = await _profileRepository.GetProfileByIdAsync(id, cancellationToken);
        if (userProfile == null)
        {
            return NotFound($"User profile with ID {id} not found");
        }
    
        return View("Edit", userProfile);
    }
    
    [HttpPost]
    [HasPermission(Permissions.UpdateUserProfile)]
    public async Task<IActionResult> Edit(Guid id, UserProfile updatedUserProfile, CancellationToken cancellationToken)
    {
        if (id != updatedUserProfile.Id)
        {
            return BadRequest("ID in URL does not match ID in body");
        }
        if (!ModelState.IsValid)
        {
            return View("Edit", updatedUserProfile);
        }
    
        await _profileRepository.UpdateProfileAsync(updatedUserProfile, cancellationToken);
    
        return RedirectToAction("GetAllProfiles");
    }
    
    
    [HttpPost]
    [HasPermission(Permissions.DeleteUserProfile)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("User profile ID is required");
        }
    
        await _profileRepository.DeleteProfileAsync(id, cancellationToken);
    
        return RedirectToAction("GetAllProfiles");
    }
}