using System.Windows.Forms;

namespace WpfDataGrid.Services
{
    public class PathBrowseHelper
    {
        public string FilePathBrowser(string originalString)
        {
            var FD = new OpenFileDialog();
            return (FD.ShowDialog() == DialogResult.OK)
                ? FD.FileName
                : originalString;
        }

        public string FolderPathBrowser(string originalString)
        {
            var FD = new FolderBrowserDialog();
            return (FD.ShowDialog() == DialogResult.OK)
                ? FD.SelectedPath
                : originalString;
        }
    }
}
