using ChargingStationTuning.Configuration;
using UnityEngine;

namespace ChargingStationTuning.Station;

/// <summary>
/// Applies the configured energy cost to a charging station.
///
/// <c>ChargingStation.Start</c> computes <c>batteryChargeCost</c> from the station's own
/// numbers, so the moment just after it is the right one to scale the result: the station is
/// fully set up, and every station gets the same treatment in every level without the mod
/// having to keep track of them.
///
/// It also means the value cannot compound. <c>Start</c> runs once per station object, so the
/// scaling happens once per station, with no bookkeeping to go stale - the alternative,
/// remembering which stations were already dealt with, would rely on instance ids that Unity
/// is free to reuse once an object is gone.
/// </summary>
internal static class ChargeCostTuner
{
    /// <summary>Keeps the cost above zero, since the game divides the stored charge by it.</summary>
    private const float MinimumChargeCost = 0.000001f;

    internal static void Apply(ChargingStation station)
    {
        if (!TuningSettings.WantsChargingChange || station == null)
            return;

        if (!ChargingStationMembers.TryReadChargeCost(station, out float vanillaCost))
            return;

        float tunedCost = Mathf.Max(MinimumChargeCost, vanillaCost * TuningSettings.EnergyCostMultiplier.Value);

        if (!ChargingStationMembers.TryWriteChargeCost(station, tunedCost))
            return;

        if (!TuningSettings.DebugLogs.Value)
            return;

        Plugin.Log.LogInfo(
            $"[Charging] {station.name}: energy per unit of charge {vanillaCost:G6} -> {tunedCost:G6} " +
            $"(x{TuningSettings.EnergyCostMultiplier.Value:0.##}).");
    }
}
