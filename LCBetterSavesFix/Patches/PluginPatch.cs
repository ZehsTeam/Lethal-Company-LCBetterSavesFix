using HarmonyLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace com.github.zehsteam.LCBetterSavesFix.Patches;

[HarmonyPatch(typeof(LCBetterSaves.Plugin))]
internal class PluginPatch
{
    private const string _placeHolderFileName = "PlaceHolder";
    private const string _tempFileFormat = "Temp{0}";

    [HarmonyPatch(nameof(LCBetterSaves.Plugin.NormalizeFileNames))]
    [HarmonyPrefix]
    static void NormalizeFileNamesPatch()
    {
        Plugin.logger.LogInfo("Normalizing LethalModDataLib save files.");

        List<string> moddataFiles = GetFiles("LCSaveFile{0}.moddata");
        RenameFilesToTempFiles(moddataFiles);
        RenameTempFilesToNormalizedFiles(moddataFiles, "LCSaveFile{0}.moddata");

        Plugin.logger.LogInfo("Normalizing LethalModDataLib save backup files.");

        List<string> moddataBackupFiles = GetFiles("LCSaveFile{0}.bac");
        RenameFilesToTempFiles(moddataBackupFiles);
        RenameTempFilesToNormalizedFiles(moddataBackupFiles, "LCSaveFile{0}.bac");
    }

    private static List<string> GetFiles(string fileFormat)
    {
        List<string> fileNames = [];

        foreach (string fileName in ES3.GetFiles())
        {
            if (!ES3.FileExists(fileName)) continue;

            if (Regex.IsMatch(fileName, @"^LCSaveFile\d+$"))
            {
                Plugin.logger.LogInfo($"Found Lethal Company save file: {fileName}");

                int index = int.Parse(fileName.Replace("LCSaveFile", ""));
                string customFileName = string.Format(fileFormat, index);

                if (ES3.FileExists(customFileName))
                {
                    Plugin.logger.LogInfo($"Found save file: {customFileName}");

                    fileNames.Add(customFileName);
                }
                else
                {
                    fileNames.Add(_placeHolderFileName);
                }
            }
        }

        return fileNames;
    }

    private static void RenameFilesToTempFiles(List<string> fileNames)
    {
        int index = 1;

        foreach (string fileName in fileNames)
        {
            if (fileName == _placeHolderFileName)
            {
                index++;
                continue;
            }

            string tempFileName = string.Format(_tempFileFormat, fileName);
            ES3.RenameFile(fileName, tempFileName);
            Plugin.logger.LogInfo($"Renamed {fileName} to {tempFileName}");

            index++;
        }
    }

    private static void RenameTempFilesToNormalizedFiles(List<string> fileNames, string fileFormat)
    {
        int fileIndex = 1;

        foreach (string fileName in fileNames)
        {
            string oldTempFileName = string.Format(_tempFileFormat, fileName);
            string newFileName = string.Format(fileFormat, fileIndex);

            if (fileName == _placeHolderFileName)
            {
                fileIndex++;
                continue;
            }

            if (ES3.FileExists(oldTempFileName))
            {
                ES3.RenameFile(oldTempFileName, newFileName);
                Plugin.logger.LogInfo($"Renamed {oldTempFileName} to {newFileName}");
            }
            else
            {
                Plugin.logger.LogInfo($"Temporary file {oldTempFileName} not found. It might have been moved or deleted.");
            }

            fileIndex++;
        }
    }
}
