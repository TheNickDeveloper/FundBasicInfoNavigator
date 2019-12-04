using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FundBasicInfoNavigator.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private string _bondList;
        private string _excuteStatus;
        private string _selectedFundApiType;
        private string _importDataPath;
        private List<string> _errorLog;
        private bool _isDataInputManualSearch;
        private bool _isDataInputImportCsvFile;
        private string _exportDataPath;
        private bool _isExportResult;
        private bool _isDisplayOnly;
        private string _buttonContents;

        List<string> _logTempMsg = new List<string>();

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

        public bool IsExportResult
        {

            get => _isExportResult;
            set
            {
                _isExportResult = value;
                NotifyOfPropertyChange(() => IsExportResult);
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

        // todo, Read file as string
        //var reader = new System.IO.StreamReader(fileToOpen);
        //reader.ReadToEnd();
        public void BrowseButtonClickImportDataPath(object sender, RoutedEventArgs e)
        {
            var FD = new System.Windows.Forms.OpenFileDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImportDataPath = FD.FileName;

            }
        }

        public void BrowseButtonClickExportDataPath(object sender, RoutedEventArgs e)
        {
            var FD = new System.Windows.Forms.OpenFileDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExportDataPath = FD.FileName;
            }
        }

        public void SearchButtonClick()
        {
            var t = new Thread(() => RunFunction());
            t.Start();
        }

        private void RunFunction()
        {
            ExcuteStatus = "Running";
            Fund.Clear();

            LogMessage = _logTempMsg;

            if (string.IsNullOrEmpty(BondListString))
            {
                _logTempMsg.Add("The bond code is empty, please enter valid fund code.");
            }
            else
            {
                var funApiHandler = new FundApiHandler();
                var listResult = new List<BasicInfo>();
                var taskList = new List<Task>();
                List<string> listSource = GetBondList(BondListString);

                foreach (var bondCode in listSource)
                {
                    var currTask = Task.Factory.StartNew(() => funApiHandler.StoreCurrentFundInfo(SelectedFundApiType, bondCode, ref listResult));
                    taskList.Add(currTask);
                }

                Task.WaitAll(taskList.ToArray());
                RefreshDataGrid(listResult);
                _logTempMsg = funApiHandler.LogList;
            }


            if (IsExportResult)
            {
                //export Report
                _logTempMsg.Add("Finish export result :)");
            }

            LogMessage = _logTempMsg;
            ExcuteStatus = "Ready";
        }

        //todo, could be a seperated class
        private bool IsPassUiValidation()
        {
            if (IsDataInputManualSearch)
            {
                if (string.IsNullOrEmpty(BondListString))
                {
                    _logTempMsg.Add("The bond code is empty, please enter valid fund code.");
                    return false;
                }
            }

            // todo
            if (IsDataInputImportCsvFile)
            {
                if (string.IsNullOrEmpty(ImportDataPath))
                {
                    _logTempMsg.Add("Import file path is empty, please enter valid file path.");
                    return false;
                }

                // todo, file no found
                if (true)
                {
                    _logTempMsg.Add("Import file path cannot be found, please enter valid file path.");
                    return false;
                }
            }

            // todo
            if (IsExportResult)
            {
                if (string.IsNullOrEmpty(ImportDataPath))
                {
                    _logTempMsg.Add("Export file path is empty, please enter valid file path.");
                    return false;
                }

                if (true)
                {
                    _logTempMsg.Add("Export folder path cannot be found, please enter valid file path.");
                    return false;
                }
            }

            return true;
        }

        private List<string> GetBondList(string stringSource)
        {
            return stringSource.Split(',').Select(x => x.Trim()).Distinct().ToList();
        }

        private void RefreshDataGrid(List<BasicInfo> targetList)
        {
            targetList.ForEach(x => Fund.Add(x));
        }
    }
}
