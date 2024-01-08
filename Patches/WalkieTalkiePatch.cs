using HarmonyLib;

namespace WalkieVolume.Patches
{
    [HarmonyPatch(typeof(WalkieTalkie))]
    internal class WalkieTalkiePatch
    {
        [HarmonyPatch("OnEnable")]
        [HarmonyPrefix]
        private static void PatchVolume(ref float ___maxVolume)
        {
            // Set held walkie talkie volume to configured value
            ___maxVolume = WalkieVolumeBase.WalkieVolume.Value;
            WalkieVolumeBase.mls.LogInfo($"Volume set to value {___maxVolume}.");
        }
    }
}
