using System.Globalization;
using System.IO;
using CsvHelper;

namespace Netytar.Settings
{
    public class SavingSystem
    {
        public const string DEFAULTFILENAME = "Settings";

        public SavingSystem(string settingsFilename = DEFAULTFILENAME)
        {
            SettingsFilename = settingsFilename;
        }

        private string SettingsFilename { get; set; }

        public int SaveSettings(NetytarSettings settings)
        {
            try
            {
                // Settings

                List<NetytarSettings> records = new List<NetytarSettings> { settings };

                using (var writer = new StreamWriter(SettingsFilename + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(records);
                }

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public NetytarSettings LoadSettings()
        {
            try
            {
                List<NetytarSettings> settings = new List<NetytarSettings>();
                using (var reader = new StreamReader(SettingsFilename + ".csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<NetytarSettings>();
                    settings = records.ToList();
                }
                return settings[0];
            }
            catch
            {
                return new DefaultSettings();
            }
        }

        
    }
}