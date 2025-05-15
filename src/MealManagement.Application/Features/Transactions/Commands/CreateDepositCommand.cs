using MealManagement.Application.Common.Interfaces;
using MealManagement.Domain.Entities;
using MealManagement.Domain.Enums;
using MealManagement.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MealManagement.Application.Features.Transactions.Commands
{
    public class CreateDepositCommand : ICommand<Guid>
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReference { get; set; }
        public string Notes { get; set; }
    }

    public class CreateDepositCommandHandler : ICommandHandler<CreateDepositCommand, Guid>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public CreateDepositCommandHandler(
            ITransactionRepository transactionRepository,
            IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
        {
            // Validate amount
            if (request.Amount <= 0)
                throw new Exception("Deposit amount must be greater than zero");

            // Get user
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found");

            // Create transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Amount = request.Amount,
                Type = TransactionType.Deposit,
                Date = DateTime.Now,
                PaymentMethod = request.PaymentMethod,
                PaymentReference = request.PaymentReference,
                Notes = request.Notes
            };

            // Update user balance
            user.Balance += request.Amount;

            await _transactionRepository.AddAsync(transaction);
            await _userRepository.UpdateAsync(user);

            return transaction.Id;
        }
    }
}