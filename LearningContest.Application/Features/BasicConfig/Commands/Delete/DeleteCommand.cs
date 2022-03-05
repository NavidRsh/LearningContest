using LearningContest.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearningContest.Application.Features.BasicConfig.Commands.Delete
{
    public class DeleteVm
    {
        [Required(ErrorMessage = "شناسه الزامی است")]
        public int? Id { get; set; }

    }
    public class DeleteCommand : IRequest<BaseResponse<bool>>
    {
        public int Id { get; set; }

    }
}
