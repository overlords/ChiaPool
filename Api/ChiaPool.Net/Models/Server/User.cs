using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class User
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Sha512 hash of the password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The miners of the user.
        /// </summary>
        [JsonIgnore]
        public List<Miner> Miners { get; set; }

        /// <summary>
        /// The plotters of the user.
        /// </summary>
        [JsonIgnore]
        public List<Plotter> Plotters { get; set; }

        public User(string name, string passwordHash)
        {
            Name = name;
            PasswordHash = passwordHash;
            Miners = new List<Miner>();
            Plotters = new List<Plotter>();
        }
    }
}
