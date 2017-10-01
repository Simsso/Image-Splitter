namespace ImageSplitter
{
    class SplitConfig
    {
        private string outputPath;
        private string[][] fileNames;

        public string OutputPath { get => outputPath; set => outputPath = value; }
        public string[][] FileNames { get => fileNames; set => fileNames = value; }
    }
}
