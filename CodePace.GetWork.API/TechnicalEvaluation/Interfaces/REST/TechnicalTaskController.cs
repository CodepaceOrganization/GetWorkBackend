﻿using System.Net.Mime;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Model.Commands;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Model.Queries;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Services;
using CodePace.GetWork.API.TechnicalEvaluation.Interfaces.REST.Resources;
using CodePace.GetWork.API.TechnicalEvaluation.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace CodePace.GetWork.API.TechnicalEvaluation.Interfaces.REST;

[ApiController]
[Route("api/v1/task-progress")]
[Produces(MediaTypeNames.Application.Json)]

public class TechnicalTaskController(
    ITechnicalTaskCommandService technicalTaskCommandService, 
    ITechnicalTaskQueryService technicalTaskQueryService): ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> AssignTechnicalTaskToUser([FromRoute] int technicalTestId, int userId)
    {
        var assignTechnicalTaskToUserCommand = new AssignTechnicalTaskToUserCommand(technicalTestId, userId);
        var technicalTask = await technicalTaskCommandService.Handle(assignTechnicalTaskToUserCommand);
        if (technicalTask is null) return BadRequest();
        return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> CreateTechnicalTask([FromBody] CreateTechnicalTaskResource createTechnicalTaskResource)
    {
        var createTechnicalTaskCommand = CreateTechnicalTaskCommandFromResourceAssembler.ToCommandFromResource(createTechnicalTaskResource);
        var technicalTask = await technicalTaskCommandService.Handle(createTechnicalTaskCommand);
        if (technicalTask is null) return BadRequest();
        return CreatedAtAction(nameof(GetTechnicalTaskById), new { technicalTaskId = technicalTask.Id }, technicalTask);
    }

    [HttpGet("{technicalTaskId:int}")]
    public async Task<IActionResult> GetTechnicalTaskById(int technicalTaskId)
    {
        var getTechnicalTaskByIdQuery = new GetTechnicalTaskByIdQuery(technicalTaskId);
        var technicalTask = await technicalTaskQueryService.Handle(getTechnicalTaskByIdQuery);
        var resource = TechnicalTaskResourceFromEntityAssembler.ToResourceFromEntity(technicalTask);
        return Ok(resource);
    }
}