using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    [HttpGet("members")]
    public IActionResult Members() => View();

    [HttpGet("clients")]
    public IActionResult Clients() => View();


    [HttpPost("clients/add")]
    public IActionResult AddClient(AddClientForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        return Ok(new { success = true });
    }


    [HttpPost("clients/edit/{id}")]
    public IActionResult EditClient(int id, EditClientForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }
        return Ok(new { success = true });
    }

    [HttpPost("projects/add")]
    public IActionResult AddProject(AddProjectForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                errors = ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                )
            });

        return Ok(new { success = true });
    }

    [HttpPost("projects/edit")]
    public IActionResult EditProject(EditProjectForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest(new
            {
                errors = ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                )
            });

        return Ok(new { success = true });
    }
    
}

