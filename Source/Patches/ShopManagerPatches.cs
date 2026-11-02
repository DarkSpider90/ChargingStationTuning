using ChargingStationTuning.Shop;
using HarmonyLib;

namespace ChargingStationTuning.Patches;

/// <summary>
/// Hooks the single place the game prices an energy crystal.
/// </summary>
[HarmonyPatch(typeof(ShopManager))]
internal static class ShopManagerPatches
{
    [HarmonyPatch("CrystalValueGet")]
    [HarmonyPostfix]
    private static void CrystalValueGetPostfix(ref float __result)
    {
        __result = CrystalPriceTuner.Apply(__result);
    }
}
