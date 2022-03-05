using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using LearningContest.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LearningContest.Application.Notifications.Order
{
    public class AddOrderEventHandler : INotificationHandler<AddOrderEventInput>
    {
        readonly ILogger<AddOrderEventHandler> _logger;
        private readonly IUnitOfWorkLearningContest _unitofwork;
        private readonly IMapper _mapper;

        public AddOrderEventHandler(ILogger<AddOrderEventHandler> logger, IUnitOfWorkLearningContest unitofwork, IMapper mapper)
        {
            _logger = logger;
            _unitofwork = unitofwork;
            _mapper = mapper;
        }

        public async Task Handle(AddOrderEventInput notification, CancellationToken cancellationToken)
        {
            //var domainItems = _mapper.Map<List<OrdersDto>, List<Domain.Entities.Order>>(notification.Result);
            //domainItems.ForEach(x => x.OrderDate = DateTime.Now);
            //await _unitofwork.OrderRepository.AddRange(domainItems);
            //await _unitofwork.SaveAsync();
        }
    }

}
