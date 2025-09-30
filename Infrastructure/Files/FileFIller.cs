
namespace DocumentHelper.Infrastructure.Files
{
    public abstract class FileFiller<InfoForFill>
    {
        protected string _TemplateName = string.Empty;
        protected string _NewFileName = string.Empty;
        protected InfoForFill _Info;

        public FileFiller(string TemplateName, string NewFileName, InfoForFill info)
        {
            _TemplateName = TemplateName;
            _NewFileName = NewFileName;
            _Info = info;
        }

        protected abstract void OpenFile();
        protected abstract void SaveFile();
        protected abstract void FillFile();

        public void ProcessFile()
        {
            OpenFile();
            FillFile();
            SaveFile();
        }

    }
}
