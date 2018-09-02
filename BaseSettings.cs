using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace hhax
{
    public static class BaseSettings
    {
        public const string DefaultSettingsName = "settings.bin";
        private static Settings _settings;

        public static Settings GetSettings => _settings ?? (_settings = FetchSettings());

        private static Settings FetchSettings()
        {
            Debug.Log("Trying to read settings..");
            try
            {
                using (var mem = new MemoryStream(File.ReadAllBytes(DefaultSettingsName)))
                {
                    var binary = new BinaryFormatter();
                    return binary.Deserialize(mem) as Settings;
                }
            }
            catch (Exception)
            {
                return GetDefault();
            }
        }

        private static Settings GetDefault()
        {
            return new Settings {ShowEspMenu = false, EspSettings = new EspSettings {StructManLodDist = 0f, DrawPlayers = false, IsEnabled = true, DrawWrecks = false}, AimBotSettings = new AimBotSettings { IsEnabled = true, AimAtPlayers = false}};
        }

        public static void SaveSettings()
        {
            using (var mem = new MemoryStream())
            {
                var binary = new BinaryFormatter();
                binary.Serialize(mem, _settings);
                File.WriteAllBytes(DefaultSettingsName, mem.ToArray());
            }
        }
    }


    [Serializable]
    public class Settings
    {
        public bool ShowEspMenu { get; set; }
        public EspSettings EspSettings { get; set; } = new EspSettings();
        public AimBotSettings AimBotSettings { get; set; } = new AimBotSettings();
        public KeyCode EspMenuKeyCode { get; set; } = KeyCode.F5;
    }

    [Serializable]
    public class EspSettings
    {
        public bool DrawOwnershipStakes { get; set; }
        public bool StorageLocker { get; set; }
        public bool FirePit { get; set; }
        public bool LootCache { get; set; }
        public bool LogResourceNode { get; set; }
        public bool FlintRock { get; set; }
        public bool Metal2Resource { get; set; }
        public bool Metal3Resource { get; set; }
        public bool Metal4Resource { get; set; }
        public bool IronRockResource { get; set; }
        public bool CoalRockResource { get; set; }
        public bool SandstoneResource { get; set; }
        public bool AIShigiForest { get; set; }
        public bool AIBorProxy { get; set; }
        public bool AIShigiProxy { get; set; }
        public bool AIYetiForest { get; set; }
        public bool AITokarProxy { get; set; }
        public bool WorkbenchDynamic { get; set; }
        public bool c4Dynamic { get; set; }
        public bool SleeperLootCrate { get; set; }
        public bool DrawWrecks { get; set; }
        public bool DrawPlayers { get; set; }
        public bool IsEnabled { get; set; }
        public float StructManLodDist { get; set; } = 0f;
    }

    [Serializable]
    public class AimBotSettings
    {
        public bool AimAtPlayers { get; set; }
        public bool IsEnabled { get; set; }
    }
}