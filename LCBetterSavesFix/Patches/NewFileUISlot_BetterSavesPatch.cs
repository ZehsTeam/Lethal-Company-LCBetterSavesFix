using HarmonyLib;

namespace com.github.zehsteam.LCBetterSavesFix.Patches;

[HarmonyPatch(typeof(NewFileUISlot_BetterSaves))]
internal class NewFileUISlot_BetterSavesPatch
{
    [HarmonyPatch(nameof(NewFileUISlot_BetterSaves.SetFileToThis))]
    [HarmonyPostfix]
    static void SetFileToThisPatch()
    {
        int newSaveFileNum = Utils.GetNewSaveFileNum();
        string newSaveFileName = $"LCSaveFile{newSaveFileNum}";

        GameNetworkManager.Instance.currentSaveFileName = newSaveFileName;
        GameNetworkManager.Instance.saveFileNum = newSaveFileNum;

        Plugin.logger.LogInfo($"Set new save file name to {newSaveFileName}");
    }
}
