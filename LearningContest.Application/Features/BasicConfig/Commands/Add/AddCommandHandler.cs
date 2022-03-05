using LearningContest.Application.Contracts.Infrastructure;
using LearningContest.Application.Contracts.Persistence;
using LearningContest.Domain.Entities;
using AutoMapper;
using LearningContest.Application.Models.Mail;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using LearningContest.Application.Contracts;
using LearningContest.Application.Responses;
using LearningContest.Application.Contracts.HttpCall;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using LearningContest.Application.Extension;
using System.Linq;

namespace LearningContest.Application.Features.BasicConfig.Commands.Add
{
    public class AddBasicConfigCommandHandler : IRequestHandler<AddCommand, BaseResponse<int>>
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<AddBasicConfigCommandHandler> _logger;
        private readonly IUnitOfWorkLearningContest _unitOfWork;
        private readonly IHttpCallService _httpCallService;
        private readonly IConfiguration _configuration;
        public AddBasicConfigCommandHandler(IMapper mapper, IEmailService emailService, ILogger<AddBasicConfigCommandHandler> logger, IUnitOfWorkLearningContest unitOfWork, IHttpCallService httpCallService, IConfiguration configuration)
        {
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpCallService = httpCallService;
            _configuration = configuration;
        }

        public async Task<BaseResponse<int>> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddBasicConfigCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                throw new Exceptions.ValidationException(validationResult);
            }

            var entity = new Domain.Entities.General.BasicConfig()
            {
                ItemKey = request.ItemKey,
                ItemValue = request.ItemValue,
                Description = request.Description,
                Module = request.Module
            }; 

            await _unitOfWork.BasicConfigRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return new BaseResponse<int>() { 
                Success = true, 
                Result = entity.Id, 
                Message = ""
            }; 
            
        }
    }
}