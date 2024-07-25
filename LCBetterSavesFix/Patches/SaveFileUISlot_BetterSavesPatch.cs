using HarmonyLib;

namespace com.github.zehsteam.LCBetterSavesFix.Patches;

[HarmonyPatch(typeof(SaveFileUISlot_BetterSaves))]
internal class SaveFileUISlot_BetterSavesPatch
{
    [HarmonyPatch(nameof(SaveFileUISlot_BetterSaves.SetFileToThis))]
    [HarmonyPostfix]
    static void SetFileToThisPatch()
    {
        Plugin.logger.LogInfo($"currentSaveFileName: {GameNetworkManager.Instance.currentSaveFileName}, saveFileNum: {GameNetworkManager.Instance.saveFileNum}");
    }
}
