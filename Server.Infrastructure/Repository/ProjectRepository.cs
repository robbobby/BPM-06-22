using System.Collections;
using Microsoft.Extensions.Logging;
using Server.Core;
using Server.Core.Models.Entities.Entity;

namespace Domain.Repository;

public class ProjectRepository : IProjectRepository {
    private readonly BmpDbContext _dbContext;
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectRepository(BmpDbContext dbContext, ILogger<ProjectRepository> logger) {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IQueryable<Project> GetAllProjects(string accountId) {
        var projects = _dbContext.Projects.Where(p => p.Account.Id.ToString() == accountId);
        return projects;
    }

    public Project Create(Project project) {
        var result = _dbContext.Projects.Add(project);
        _dbContext.SaveChanges();

        return result.Entity;
    }
}
