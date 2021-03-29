using System;

namespace ChiaMiningManager.Models
{
    public class Plot
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Name { get; set; }
        public int Minutes { get; set; }

        public string Path { get; set; }
    }
}
