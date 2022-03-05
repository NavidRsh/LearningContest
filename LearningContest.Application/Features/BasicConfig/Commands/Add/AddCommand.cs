namespace LearningContest.Application.Features.BasicConfig.Commands.Add
{
    using System;
    using MediatR;
    using LearningContest.Application.Responses;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CommonInfrastructure.General.Enums;

    public class AddVm
    {
        public CommonInfrastructure.General.Enums.BasicConfigKeyEnum ItemKey
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

        public ModuleEnum Module
        {
            get;
            set;
        }
    }

    public class AddCommand : IRequest<BaseResponse<int>>
    {
        public CommonInfrastructure.General.Enums.BasicConfigKeyEnum ItemKey
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

        public ModuleEnum Module
        {
            get;
            set;
        }
    }
}