using LearningContest.Application.Contracts;
using LearningContest.Application.Contracts.Persistence;
using LearningContest.Application.Extension;
using LearningContest.Application.PublishStrategy;
using LearningContest.Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LearningContest.Application.Contracts.HttpCall;
using Microsoft.Extensions.Configuration;
using System.Linq;
using LearningContest.Application.Exceptions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace LearningContest.Application.Features.BasicConfig.Queries.GetById
{
    public class GetByIdQueryHandler : BaseHandler, IRequestHandler<GetByIdQuery, BaseResponse<BasicConfigDto>>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IHttpCallService _httpCallService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWorkLearningContest _unitOfWork;
        public GetByIdQueryHandler(
            ILogger logger,
            IMediator mediator,
            IHttpCallService httpCallService,
            IConfiguration configuration
, IUnitOfWorkLearningContest unitOfWork) : base(configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _httpCallService = httpCallService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<BasicConfigDto>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {

            var result = await _unitOfWork.BasicConfigRepository.GetBasicConfigById(request.Id); 

            return new BaseResponse<BasicConfigDto>() { Result = result };
        }
    }
}
