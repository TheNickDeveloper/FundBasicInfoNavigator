using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FundBasicInfoNavigator.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private string _bondList;
        private string _excuteStatus;
        private string _selectedFundApiType;
        private List<string> _errorLog;

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
            
            var logTempMsg = new List<string>();
            LogMessage = logTempMsg;

            if (string.IsNullOrEmpty(BondListString))
            {
                logTempMsg.Add("The bond code is empty, please enter valid fund code.");
            }
            else
            {
                var funApiHandler = new FundApiHandler();
                var listResult = new List<BasicInfo>();
                var taskList = new List<Task>();
                List<string> listSource = GetBondList(BondListString);

                foreach (var bondCode in listSource)
                {
                    var currTask = Task.Factory.StartNew(()=> funApiHandler.StoreCurrentFundInfo(SelectedFundApiType, bondCode, ref listResult));
                    taskList.Add(currTask);
                }

                Task.WaitAll(taskList.ToArray());
                RefreshDataGrid(listResult);
                logTempMsg = funApiHandler.LogList;
            }
            LogMessage = logTempMsg;
            ExcuteStatus = "Ready";
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
