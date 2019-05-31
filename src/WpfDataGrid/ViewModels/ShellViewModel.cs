using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WpfDataGrid.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {

        private string _bondList;
        private string _excuteStatus;

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
            List<string> listSource = GetBondList(BondListString);
            List<BasicInfo> listTarget = httpHandler.SimulateExampleList(listSource);
            RefreshDataGrid(listTarget);

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
