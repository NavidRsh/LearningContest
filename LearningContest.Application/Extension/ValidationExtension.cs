using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using LearningContest.Application.Responses;
using MediatR;
using FluentValidation;
using System.Linq;

namespace LearningContest.Application.Extension
{
    public static class ValidationExtensions
    {
        public static async Task<BaseResponse<TResponse>> Validate<TRequest, TValidator, TResponse>(this TRequest request, bool generateException = true)
            where TRequest : IRequest<BaseResponse<TResponse>>
            where TValidator : AbstractValidator<TRequest>, new()
       
        {
            var createResponse = new BaseResponse<TResponse>();
            var validationResult = await (new TValidator()).ValidateAsync(request);
            if (validationResult.Errors.Count > 0)
            {
                createResponse.Success = false;
                createResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createResponse.ValidationErrors.Add(error.ErrorMessage);
                }
                if (generateException)
                {
                    throw new LearningContest.Application.Exceptions.ValidationException(validationResult);
                }
            }
            return createResponse;
        }
    }
}
