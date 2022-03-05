using LearningContest.Application.Responses;
using MediatR;
using Sieve.Models;
using System;
using System.Collections.Generic;

namespace LearningContest.Application.Features.BasicConfig.Queries.List
{
    public class ListVm
    {
        public SieveModel Model { get; set; }
    }

    public class ListQuery : IRequest<BaseResponse<ListResponse>>
    {
        public SieveModel Model { get; set; }
    }

    public class ListResponse
    {
        public int Count { get; set; }
        public List<Queries.GetById.BasicConfigDto> List { get; set; }
    }
    



}
