using FinTrack.Domain.Common;

namespace FinTrack.Domain.Expenses;

    public sealed class ExpenseAllocation : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid ProfileId { get; private set; }
        public Guid ExpenseId { get; private set; }
        public Money AssignedAmount { get; private set; } = Money.Zero;
        public DateOnly? PaidAt { get; private set; }

        private ExpenseAllocation() {}

        internal static ExpenseAllocation Create(Guid profileId, Guid expenseId,
            Money assignedAmount, DateOnly? paidAt = null
        )
        {
            var allocation = new ExpenseAllocation
            {
                Id = Guid.CreateVersion7(),
                ProfileId = profileId,
                ExpenseId = expenseId,
                AssignedAmount = assignedAmount,
                PaidAt = paidAt
            };

            return allocation;
        }
}