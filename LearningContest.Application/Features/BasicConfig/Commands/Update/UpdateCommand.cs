namespace LearningContest.Application.Features.BasicConfig.Commands.Update
{
    using System;
    using MediatR;
    using LearningContest.Application.Responses;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CommonInfrastructure.General.Enums;

    public class UpdateVm
    {
        [Required(ErrorMessage = "شناسه الزامی است")]
        public Int32? Id
        {
            get;
            set;
        }

        public String ItemValue
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

    }

    public class UpdateCommand : IRequest<BaseResponse<bool>>
    {
        public Int32 Id
        {
            get;
            set;
        }

        

        public String ItemValue
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        
    }
}