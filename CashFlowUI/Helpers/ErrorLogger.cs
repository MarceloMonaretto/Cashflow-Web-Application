namespace CashFlowUI.Helpers
{
    public class ErrorLogger : IErrorLogger
    {
        public bool LogErrorMessage(string errorMessage)
        {
            var tempFolderPath = Path.GetTempPath();
            var cashFlowUiTempFolder = Path.Join(tempFolderPath, "CashflowUI");
            if (!Directory.Exists(cashFlowUiTempFolder))
            {
                Directory.CreateDirectory(cashFlowUiTempFolder);
            }

            var cashFlowUiErrorLogFile = Path.Combine(cashFlowUiTempFolder, "ErrorLogs.txt");

            var now = DateTime.Now;
            var logMessage = $"{now}  -->  {errorMessage}";

            using (StreamWriter sw = File.AppendText(cashFlowUiErrorLogFile))
            {
                sw.WriteLine(logMessage);
            }

            using (var sr = new StreamReader(cashFlowUiErrorLogFile))
            {
                var text = sr.ReadToEnd();
                return text.Contains(logMessage);
            }
        }
    }
}
