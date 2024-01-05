﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using LethalConfig;
using LethalConfig.ConfigItems;
using LethalConfig.ConfigItems.Options;
using HarmonyLib;
using WalkieVolume.Patches;

namespace WalkieVolume
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("ainavt.lc.lethalconfig")]
    public class WalkieVolumeBase : BaseUnityPlugin
    {
        private const string modGUID = "unity.WalkieVolumeMod";
        private const string modName = "WalkieVolume";
        private const string modVersion = "1.0.0.0";

        public static ConfigEntry<float> WalkieVolume;

        private readonly Harmony harmony = new Harmony(modGUID);

        private static WalkieVolumeBase Instance;

        public static ManualLogSource mls;

        private void Awake()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            WalkieVolume = base.Config.Bind<float>("General", "WalkieVolume", 1f, "Volume multiplier. 0.5 for 50%, 1.5 for 150% etc. Default is 1. Max value is 2.");

            // Volume slider configuration.
            var volumeSlider = new FloatSliderConfigItem(WalkieVolume, new FloatSliderOptions
            {
                RequiresRestart = false,
                Min = 0f,
                Max = 2f,
            });
            LethalConfigManager.AddConfigItem(volumeSlider);
            LethalConfigManager.SetModDescription("Mod for adjusting walkie talkie volume.");


            if (Instance == null)
            {
                Instance = this;
            }

            harmony.PatchAll(typeof(WalkieVolumeBase));
            harmony.PatchAll(typeof(WalkieTalkiePatch));

            mls.LogInfo($"WalkieVolume mod loaded. Walkie volume set to: {WalkieVolume.Value * 100}%");
        }
    }
}
