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

namespace LearningContest.Application.Features.BasicConfig.Commands.Delete
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, BaseResponse<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCommandHandler> _logger;
        private readonly IUnitOfWorkLearningContest _unitOfWork;
        public DeleteCommandHandler(IMapper mapper, IEmailService emailService, ILogger<DeleteCommandHandler> logger, IUnitOfWorkLearningContest unitOfWork, IHttpCallService httpCallService, IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            await _unitOfWork.BasicConfigRepository.DeleteById(request.Id);
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