﻿using CodePace.GetWork.API.TechnicalEvaluation.Domain.Model.Aggregates;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Model.Queries;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Repositories;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Services;

namespace CodePace.GetWork.API.TechnicalEvaluation.Application.Internal.QueryServices;

public class TechnicalTestQueryService(ITechnicalTestRepository technicalTestRepository): ITechnicalTestQueryService
{
    public Task<IEnumerable<TechnicalTest>> Handle(GetAllTechnicalTestsQuery query)
    {
        throw new NotImplementedException();
    }
}