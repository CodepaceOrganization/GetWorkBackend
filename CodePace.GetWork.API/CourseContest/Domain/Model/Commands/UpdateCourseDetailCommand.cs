﻿using CodePace.GetWork.API.CourseContest.Domain.Model.Entities;

namespace CodePace.GetWork.API.CourseContest.Domain.Model.Commands;

public class UpdateCourseDetailCommand(
    int id,
    int courseId,
    string description,
    string image,
    string image2,
    string image3,
    string introduction,
    string development,
    string test,
    List<string> goals)
{
    public int Id { get; set; } = id;
    public int CourseId { get; set; } = courseId;
    public string Description { get; set; } = description;
    public string Image { get; set; } = image;
    public string Image2 { get; set; } = image2;
    public string Image3 { get; set; } = image3;
    public string Introduction { get; set; } = introduction;
    public string Development { get; set; } = development;
    public string Test { get; set; } = test;
    public List<string> Goals { get; set; } = goals;
}