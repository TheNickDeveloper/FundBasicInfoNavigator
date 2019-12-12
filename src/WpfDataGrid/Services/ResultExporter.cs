using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace FundBasicInfoNavigator.Services
{
    public class ResultExporter
    {
        public void ExportAsCsvFile<T>(string path, string exportFileName, List<T> objList)
        {
            var sb = new StringBuilder();
            var titleNameString = GetTitleNameString<T>();

            sb.AppendLine(titleNameString);
            objList.ForEach(x => sb.AppendLine(x.ToString()));
            path += @$"\{exportFileName}.csv";
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private string GetTitleNameString<T>()
        {
            var propertiesName = typeof(T).GetProperties();
            var titleNameString = string.Empty;

            foreach (PropertyInfo item in propertiesName)
            {
                titleNameString += item.Name + ", ";
            }

            return titleNameString.Substring(0, titleNameString.Length - 2);
        }

        public void ExportAsExcel<T>(string path, string exportFileName, List<T> objList)
        {
            ExportAsCsvFile<T>(path, "tempFile", objList);
            var csvResultFile = path + @"\tempFile.csv";

            var app = new Excel.Application
            {
                DisplayAlerts = false
            };

            var workbook = app.Workbooks.Open(csvResultFile);
            workbook.Saved = true;

            var workSheet = (Excel.Worksheet)app.ActiveSheet;
            workSheet.Name = exportFileName;
            workbook.SaveAs(path + @$"\{exportFileName}.xlsx", XlFileFormat.xlExcel8);
            workbook.Close();

            File.Delete(csvResultFile);
        }

    }
}
