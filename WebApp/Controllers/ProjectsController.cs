using Microsoft.AspNetCore.Mvc;
using Business.Models;
using Business.Services;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    [HttpGet("")]
    public async Task<IActionResult> Projects()
    {
        var projects = await _projectService.GetAllAsync();
        return View(projects); 
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                x => x.Key,
                x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(new { success = false, errors });
        }

        var success = await _projectService.CreateAsync(form);
        return Ok(new { success });
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit(EditProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                x => x.Key,
                x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return BadRequest(new { success = false, errors });
        }

        var success = await _projectService.UpdateAsync(form);
        return Ok(new { success });
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        await _projectService.DeleteAsync(id);
        return Ok(new { success = true });
    }
}

