using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using WalkieVolume.Patches;
using System;

namespace WalkieVolume
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class WalkieVolumeBase : BaseUnityPlugin
    {
        private const string modGUID = "unity.WalkieVolumeMod";
        private const string modName = "WalkieVolume";
        private const string modVersion = "1.0.0.0";

        public static ConfigEntry<float> VolumeMultiplier;

        private readonly Harmony harmony = new Harmony(modGUID);

        private static WalkieVolumeBase Instance;

        public static ManualLogSource mls;

        private void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            VolumeMultiplier = base.Config.Bind<float>("General", "VolumeMultiplier", 1f, "Volume multiplier. 0.5 for 50%, 1.5 for 150% etc. Max value is 2.");
            VolumeMultiplier.Value = Math.Min(Math.Max(VolumeMultiplier.Value, 0f), 2f);

            
            if (Instance == null)
            {
                Instance = this;
            }

            harmony.PatchAll(typeof(WalkieVolumeBase));
            harmony.PatchAll(typeof(WalkieTalkiePatch));

            mls.LogInfo($"WalkieVolume mod loaded. Walkie volume set to: {VolumeMultiplier.Value * 100}%");
        }
    }
}
