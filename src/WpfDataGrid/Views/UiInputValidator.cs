using System.IO;

namespace FundBasicInfoNavigator.Views
{
    public class UiInputValidator
    {
        public string ErrorMessage { get; set; }


        public bool IsEmptyContents(string objValueString, string objectName)
        {
            if (string.IsNullOrEmpty(objValueString))
            {
                ErrorMessage = $"{objectName} cannot be blank.";
                return false;
            }
            return true;
        }

        public bool IsValideFilePath(string path, string objectName)
        {
            IsEmptyContents(path, objectName);

            if (File.Exists(path) == false)
            {
                ErrorMessage = $"{objectName} did not exsit.";
                return false;
            }
            return true;
        }

        public bool IsValideFolderPath(string path, string objectName)
        {
            IsEmptyContents(path, objectName);

            if (Directory.Exists(path) == false)
            {
                ErrorMessage = $"{objectName} did not exsit.";
                return false;
            }
            return true;
        }

    }
}
