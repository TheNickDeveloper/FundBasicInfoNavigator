using FundBasicInfoNavigator.Interfaces;
using FundBasicInfoNavigator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FundBasicInfoNavigator.Services
{
    public class FundInfoSearcher<T>
    {
        private readonly IFundHandler<T> _fundHandler;

        public List<T> ResultList { get; set; }
        public List<string> LogList { get; set; }
        public ShellViewModel ViewModel { get; set; }

        public FundInfoSearcher(IFundHandler<T> fundHandler, ShellViewModel shellViewModel)
        {
            _fundHandler = fundHandler;
            ViewModel = shellViewModel;
        }

        public void SearchButtonClickLogic()
        {
            ViewModel.LogMessage = new List<string>();
            ViewModel.Fund.Clear();

            if (IsPassUiValidation())
            {
                var t = new Thread(() => ExecuteFundInfoSearching());
                t.Start();
            }
        }

        private bool IsPassUiValidation()
        {
            var uiInputValidator = new InputValidator();
            var valideResult = true;

            if (ViewModel.IsDataInputManualSearch)
            {
                valideResult = uiInputValidator.IsEmptyContents(ViewModel.BondListString, "bond code");
            }

            if (ViewModel.IsDataInputImportCsvFile && valideResult)
            {
                valideResult = uiInputValidator.IsValideFilePath(ViewModel.ImportDataPath, "Import file path");
            }

            if (ViewModel.IsExportCsvResult || ViewModel.IsExportExcelResult && valideResult)
            {
                valideResult = uiInputValidator.IsValideFolderPath(ViewModel.ExportDataPath, "Export file path");
            }

            if (!valideResult)
            {
                ViewModel.LogMessage = new List<string> { uiInputValidator.ErrorMessage };
                return false;
            }

            return true;
        }

        private void ExecuteFundInfoSearching()
        {
            ViewModel.ExcuteStatus = "Running";
            var taskList = new List<Task>();
            var listSource = GetBondList();

            foreach (var bondCode in listSource)
            {
                var currTask = Task.Factory.StartNew(() => _fundHandler.StoreCurrentFundInfo(bondCode));
                taskList.Add(currTask);
            }

            Task.WaitAll(taskList.ToArray());
            RefreshDataGrid(_fundHandler.ResultList.Cast<IFundBasicInfo>().ToList());

            var logMessage = _fundHandler.LogList;
            logMessage.Add("Finish searching process.");

            if (ViewModel.IsExportCsvResult || ViewModel.IsExportExcelResult)
            {
                ExportResult(_fundHandler.ResultList);
                logMessage.Add("Finish exporting result.");
            }

            ViewModel.LogMessage = logMessage;
            ViewModel.ExcuteStatus = "Ready";
        }

        private List<string> GetBondList()
        {
            string stringSource = string.Empty;

            if (ViewModel.IsDataInputManualSearch)
            {
                stringSource = ViewModel.BondListString;
            }

            if (ViewModel.IsDataInputImportCsvFile)
            {
                var reader = new System.IO.StreamReader(ViewModel.ImportDataPath);
                stringSource = reader.ReadToEnd();
            }

            return stringSource.Split(',').Select(x => x.Trim()).Distinct().ToList();
        }

        private void RefreshDataGrid(List<IFundBasicInfo> targetList)
        {
            targetList.ForEach(x => ViewModel.Fund.Add(x));
        }

        private void ExportResult(List<T> listResult)
        {
            var exportHandler = new ResultExporter();
            var exportFileName = $"{ViewModel.SelectedFundApiType}_{DateTime.Now.ToString("yyyyMMdd")}";

            if (ViewModel.IsExportCsvResult)
            {
                exportHandler.ExportAsCsvFile(ViewModel.ExportDataPath, exportFileName, listResult);
            }

            if (ViewModel.IsExportExcelResult)
            {
                exportHandler.ExportAsExcel(ViewModel.ExportDataPath, exportFileName, listResult);
            }
        }
    }
}
