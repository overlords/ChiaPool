namespace ChiaMiningManager.Models
{
    public class PlotInfo
    {
        public string PublicKey { get; set; }
        public string FileName { get; set; }
        public int Minutes { get; set; }

        public PlotInfo(string publicKey, string fileName)
        {
            PublicKey = publicKey;
            FileName = fileName;
        }
        public PlotInfo()
        {
        }
    }
}
