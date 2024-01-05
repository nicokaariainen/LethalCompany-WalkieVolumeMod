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
            var value = this.Config.Bind<float>("General", "VolumeMultiplier", 1f, "Volume multiplier. 0.5 for 50%, 1.5 for 150% etc. Max value is 2.").Value;
            VolumeMultiplier.Value = Math.Min(Math.Max(value, 0f), 2f);

            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("WalkieVolume mod loaded.");

            harmony.PatchAll(typeof(WalkieVolumeBase));
            harmony.PatchAll(typeof(WalkieTalkiePatch));
        }


    }
}
