﻿using CodePace.GetWork.API.CourseContest.Domain.Model.Aggregates;
using CodePace.GetWork.API.CourseContest.Domain.Model.Entities;
using CodePace.GetWork.API.Shared.Domain.Repositories;

namespace CodePace.GetWork.API.CourseContest.Domain.Repositories;

public interface ICourseDetailRepository : IBaseRepository<CourseDetail>
{
}