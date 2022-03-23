using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Extensions
{
    public static class TransactionFiltersExtensions
    {
        public static IEnumerable<Transaction> FilterByDateInInterval(this IEnumerable<Transaction> transactions,
            DateOnly start, DateOnly end)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime.ToDateOnly() >= start && t.TransactionTime.ToDateOnly() <= end);

            return validTransactions;
        }


        public static IEnumerable<Transaction> FilterByDateNotInInterval(this IEnumerable<Transaction> transactions,
            DateOnly firstDate, DateOnly secondDate)
        {
            var validTransactions = transactions?
               .Where(t => t.TransactionTime.ToDateOnly() < firstDate || t.TransactionTime.ToDateOnly() > secondDate);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateEarlierThan(this IEnumerable<Transaction> transactions,
            DateOnly dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime.ToDateOnly() < dateToCompare);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateLaterThan(this IEnumerable<Transaction> transactions,
            DateOnly dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime.ToDateOnly() > dateToCompare);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateEqualTo(this IEnumerable<Transaction> transactions,
            DateOnly dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime.ToDateOnly() == dateToCompare);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateDifferentThan(this IEnumerable<Transaction> transactions,
            DateOnly dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime.ToDateOnly() != dateToCompare);

            return validTransactions;
        }

        public static DateOnly ToDateOnly(this DateTime date)
        {
            return DateOnly.FromDateTime(date);
        }

        public static IEnumerable<Transaction> FilterByPaymentTypeEqualTo(this IEnumerable<Transaction> transactions,
            string paymentType)
        {
            var validTransactions = transactions?
                .Where(t => t.PaymentType.ToLower() == paymentType.ToLower());

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByPaymentTypeDifferentThan(this IEnumerable<Transaction> transactions,
            string paymentType)
        {
            var validTransactions = transactions?
                .Where(t => t.PaymentType.ToLower() != paymentType.ToLower());

            return validTransactions;
        }
    }
}
