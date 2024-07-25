using HarmonyLib;

namespace com.github.zehsteam.LCBetterSavesFix.Patches;

[HarmonyPatch(typeof(DeleteFileButton_BetterSaves))]
internal class DeleteFileButton_BetterSavesPatch
{
    [HarmonyPatch(nameof(DeleteFileButton_BetterSaves.DeleteFile))]
    [HarmonyPrefix]
    static void DeleteFilePatch(ref DeleteFileButton_BetterSaves __instance)
    {
        string moddataBackupFileName = $"LCSaveFile{__instance.fileToDelete}.bac";

        if (ES3.FileExists(moddataBackupFileName))
        {
            ES3.DeleteFile(moddataBackupFileName);

            Plugin.logger.LogInfo($"Deleted LethalModDataLib {moddataBackupFileName} save file.");
        }
    }
}
