﻿using System.Net.Mime;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Model.Commands;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Model.Queries;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Services;
using CodePace.GetWork.API.TechnicalEvaluation.Interfaces.REST.Resources;
using CodePace.GetWork.API.TechnicalEvaluation.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace CodePace.GetWork.API.TechnicalEvaluation.Interfaces.REST;

[ApiController]
[Route("api/v1/technical-tasks")]
[Produces(MediaTypeNames.Application.Json)]
public class TechnicalTaskController(ITechnicalTaskCommandService technicalTaskCommandService, ITechnicalTaskQueryService technicalTaskQueryService): ControllerBase
{
    [HttpPut("{technicalTestId:int}/assign/{userId:int}")]
    public async Task<IActionResult> AssignTechnicalTaskToUser([FromRoute] int technicalTestId, int userId)
    {
        var assignTechnicalTaskToUserCommand = new AssignTechnicalTaskToUserCommand(technicalTestId, userId);
        var technicalTask = await technicalTaskCommandService.Handle(assignTechnicalTaskToUserCommand);
        if (technicalTask is null) return BadRequest();
        return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> CreateTechnicalTask([FromRoute] int technicalTestId, [FromBody] CreateTechnicalTaskResource createTechnicalTaskResource)
    {
        var createTechnicalTaskCommand = CreateTechnicalTaskCommandFromResourceAssembler.ToCommandFromResource(technicalTestId, createTechnicalTaskResource);
        var technicalTask = await technicalTaskCommandService.Handle(createTechnicalTaskCommand);
        var resource = TechnicalTaskResourceFromEntityAssembler.ToResourceFromEntity(technicalTask);
        return CreatedAtAction(nameof(GetTechnicalTaskById), new { technicalTaskId = resource.Id }, resource);
    }
    [HttpPut ("{technicalTaskId:int}/update/{userId:int}")]
    public async Task<IActionResult> UpdateTechnicalTask([FromRoute] int technicalTaskId, int userId, [FromBody] UpdateTaskProgressResource updateTechnicalTaskResource)
    {
        Console.WriteLine("Update Technical Task");
        Console.WriteLine(userId);
        var updateTechnicalTaskCommand = UpdateTaskProgressCommandFromResourceAssembler.ToCommandFromResource(technicalTaskId, userId, updateTechnicalTaskResource);
        var technicalTask = await technicalTaskCommandService.Handle(updateTechnicalTaskCommand);
        return Ok();
    }

    [HttpGet("/{technicalTaskId:int}")]
    public async Task<IActionResult> GetTechnicalTaskById(int technicalTaskId)
    {
        var getTechnicalTaskByIdQuery = new GetTechnicalTaskByIdQuery(technicalTaskId);
        var technicalTask = await technicalTaskQueryService.Handle(getTechnicalTaskByIdQuery);
        var resource = TechnicalTaskResourceFromEntityAssembler.ToResourceFromEntity(technicalTask);
        return Ok(resource);
    }
    
    [HttpGet("details-{technicalTestId:int}")]
    public async Task<IActionResult> GetAllTechnicalTaskByTechnicalTestId([FromRoute] int technicalTestId)
    {
        var getAllTechnicalTaskQuery = new GetAllTechnicalTaskByTechnicalTestIdQuery(technicalTestId);
        var technicalTasks = await technicalTaskQueryService.Handle(getAllTechnicalTaskQuery);
        var resources = technicalTasks.Select(TechnicalTaskResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    [HttpGet("details-{technicalTestId:int}/user/{userId:int}")]
    public async Task<IActionResult> GetTechnicalTaskByUserId([FromRoute] int userId, [FromRoute] int technicalTestId)
    {
        var getTechnicalTaskByUserIdQuery = new GetAllTechnicalTaskByUserIdQuery(userId, technicalTestId);
        var technicalTasks = await technicalTaskQueryService.Handle(getTechnicalTaskByUserIdQuery);
        var resources = technicalTasks.Select(TechnicalTaskResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}