﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WpfDataGrid.ViewModels;

namespace FundBasicInfoNavigator.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private string _bondList;
        private string _excuteStatus;
        private string _selectedFundApiType;
        private string _importDataPath;
        private bool _isDataInputManualSearch;
        private bool _isDataInputImportCsvFile;
        private string _exportDataPath;
        private bool _isExportCsvResult;
        private bool _isDisplayOnly;
        private string _buttonContents;
        private List<string> _errorLog;
        private bool _isExportExcelResult;
        readonly List<string> _logTempMsg = new List<string>();

        public Caliburn.Micro.BindableCollection<BasicInfo> Fund { get; set; }

        public List<string> FundApiType
        {
            get
            {
                return new List<string>
                {
                    "EastMoneyFund",
                    "Test1",
                    "Test2"
                };
            }
        }

        public string SelectedFundApiType
        {
            get => _selectedFundApiType;
            set
            {
                _selectedFundApiType = value;
                NotifyOfPropertyChange(() => SelectedFundApiType);
            }
        }

        public bool IsDataInputManualSearch
        {
            get => _isDataInputManualSearch;
            set
            {
                _isDataInputManualSearch = value;
                NotifyOfPropertyChange(() => IsDataInputManualSearch);
            }
        }

        public bool IsDataInputImportCsvFile
        {

            get => _isDataInputImportCsvFile;
            set
            {
                _isDataInputImportCsvFile = value;
                NotifyOfPropertyChange(() => IsDataInputImportCsvFile);
            }
        }

        public bool IsDisplayOnly
        {

            get => _isDisplayOnly;
            set
            {
                _isDisplayOnly = value;
                NotifyOfPropertyChange(() => IsDisplayOnly);
            }
        }

        public bool IsExportCsvResult
        {

            get => _isExportCsvResult;
            set
            {
                _isExportCsvResult = value;
                NotifyOfPropertyChange(() => IsExportCsvResult);
            }
        }

        public bool IsExportExcelResult
        {
            get => _isExportExcelResult;
            set
            {
                _isExportExcelResult = value;
                NotifyOfPropertyChange(() => IsExportExcelResult);
            }
        }

        public string ImportDataPath
        {
            get => _importDataPath;
            set
            {
                _importDataPath = value;
                NotifyOfPropertyChange(() => ImportDataPath);
            }
        }

        public string ExportDataPath
        {
            get => _exportDataPath;
            set
            {
                _exportDataPath = value;
                NotifyOfPropertyChange(() => ExportDataPath);
            }
        }

        public string BondListString
        {
            get => _bondList;
            set
            {
                _bondList = value;
                NotifyOfPropertyChange(() => BondListString);
            }
        }

        public string ExcuteStatus
        {
            get => _excuteStatus;
            set
            {
                _excuteStatus = value;
                NotifyOfPropertyChange(() => ExcuteStatus);
            }
        }

        public string ButtonContents
        {
            get => _buttonContents;
            set
            {
                _buttonContents = IsDisplayOnly ? "Search" : "Search & Export";
                NotifyOfPropertyChange(() => ButtonContents);
            }
        }

        public List<string> LogMessage
        {
            get => _errorLog;
            set
            {
                _errorLog = value;
                NotifyOfPropertyChange(() => LogMessage);
            }
        }

        public ShellViewModel()
        {
            Fund = new Caliburn.Micro.BindableCollection<BasicInfo>();
            SelectedFundApiType = "EastMoneyFund";
            IsDataInputManualSearch = true;
            IsDisplayOnly = true;
        }


        public void BrowseButtonClickImportDataPath(object sender, RoutedEventArgs e)
        {
            var FD = new OpenFileDialog();
            if (FD.ShowDialog() == DialogResult.OK)
            {
                ImportDataPath = FD.FileName;

            }
        }

        public void BrowseButtonClickExportDataPath(object sender, RoutedEventArgs e)
        {
            var FD = new FolderBrowserDialog();
            if (FD.ShowDialog() == DialogResult.OK)
            {
                ExportDataPath = FD.SelectedPath;
            }
        }

        public void SearchButtonClick()
        {
            LogMessage = new List<string>();
            Fund.Clear();

            if (IsPassUiValidation())
            {
                var t = new Thread(() => RunFunction());
                t.Start();
            }

            LogMessage = _logTempMsg;
        }

        private bool IsPassUiValidation()
        {
            _logTempMsg.Clear();
            var uiInputValidator = new UiInputValidator();
            var valideResult = true;

            if (IsDataInputManualSearch)
            {
                valideResult = uiInputValidator.IsEmptyContents(BondListString, "bond code");
            }

            if (IsDataInputImportCsvFile && valideResult)
            {
                valideResult = uiInputValidator.IsValideFilePath(ImportDataPath, "Import file path");
            }

            if (IsExportCsvResult || IsExportExcelResult && valideResult)
            {
                valideResult = uiInputValidator.IsValideFolderPath(ExportDataPath, "Export file path");
            }

            if (!valideResult)
            {
                _logTempMsg.Add(uiInputValidator.ErrorMessage);
                return false;
            }

            return true;
        }

        private void RunFunction()
        {
            ExcuteStatus = "Running";

            var funApiHandler = new FundApiHandler();
            var listResult = new List<BasicInfo>();
            var taskList = new List<Task>();
            List<string> listSource = GetBondList();

            foreach (var bondCode in listSource)
            {
                var currTask = Task.Factory.StartNew(() => funApiHandler.StoreCurrentFundInfo(SelectedFundApiType, bondCode, ref listResult));
                taskList.Add(currTask);
            }

            Task.WaitAll(taskList.ToArray());
            RefreshDataGrid(listResult);

            var _log = new List<string>();
            _log = funApiHandler.LogList;

            if (IsExportCsvResult || IsExportExcelResult)
            {
                ExportResult(listResult);
                _log.Add("Finish export result :)");
            }

            LogMessage = _log;
            ExcuteStatus = "Ready";
        }


        private List<string> GetBondList()
        {
            string stringSource = string.Empty;

            if (IsDataInputManualSearch)
            {
                stringSource = BondListString;
            }

            if (IsDataInputImportCsvFile)
            {
                var reader = new System.IO.StreamReader(ImportDataPath);
                stringSource = reader.ReadToEnd();
            }

            return stringSource.Split(',').Select(x => x.Trim()).Distinct().ToList();
        }

        private void RefreshDataGrid(List<BasicInfo> targetList)
        {
            targetList.ForEach(x => Fund.Add(x));
        }

        private void ExportResult(List<BasicInfo> listResult)
        {
            var exportHandler = new ResultExporter();
            var exportFileName = $"FundResultOutput_{DateTime.Now.ToString("yyyyMMdd")}";

            if (IsExportCsvResult)
            {
                exportHandler.ExportAsCsvFile(ExportDataPath, exportFileName, listResult);
            }

            if (IsExportExcelResult)
            {
                exportHandler.ExportAsExcel(ExportDataPath, exportFileName, listResult);
            }
        }
    }
}
