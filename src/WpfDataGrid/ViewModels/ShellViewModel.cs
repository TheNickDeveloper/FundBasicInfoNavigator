using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WpfDataGrid.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {

        private string _bondList;
        private string _excuteStatus;
        private List<string> _errorLog;

        public Caliburn.Micro.BindableCollection<BasicInfo> Fund { get; set; }

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
        }

        public void SearchButtonClick()
        {
            var t = new Thread(() => RunFunction());
            t.Start();
        }

        private void RunFunction()
        {
            ExcuteStatus = "Running";

            var httpHandler = new HttpHandler();
            var listResult = new List<BasicInfo>();

            List<string> listSource = GetBondList(BondListString);
            var taskList = new List<Task>();

            foreach (var bondCode in listSource)
            {
                var currTask = Task.Factory.StartNew(
                    ()=> httpHandler.StoreCurrentFundInfo(bondCode, ref listResult));

                taskList.Add(currTask);
            }

            Task.WaitAll(taskList.ToArray());

            RefreshDataGrid(listResult);

            LogMessage = httpHandler.LogList;

            ExcuteStatus = "Ready";
        }

        private List<string> GetBondList(string stringSource)
        {
            return stringSource.Split(',').Select(x => x.Trim()).ToList();
        }

        private void RefreshDataGrid(List<BasicInfo> targetList)
        {
            Fund.Clear();
            foreach (var item in targetList)
                Fund.Add(item);
        }

    }
}
