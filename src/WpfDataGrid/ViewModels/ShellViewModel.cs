using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Caliburn.PresentationFramework;

namespace WpfDataGrid.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase
    {

        private readonly HttpHandler _httpHandler = new HttpHandler();
        private string _bondList;

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

        public ShellViewModel()
        {
            Fund = new Caliburn.Micro.BindableCollection<BasicInfo>();
        }

        public void SearchButtonClick()
        {
            List<string> listSource = GetBondList(BondListString);
            List<BasicInfo> targetList = _httpHandler.SimulateExampleList(listSource);
            RefreshDataGrid(targetList);
        }

        private void RunFunction()
        {
            List<string> listSource = GetBondList(BondListString);
            List<BasicInfo> targetList = _httpHandler.SimulateExampleList(listSource);
            RefreshDataGrid(targetList);
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
