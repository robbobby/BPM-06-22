using Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Aggregates;
using Server.Core.Interfaces.Service;

namespace Api.Controllers; 

[Route("api/[controller]/[action]")]

public class ProjectController : ControllerBase {
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(IProjectService projectService, ILogger<ProjectController> logger) {
        _projectService = projectService;
        _logger = logger;
    }
    
    [HttpPost]
    [Authorize(Policy = "User")]
    public Task<IActionResult> Create([FromBody] ProjectCreateRequest project) {
        try {
            project.AccountId = TokenHelper.GetAccountIdFromToken(Request);
            var projectCreated = _projectService.Create(project);
            return Task.FromResult<IActionResult>(Ok(projectCreated));
        } catch (Exception ex) {
            _logger.LogError(ex.Message);
            return Task.FromResult<IActionResult>(BadRequest(ex.Message));
        }
    }
    
    
    [HttpGet]
    [Authorize(Policy = "User")]
    public Task<IActionResult> GetAllProjects() {
        var accountId = TokenHelper.GetAccountIdFromToken(Request);
        try {
            var projects = _projectService.GetAllProjects(accountId);
            return Task.FromResult<IActionResult>(Ok(projects));
        } catch (Exception ex) {
            _logger.LogError("Failed to get all projects: {Ex}", ex);
            return Task.FromResult<IActionResult>(BadRequest("Error occured"));
        }
    }
}