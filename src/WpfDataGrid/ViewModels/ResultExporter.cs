using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace WpfDataGrid.ViewModels
{
    public class ResultExporter
    {
        public void ExportAsCsvFile<T>(string path, List<T> objList)
        {
            var sb = new StringBuilder();
            var titleNameString = GetTitleNameString<T>();

            sb.AppendLine(titleNameString);
            objList.ForEach(x => sb.AppendLine(x.ToString()));

            path += @$"\FundResultOutput_{DateTime.Now.ToString("yyyyMMdd")}.csv";
            File.WriteAllText(path, sb.ToString());
        }

        private string GetTitleNameString<T>()
        {
            var propertiesName = typeof(T).GetProperties();
            var titleNameString = string.Empty;

            foreach (PropertyInfo item in propertiesName)
            {
                titleNameString += item.Name + ", ";
            }

            return titleNameString.Substring(titleNameString.Length - 2);
        }

    }
}
