using System;

namespace ImageSplitter
{
    /// <summary>
    /// Helper class that wraps the Microsoft.Win32.OpenFileDialog
    /// </summary>
    class OpenFileDialog
    {
        public String Filter = "";
        public String DefaultExtension = "";

        public OpenFileDialog() : this("", "") { }

        public OpenFileDialog(String DefaultExtension) : this(DefaultExtension, "") { }

        public OpenFileDialog(String DefaultExtension, String Filter)
        {
            this.Filter = Filter;
            this.DefaultExtension = DefaultExtension;
        }

        /// <summary>
        /// Shows the open file dialog.
        /// </summary>
        /// <returns>The path of the selected file or null if aborted.</returns>
        public String Show()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = this.DefaultExtension,
                Filter = this.Filter
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FileName;
            }
            return null;
        }
    }
}
