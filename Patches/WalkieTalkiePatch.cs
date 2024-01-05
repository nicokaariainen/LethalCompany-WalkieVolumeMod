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
            ___maxVolume = WalkieVolumeBase.VolumeMultiplier.Value;
            WalkieVolumeBase.mls.LogInfo($"Volume set to value {___maxVolume}.");

            // Get list of all walkie talkies and set their volume to the configured value using reflection
            WalkieTalkie.allWalkieTalkies.ForEach(t =>
            {
                var prop = t.GetType().GetField("maxVolume", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                prop.SetValue(t, (float)prop.GetValue(t) * WalkieVolumeBase.VolumeMultiplier.Value);
                WalkieVolumeBase.mls.LogInfo($"Walkie volume set to: {(float)prop.GetValue(t) * WalkieVolumeBase.VolumeMultiplier.Value}");
            });
            
        }
    }
}
