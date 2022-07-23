using System.Collections;
using AutoMapper;
using Server.Core.Aggregates;
using Server.Core.Models.Entities.Entity;

namespace Server.Core.Interfaces.Service; 

public class ProjectService : IProjectService {

    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper) {
        _projectRepository = projectRepository;
        this._mapper = mapper;
    }

    public IQueryable<Project> GetAllProjects(string accountId) {
        return _projectRepository.GetAllProjects(accountId);
        
    }

    public Project Create(ProjectCreateRequest project) {
        var projectEntity = _mapper.Map<Project>(project);
        var result =_projectRepository.Create(projectEntity);
        return result;
    }
}

public interface IProjectService {

    IQueryable<Project> GetAllProjects(string accountId);
    Project Create(ProjectCreateRequest project);
}
