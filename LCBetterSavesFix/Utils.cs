using System.Text.RegularExpressions;

namespace com.github.zehsteam.LCBetterSavesFix;

internal class Utils
{
    public static int GetNewSaveFileNum()
    {
        int saveFileName = 1;

        foreach (string fileName in ES3.GetFiles())
        {
            if (!ES3.FileExists(fileName)) continue;

            if (Regex.IsMatch(fileName, @"^LCSaveFile\d+$"))
            {
                saveFileName++;
            }
        }

        return saveFileName;
    }
}
