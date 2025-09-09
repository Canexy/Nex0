using System.Collections.Generic;

namespace Nexo.Models
{
    public class EmulatorOption
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "comando" o "archivo"
        public string Argument { get; set; } = string.Empty;
        public List<string>? Values { get; set; }
        public string? FilePath { get; set; }
    }
}