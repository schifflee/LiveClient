using Microsoft.Win32;

namespace PowerCreator.LiveClient.Infrastructure
{
    public static class OpenFileDialogHelper
    {
        public static string OpenFileDialogWindow(string filter)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = filter
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }

    }
}
