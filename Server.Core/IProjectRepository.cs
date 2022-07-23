using Server.Core.Models.Entities.Entity;

namespace Server.Core;

public interface IProjectRepository {

    IQueryable<Project> GetAllProjects(string accountId);
    Project Create(Project project);
}
