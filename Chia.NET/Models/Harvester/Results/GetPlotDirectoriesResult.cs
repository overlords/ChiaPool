﻿using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    internal sealed class GetPlotDirectoriesResult : ChiaResult
    {
        [JsonPropertyName("directories")]
        public string[] Directories { get; }

        private GetPlotDirectoriesResult()
        {
        }
    }
}
