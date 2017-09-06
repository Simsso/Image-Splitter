using System;

namespace ImageSplitter
{
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

        public String Show()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = this.DefaultExtension,
                Filter = this.Filter
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }
            return null;
        }
    }
}
