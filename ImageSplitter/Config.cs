using System;

namespace ImageSplitter
{
    /// <summary>
    /// Represents a user configuration for export file names.
    /// </summary>
    class Config
    {
        private string[] columnNames, rowNames;

        public Config LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }

        public String GetName(int x, int y)
        {
            return columnNames[x] + "_" + columnNames[y];
        }
    }
}
