using LearningContest.Application.Contracts;
using LearningContest.Application.Contracts.HttpCall;
using LearningContest.Application.Contracts.Persistence;
using LearningContest.Application.Contracts.Persistence.Dapper;
using LearningContest.Application.Extension;
using LearningContest.Application.PublishStrategy;
using LearningContest.Application.Responses;
using LearningContest.Domain.Common;
using LearningContest.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LearningContest.Application.Features.BasicConfig.Queries.List
{
    public class ListQueryHandler : IRequestHandler<ListQuery, BaseResponse<ListResponse>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IHttpCallService _httpCallService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkLearningContest _unitOfWork;

        public ListQueryHandler(IMapper mapper,
            ILogger logger,
            IMediator mediator,
            IConfiguration configuration,
            IHttpCallService httpCallService,
            IUnitOfWorkLearningContest unitOfWork
            )
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
            _httpCallService = httpCallService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<ListResponse>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.BasicConfigRepository.GetSieveList(request.Model);
            var count = await _unitOfWork.BasicConfigRepository.GetSieveCount(request.Model);

            return new BaseResponse<ListResponse> { 
                Result = new ListResponse { Count = count, List = result } };
        }
                
    }
}
