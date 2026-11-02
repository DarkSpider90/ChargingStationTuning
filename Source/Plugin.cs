using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using ChargingStationTuning.Configuration;
using ChargingStationTuning.Station;
using HarmonyLib;

namespace ChargingStationTuning;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; }

    private Harmony _harmony;

    private void Awake()
    {
        Log = Logger;

        TuningSettings.Bind(Config);
        ApplyPatches();
        ChargingStationMembers.ReportAvailability();

        Log.LogInfo($"{MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} loaded.");
    }

    /// <summary>
    /// Patches one class at a time so a single patch that a game update invalidates cannot
    /// abort the whole plugin the way <c>Harmony.PatchAll</c> does.
    /// </summary>
    private void ApplyPatches()
    {
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

        foreach (Type type in AccessTools.GetTypesFromAssembly(Assembly.GetExecutingAssembly()))
        {
            try
            {
                List<MethodInfo> patched = _harmony.CreateClassProcessor(type).Patch();
                if (patched is { Count: > 0 })
                    continue;

                // Harmony silently ignores a class that carries no [HarmonyPatch] attribute of
                // its own, however its methods are annotated, so such a patch never runs and
                // nothing says so. Catch that here instead of in a bug report.
                if (DeclaresHarmonyPatchMethods(type))
                {
                    Log.LogError(
                        $"[Plugin] {type.FullName} declares Harmony patch methods but carries no " +
                        "[HarmonyPatch] attribute on the class itself, so none of them were applied.");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"[Plugin] Failed to apply patches from {type.FullName}; continuing. {ex}");
            }
        }
    }

    private static bool DeclaresHarmonyPatchMethods(Type type)
    {
        foreach (MethodInfo method in AccessTools.GetDeclaredMethods(type))
        {
            if (method.GetCustomAttribute<HarmonyPrefix>() != null ||
                method.GetCustomAttribute<HarmonyPostfix>() != null ||
                method.GetCustomAttribute<HarmonyTranspiler>() != null ||
                method.GetCustomAttribute<HarmonyFinalizer>() != null)
            {
                return true;
            }
        }

        return false;
    }
}
