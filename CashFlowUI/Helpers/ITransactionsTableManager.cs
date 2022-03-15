using Microsoft.Extensions.Primitives;
using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Helpers
{
    public interface ITransactionsTableManager
    {
        List<Transaction> FilterTransactions(List<Transaction> allTransactions,
            IEnumerable<KeyValuePair<string, StringValues>> searchBuilderFilters);
        List<Transaction> SearchForText(List<Transaction> filteredTransactionList, StringValues searchValue);
        List<Transaction> MakePagination(List<Transaction> filteredTransactionList, int start, int length);
        List<Transaction> SortData(List<Transaction> filteredTransactionList, string columnName, StringValues sortDirection);
        dynamic CreateUpdatedTableConfiguration(List<Transaction> transactions,
            List<Transaction> filteredTransactionList, StringValues formDraw, int totalRecords);
    }
}