using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectEntity>> GetAllAsync();
    Task<ProjectEntity?> GetAsync(string id);
    Task<bool> CreateAsync(AddProjectForm form);
    Task<bool> UpdateAsync(EditProjectForm form);
    Task DeleteAsync(string id);
}
