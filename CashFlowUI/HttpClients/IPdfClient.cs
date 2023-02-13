using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public interface IPdfClient
    {
        Task<string> GetPdf();
    }
}