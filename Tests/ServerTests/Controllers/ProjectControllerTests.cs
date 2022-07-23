using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Api.Controllers;
using Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Core.Interfaces.Service;
using Server.Core.Models.Entities.Entity;
using ServerTests.Helpers;
using Xunit;

namespace ServerTests.Controllers;

public class ProjectControllerTests {
    private readonly Mock<IProjectService> _mockProjectService = new();
    private readonly Mock<ILogger<ProjectController>> _logger = new();
    private ProjectController _sut;

    private MockHelper _mockHelper = MockHelper.Instance;

    private void ResetMocks() {
        _mockProjectService.Reset();
        _logger.Reset();
    }

    [Fact(DisplayName = "Create() - Success - Returns Ok() with list of projects"), Trait("ProjectController", "Create")]
    public async void Create_Success_ReturnsOk() {
        ResetMocks();
        MockHelper.ProjectService.Create.ReturnsProject(_mockProjectService);

        var httpContext = new DefaultHttpContext();

        MockHelper.HttpContext.SetAuthHeader(httpContext);
        _sut = new ProjectController(_mockProjectService.Object, _logger.Object) {
            ControllerContext = new ControllerContext() {
                HttpContext = httpContext
            }
        };

        var result = await _sut.Create(TestModelHelper.ProjectCreateRequest);

        var response = result.Should().BeOfType<OkObjectResult>().Subject;
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var projects = response.Value.Should().BeAssignableTo<Project>().Subject;
        projects.Should().BeEquivalentTo(TestModelHelper.CreatedProject);
    }
    
    [Fact(DisplayName = "Create() - Failure - Returns BadRequest() with error message"), Trait("ProjectController", "Create")]
    public async void Create_Failure_ReturnsBadRequest() {
        ResetMocks();
        MockHelper.ProjectService.Create.ThrowsException(_mockProjectService);

        var httpContext = new DefaultHttpContext();

        MockHelper.HttpContext.SetAuthHeader(httpContext);
        _sut = new ProjectController(_mockProjectService.Object, _logger.Object) {
            ControllerContext = new ControllerContext() {
                HttpContext = httpContext
            }
        };

        var result = await _sut.Create(TestModelHelper.ProjectCreateRequest);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact(DisplayName = "GetAll() - Success - Returns Ok() with list of projects"), Trait("ProjectController", "GetAll")]
    public async void GetAll_Success_ReturnsOk() {
        ResetMocks();
        MockHelper.ProjectService.GetAll.ReturnsProjects(_mockProjectService);

        var httpContext = new DefaultHttpContext();

        MockHelper.HttpContext.SetAuthHeader(httpContext);
        _sut = new ProjectController(_mockProjectService.Object, _logger.Object) {
            ControllerContext = new ControllerContext() {
                HttpContext = httpContext
            }
        };

        var result = await _sut.GetAllProjects();

        var response = result.Should().BeOfType<OkObjectResult>().Subject;
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var projects = response.Value.Should().BeAssignableTo<IQueryable<Project>>().Subject;
        projects.Should().BeEquivalentTo(TestModelHelper.Projects);
        projects.Select(project => project).ToList().Should().BeAssignableTo<List<Project>>();
    }
}
