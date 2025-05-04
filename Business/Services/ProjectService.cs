using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Business.Models;
using Business.Interfaces;

namespace Business.Services;

public class ProjectService(AppDbContext context) : IProjectService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Client)
            .ToListAsync();
    }

    public async Task<ProjectEntity?> GetAsync(string id)
    {
        return await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(ProjectEntity entity)
    {
        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
        {
            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CreateAsync(AddProjectForm form)
    {
        try
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientName == form.ClientName);
            if (client == null)
            {
                client = new ClientEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientName = form.ClientName
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            var entity = new ProjectEntity
            {
                Id = Guid.NewGuid().ToString(),
                ProjectName = form.ProjectName,
                Description = form.Description,
                StartDate = DateTime.Parse(form.StartDate),
                EndDate = DateTime.TryParse(form.EndDate, out var endDate) ? endDate : null,
                Budget = decimal.TryParse(form.Budget, out var budget) ? budget : null,
                Created = DateTime.Now,
                ClientId = client.Id,
                UserID = _context.Users.First().Id,
                StatusId = _context.Statuses.First().Id
            };

            _context.Projects.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    //public async Task<bool> UpdateAsync(EditProjectForm form)
    //{
    //    try
    //    {
    //        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == form.Id);
    //        if (project == null)
    //            return false;

    //        var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientName == form.ClientName);
    //        if (client == null)
    //        {
    //            client = new ClientEntity
    //            {
    //                Id = Guid.NewGuid().ToString(),
    //                ClientName = form.ClientName
    //            };
    //            _context.Clients.Add(client);
    //            await _context.SaveChangesAsync();
    //        }

    //        project.ProjectName = form.ProjectName;
    //        project.Description = form.Description;
    //        project.StartDate = DateTime.Parse(form.StartDate);
    //        project.EndDate = DateTime.TryParse(form.EndDate, out var endDate) ? endDate : null;
    //        project.Budget = decimal.TryParse(form.Budget, out var budget) ? budget : null;
    //        project.ClientId = client.Id;

    //        await _context.SaveChangesAsync();
    //        return true;
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}

    public async Task<bool> UpdateAsync(EditProjectForm form)
    {
        try
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == form.Id);
            if (project == null) return false;


            project.ProjectName = form.ProjectName;
            project.Description = form.Description;
            project.StartDate = DateTime.Parse(form.StartDate);
            project.EndDate = DateTime.TryParse(form.EndDate, out var endDate) ? endDate : null;
            project.Budget = decimal.TryParse(form.Budget, out var budget) ? budget : null;

            
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientName == form.ClientName);
            if (client == null)
            {
                client = new ClientEntity { Id = Guid.NewGuid().ToString(), ClientName = form.ClientName };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            project.ClientId = client.Id;

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

}