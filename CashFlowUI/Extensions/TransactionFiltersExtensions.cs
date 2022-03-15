using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Extensions
{
    public static class TransactionFiltersExtensions
    {
        public static IEnumerable<Transaction> FilterByPaymentTypeEqualTo(this IEnumerable<Transaction> transactions,
            string paymentType)
        {
            if(paymentType is null)
            {
                return transactions;
            }

            var transactionList = transactions.Where(t => t.PaymentType.ToLower() == paymentType.ToLower());
            return transactionList;
        }

        public static IEnumerable<Transaction> FilterByPaymentTypeDifferentThan(this IEnumerable<Transaction> transactions,
            string paymentType)
        {
            return transactions.Where(t => t.PaymentType.ToLower() != paymentType.ToLower());
        }


        public static IEnumerable<Transaction> FilterByDateInInterval(this IEnumerable<Transaction> transactions,
            DateTime start, DateTime end)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime >= start && t.TransactionTime <= end);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateNotInInterval(this IEnumerable<Transaction> transactions,
            DateTime firstDate, DateTime secondDate)
        {
            var validTransactions = transactions?
               .Where(t => t.TransactionTime < firstDate || t.TransactionTime > secondDate);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateEarlierThan(this IEnumerable<Transaction> transactions,
            DateTime dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime < dateToCompare);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateLaterThan(this IEnumerable<Transaction> transactions,
            DateTime dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime > dateToCompare);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateEqualTo(this IEnumerable<Transaction> transactions,
            DateTime dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime.ToYearMonthDayFormat() == dateToCompare);

            return validTransactions;
        }

        public static IEnumerable<Transaction> FilterByDateDifferentThan(this IEnumerable<Transaction> transactions,
            DateTime dateToCompare)
        {
            var validTransactions = transactions?
                .Where(t => t.TransactionTime.ToYearMonthDayFormat() != dateToCompare);

            return validTransactions;
        }

        public static DateTime ToYearMonthDayFormat(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
    }
}
