using ChargingStationTuning.Configuration;
using UnityEngine;

namespace ChargingStationTuning.Shop;

/// <summary>
/// Applies the configured price to an energy crystal.
///
/// <c>ShopManager.CrystalValueGet</c> is the one place the game works out what a crystal
/// costs - it raises the base price by the run's progress and rounds up - and both the shop
/// pool and the price on the shelf go through it. Scaling what it returns therefore changes
/// the price everywhere at once and nowhere twice.
///
/// The result is rounded up and kept at one or more, the same way vanilla does, so a low
/// multiplier cannot hand out free crystals.
/// </summary>
internal static class CrystalPriceTuner
{
    private const float MinimumPrice = 1f;

    internal static float Apply(float vanillaPrice)
    {
        if (!TuningSettings.WantsPriceChange)
            return vanillaPrice;

        float tunedPrice = Mathf.Max(
            MinimumPrice,
            Mathf.Ceil(vanillaPrice * TuningSettings.CrystalPriceMultiplier.Value));

        if (TuningSettings.DebugLogs.Value)
        {
            Plugin.Log.LogInfo(
                $"[Shop] Energy crystal price {vanillaPrice:0} -> {tunedPrice:0} " +
                $"(x{TuningSettings.CrystalPriceMultiplier.Value:0.##}).");
        }

        return tunedPrice;
    }
}
