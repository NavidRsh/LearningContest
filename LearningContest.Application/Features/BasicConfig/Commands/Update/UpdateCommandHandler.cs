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

namespace LearningContest.Application.Features.BasicConfig.Commands.Update
{
    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, BaseResponse<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler> _logger;
        private readonly IUnitOfWorkLearningContest _unitOfWork;
        private readonly IHttpCallService _httpCallService;
        public UpdateCommandHandler(IMapper mapper, IEmailService emailService, ILogger<UpdateCommandHandler> logger, IUnitOfWorkLearningContest unitOfWork, IHttpCallService httpCallService, IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpCallService = httpCallService;
        }

        public async Task<BaseResponse<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            var basket = await _unitOfWork.BasicConfigRepository.GetAsyncWithNoTrack(request.Id);
            if (basket == null)
            {
                throw new Exception("شناسه یافت نشد"); 
            }

            basket.ItemValue = request.ItemValue;
            basket.Description = request.Description; 

            _unitOfWork.BasicConfigRepository.Update(basket);
            await _unitOfWork.SaveAsync();

            return new BaseResponse<bool>()
            {
                Success = true,
                Result = true,
                Message = ""
            };

        }
    }
}