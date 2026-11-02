using System.Reflection;
using ChargingStationTuning.Configuration;
using HarmonyLib;

namespace ChargingStationTuning.Station;

/// <summary>
/// Resolves the one private field the mod writes, once, and says plainly when it cannot.
///
/// This is the part the mod this one replaces got wrong. It reached for a field called
/// <c>chargeRate</c>, which R.E.P.O. no longer has, took the <c>null</c> that
/// <c>AccessTools.Field</c> returns for a missing field, and called <c>GetValue</c> on it. The
/// result was a <c>NullReferenceException</c> for every charging station in every level, the
/// charging rate silently unchanged, and nothing in the log to say the mod had stopped working
/// - while the crystal price it also changed kept working, so crystals cost more for no
/// benefit at all.
///
/// So the field is looked up once at load, the answer is remembered, and a failure is reported
/// as a single clear error naming the field. A game update can then break this mod loudly
/// instead of quietly.
/// </summary>
internal static class ChargingStationMembers
{
    /// <summary>
    /// Truck energy spent per unit of battery life delivered. <c>ChargingStation.Start</c> sets
    /// it to <c>1f / energyPerCrystal / 100f</c> and <c>ChargeItems</c> divides the station's
    /// stored charge by it, so scaling it is exactly "charging costs this much energy". The
    /// crystal count and the bar are driven by other fields and are left alone.
    /// </summary>
    private const string BatteryChargeCostField = "batteryChargeCost";

    private static FieldInfo batteryChargeCost;
    private static bool resolved;

    /// <summary>Looks the field up once. False when the game no longer has it.</summary>
    internal static bool IsAvailable
    {
        get
        {
            Resolve();
            return batteryChargeCost != null;
        }
    }

    internal static bool TryReadChargeCost(ChargingStation station, out float chargeCost)
    {
        chargeCost = 0f;

        if (!IsAvailable || station == null)
            return false;

        if (batteryChargeCost.GetValue(station) is not float value)
            return false;

        chargeCost = value;
        return true;
    }

    internal static bool TryWriteChargeCost(ChargingStation station, float chargeCost)
    {
        if (!IsAvailable || station == null)
            return false;

        batteryChargeCost.SetValue(station, chargeCost);
        return true;
    }

    /// <summary>
    /// Reports at load time whether the mod can do its job, so the answer is in the log before
    /// anybody wonders why nothing changed.
    /// </summary>
    internal static void ReportAvailability()
    {
        if (IsAvailable)
        {
            if (TuningSettings.DebugLogs.Value)
            {
                Plugin.Log.LogInfo(
                    $"[Station] ChargingStation.{BatteryChargeCostField} resolved; charging changes are active.");
            }

            return;
        }

        Plugin.Log.LogError(
            $"[Station] ChargingStation has no \"{BatteryChargeCostField}\" field in this version of " +
            "R.E.P.O., so the charging cost cannot be changed. The mod needs an update; nothing else it " +
            "does has been touched.");
    }

    private static void Resolve()
    {
        if (resolved)
            return;

        resolved = true;
        batteryChargeCost = AccessTools.Field(typeof(ChargingStation), BatteryChargeCostField);
    }
}
