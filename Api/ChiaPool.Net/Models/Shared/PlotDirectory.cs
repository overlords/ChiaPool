namespace ChiaPool.Models
{
    public class PlotDirectory
    {
        public string Path { get; init; }
        public int Size { get; init; }
        public int Parallelism { get; init; }

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

            string[] parts = input.Split(':', 3);

            if (parts.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int sizeInGB))
            {
                return false;
            }
            if (!int.TryParse(parts[1], out int parallelism))
            {
                return false;
            }

            plotDirectory = new PlotDirectory()
            {
                Path = parts[2],
                Size = sizeInGB,
                Parallelism = parallelism,
            };
            return true;
        }
    }
}
