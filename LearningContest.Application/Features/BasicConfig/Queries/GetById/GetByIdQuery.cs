namespace LearningContest.Application.Features.BasicConfig.Queries.GetById
{
    using System;
    using MediatR;
    using LearningContest.Application.Responses;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sieve.Attributes;
    using CommonInfrastructure.General.Enums;

    public class GetByIdQuery : IRequest<BaseResponse<BasicConfigDto>>
    {
        public Int32 Id
        {
            get;
            set;
        }
    }

    public class BasicConfigDto
    {
        [Sieve(CanFilter = true, CanSort = true)]
        public Int32 Id
        {
            get;
            set;
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public CommonInfrastructure.General.Enums.BasicConfigKeyEnum ItemKey
        {
            get;
            set;
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public String ItemValue
        {
            get;
            set;
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public String Description
        {
            get;
            set;
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public ModuleEnum Module
        {
            get;
            set;
        }
    }
}