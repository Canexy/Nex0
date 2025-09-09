using System.Collections.Generic;
using System.Linq;

namespace Nexo.Models
{
    public class Emulator
    {
        public string Name { get; set; } = "";
        public string ExecutablePath { get; set; } = "";
        public string Arguments { get; set; } = "-fullscreen -rom";
        public List<string> AssociatedExtensions { get; set; } = new List<string>();
        public string Configuration { get; set; } = "Default";
        public string IconPath { get; set; } = "";
        public List<EmulatorOption> Options { get; set; } = new List<EmulatorOption>();
        
        // Propiedad de solo lectura para mostrar las extensiones
        public string ExtensionsDisplay => string.Join(", ", AssociatedExtensions);
    }
}