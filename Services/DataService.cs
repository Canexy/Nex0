using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Nexo.Models;

namespace Nexo.Services
{
    public class DataService
    {
        private const string EmulatorsFile = "emulators.json";

        public List<Emulator> LoadEmulators()
        {
            if (File.Exists(EmulatorsFile))
            {
                try
                {
                    var json = File.ReadAllText(EmulatorsFile);
                    return JsonSerializer.Deserialize<List<Emulator>>(json) ?? new List<Emulator>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading emulators: {ex.Message}");
                    return new List<Emulator>();
                }
            }
            return new List<Emulator>();
        }

        public void SaveEmulators(List<Emulator> emulators)
        {
            try
            {
                var json = JsonSerializer.Serialize(emulators, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(EmulatorsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving emulators: {ex.Message}");
            }
        }
    }
}