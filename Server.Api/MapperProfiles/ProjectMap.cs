using Server.Core.Aggregates;
using Server.Core.Models.Entities.Entity;

namespace Api.MapperProfiles;

public class ProjectMap : AutoMapperProfile {
    public ProjectMap() {
        CreateMap<ProjectCreateRequest, Project>();
        CreateMap<Project, ProjectCreateRequest>();
    }
}