using HarmonyLib;

namespace WalkieVolume.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        public const float DEFAULT_VOLUME_ALIVE = 1.0f;
        public const float DEFAULT_VOLUME_DEAD = 0.8f;

        [HarmonyPatch("UpdatePlayerVoiceEffects")]
        [HarmonyPostfix]
        public static void PatchUpdatePlayerVoiceEffects()
        {
            if (GameNetworkManager.Instance == null || GameNetworkManager.Instance.localPlayerController == null)
            {
                return;
            }

            var localPlayerScript = (!GameNetworkManager.Instance.localPlayerController.isPlayerDead 
                || !(GameNetworkManager.Instance.localPlayerController.spectatedPlayerScript != null)
                ? GameNetworkManager.Instance.localPlayerController 
                : GameNetworkManager.Instance.localPlayerController.spectatedPlayerScript);

            foreach (var playerScript in StartOfRound.Instance.allPlayerScripts)
            {
                if ((!playerScript.isPlayerControlled && !playerScript.isPlayerDead)
                    || playerScript == GameNetworkManager.Instance.localPlayerController)
                    continue;

                if (playerScript.voicePlayerState == null 
                    || playerScript.currentVoiceChatIngameSettings._playerState == null 
                    || playerScript.currentVoiceChatAudioSource == null)
                    continue;
                
                // We don't want to modify the volume if the currently looked at player is dead
                if (playerScript.isPlayerDead) continue;

                // Flag that checks whether the local player should hear the current player from a walkie talkie
                var isHeardFromWalkie = 
                    playerScript.speakingToWalkieTalkie 
                    && localPlayerScript.holdingWalkieTalkie 
                    && playerScript != localPlayerScript;

                var defaultVolume = 
                    GameNetworkManager.Instance.localPlayerController.isPlayerDead 
                    ? DEFAULT_VOLUME_DEAD 
                    : DEFAULT_VOLUME_ALIVE;

                // Set volume according to configured setting if player heard from walkie, else set to default.
                if (isHeardFromWalkie)
                {
                    playerScript.voicePlayerState.Volume = defaultVolume * WalkieVolumeBase.WalkieVolume.Value;
                }
            }
        }

    }
}
