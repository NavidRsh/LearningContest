using CommonInfrastructure.Access.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LearningContest.Application.Features.BasicConfig.Commands.Add;
using LearningContest.Application.Features.BasicConfig.Commands.Delete;
using LearningContest.Application.Features.BasicConfig.Commands.Update;
using LearningContest.Application.Features.BasicConfig.Queries.GetById;
using LearningContest.Application.Features.BasicConfig.Queries.List;
using LearningContest.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningContest.Api.Controllers
{
    [Route("api/LearningContest/[controller]")]
    [ApiController]
    public class BasicConfigController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BasicConfigController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        [LearningContestAuthorize]
        [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(AddVm value)
        {
            return Ok(await _mediator.Send(new AddCommand()
            {
                ItemKey = value.ItemKey,
                ItemValue  = value.ItemValue,
                Module = value.Module,
                Description = value.Description
            }));
        }

        [HttpPut("update/{id}")]
        [LearningContestAuthorize]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, UpdateVm value)
        {
            return Ok(await _mediator.Send(new UpdateCommand()
            {
                Id = value.Id.Value,
                ItemValue = value.ItemValue,                
                Description = value.Description
            }));
        }

        [HttpDelete("delete/{id}")]
        [LearningContestAuthorize]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteCommand()
            {
                Id = id
            }));
        }

        [HttpGet("get/{id}")]
        [LearningContestAuthorize]
        [ProducesResponseType(typeof(BaseResponse<BasicConfigDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            var dtos = await _mediator.Send(new GetByIdQuery()
            {
                Id = id
            });
            return Ok(dtos);
        }


        [HttpPost("list")]
        [LearningContestAuthorize]
        [ProducesResponseType(typeof(BaseResponse<ListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(ListVm value)
        {
            var dtos = await _mediator.Send(new ListQuery()
            {
                Model = value.Model
            });
            return Ok(dtos);
        }
    }
}
