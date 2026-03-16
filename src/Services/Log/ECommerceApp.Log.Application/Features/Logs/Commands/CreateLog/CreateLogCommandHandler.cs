using ECommerceApp.Log.Application.DTOs;
using ECommerceApp.Log.Application.Interfaces;
using ECommerceApp.Shared.Models;
using MediatR;

namespace ECommerceApp.Log.Application.Features.Logs.Commands.CreateLog
{
    public sealed class CreateLogCommandHandler : IRequestHandler<CreateLogCommand, Result>
    {
        private readonly ILogRepository _logRepository;

        public CreateLogCommandHandler(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<Result> Handle(CreateLogCommand request, CancellationToken cancellationToken)
        {
            await _logRepository.AddAsync(new CreateLogDto
            {
                Level = request.Level,
                Message = request.Message,
                Source = request.Source,
                Exception = request.Exception
            }, cancellationToken);

            return Result.Success();
        }
    }
}
