using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.LCBetterSavesFix.Patches;
using HarmonyLib;

namespace com.github.zehsteam.LCBetterSavesFix;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("LCBetterSaves", BepInDependency.DependencyFlags.HardDependency)]
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance;
    internal static ManualLogSource logger;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(PluginPatch));
        harmony.PatchAll(typeof(NewFileUISlot_BetterSavesPatch));
        harmony.PatchAll(typeof(SaveFileUISlot_BetterSavesPatch));
        harmony.PatchAll(typeof(DeleteFileButton_BetterSavesPatch));
    }
}
