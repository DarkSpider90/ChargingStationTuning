using ChargingStationTuning.Station;
using HarmonyLib;

namespace ChargingStationTuning.Patches;

/// <summary>
/// Hooks the moment a charging station finishes setting itself up.
/// </summary>
// Harmony skips a class that carries no [HarmonyPatch] attribute of its own, however its
// methods are annotated, so the bare attribute here is what makes the patch below apply.
[HarmonyPatch(typeof(ChargingStation))]
internal static class ChargingStationPatches
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void StartPostfix(ChargingStation __instance)
    {
        ChargeCostTuner.Apply(__instance);
    }
}
