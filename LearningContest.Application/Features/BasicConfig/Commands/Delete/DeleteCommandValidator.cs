using LearningContest.Application.Contracts.Persistence;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LearningContest.Application.Features.BasicConfig.Commands.Delete
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        //private readonly IEventRepository _eventRepository;
        public DeleteCommandValidator(/*IEventRepository eventRepository*/)
        {
            //_eventRepository = eventRepository;

            //RuleFor(p => p.Name)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            //RuleFor(p => p.Date)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .GreaterThan(DateTime.Now);

            ////RuleFor(e => e)
            ////    .MustAsync(EventNameAndDateUnique)
            ////    .WithMessage("An event with the same name and date already exists.");

            //RuleFor(p => p.Price)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .GreaterThan(0);
        }

        //private async Task<bool> EventNameAndDateUnique(CreateEventCommand e, CancellationToken token)
        //{
        //    return !(await _eventRepository.IsEventNameAndDateUnique(e.Name, e.Date));
        //}
    }
}
