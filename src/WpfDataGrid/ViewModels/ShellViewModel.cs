using FundBasicInfoNavigator.Services;
using FundBasicInfoNavigator.Services.FundHandlers;
using System.Collections.Generic;
using FundBasicInfoNavigator.Data;
using FundBasicInfoNavigator.Models;
using WpfDataGrid.Services;
using FundBasicInfoNavigator.Interfaces;

namespace FundBasicInfoNavigator.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private readonly FundApiSource _fundApiSource;
        private readonly PathBrowseHelper _pathBrowseHelper;

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

        public Caliburn.Micro.BindableCollection<IFundBasicInfo> Fund { get; set; }

        public List<string> FundApiType
        {
            get
            {
                return _fundApiSource.FundApiType;
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
            _fundApiSource = new FundApiSource();
            _pathBrowseHelper = new PathBrowseHelper();

            Fund = new Caliburn.Micro.BindableCollection<IFundBasicInfo>();
            SelectedFundApiType = "EastMoneyFund";
            IsDataInputManualSearch = true;
            IsDisplayOnly = true;
        }

        public void BrowseButtonClickImportDataPath()
        {
            ImportDataPath = _pathBrowseHelper.FilePathBrowser(ImportDataPath);
        }

        public void BrowseButtonClickExportDataPath()
        {
            ExportDataPath = _pathBrowseHelper.FolderPathBrowser(ExportDataPath);
        }

        public void SearchButtonClick()
        {
            var apiDataExtractor = new ApiDataExtractor();

            switch (SelectedFundApiType)
            {
                case "EastMoneyFund":
                    var fundHandler = new EastMoneyFundHandler(apiDataExtractor);
                    var EastMoneyFundSearcher = new FundInfoSearcher<EastMoneyFundBasicInfo>(fundHandler, this);
                    EastMoneyFundSearcher.SearchButtonClickLogic();
                    break;

                default:
                    var funHandlerTest = new EastMoneyFundHandler(apiDataExtractor);
                    var TestFundSearcher = new FundInfoSearcher<EastMoneyFundBasicInfo>(funHandlerTest, this);
                    TestFundSearcher.SearchButtonClickLogic();
                    break;
            }
        }

    }
}
