namespace ChiaPool.Models
{
    public class PlotDirectory
    {
        public string Path { get; init; }
        public int PlotCount { get; init; }

        public PlotDirectory()
        {
        }

        public static bool TryParse(string input, out PlotDirectory plotDirectory)
        {
            plotDirectory = null;

            if (!input.Contains(':'))
            {
                return false;
            }

            string[] parts = input.Split(':', 2);

            if (parts.Length == 0)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int plotCount))
            {
                return false;
            }

            plotDirectory = new PlotDirectory()
            {
                Path = parts[1],
                PlotCount = plotCount
            };
            return true;
        }
    }
}
