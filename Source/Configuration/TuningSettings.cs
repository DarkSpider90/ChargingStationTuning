using BepInEx.Configuration;
using UnityEngine;

namespace ChargingStationTuning.Configuration;

/// <summary>
/// Every setting the mod has, and nothing else.
///
/// The two multipliers are deliberately separate. The charging station's efficiency and the
/// price of an energy crystal are two different decisions - one is how far a crystal goes, the
/// other is what it costs - and tying them to a single number, as the mod this one replaces
/// did, means a player cannot have one without the other.
///
/// Both read as "what vanilla does, times this", so 1.00 is always vanilla and the direction
/// never has to be guessed at.
/// </summary>
internal static class TuningSettings
{
    private const string GeneralSection = "General";
    private const string ChargingSection = "Charging";
    private const string ShopSection = "Shop";

    internal static ConfigEntry<bool> EnableMod { get; private set; }
    internal static ConfigEntry<bool> DebugLogs { get; private set; }
    internal static ConfigEntry<float> EnergyCostMultiplier { get; private set; }
    internal static ConfigEntry<float> CrystalPriceMultiplier { get; private set; }

    internal static void Bind(ConfigFile config)
    {
        EnableMod = config.Bind(
            GeneralSection,
            "Enable Mod",
            true,
            "Turn the whole mod off without uninstalling it.");

        DebugLogs = config.Bind(
            GeneralSection,
            "Debug Logs",
            false,
            "Write a line to the log for every change the mod makes.");

        EnergyCostMultiplier = config.Bind(
            ChargingSection,
            "Energy Cost Multiplier",
            0.5f,
            new ConfigDescription(
                "How much truck energy charging your gear costs, as a share of what the game charges. " +
                "1.00 is exactly vanilla, 0.50 is half the energy for the same charge, 2.00 is twice as " +
                "much. This is the setting the mod exists for.",
                new AcceptableValueRange<float>(0.05f, 20f)));

        CrystalPriceMultiplier = config.Bind(
            ShopSection,
            "Crystal Price Multiplier",
            1f,
            new ConfigDescription(
                "What an energy crystal costs in the shop, as a share of the normal price. 1.00 leaves the " +
                "price alone. Raise it if you want to pay for the cheaper charging rather than get it free.",
                new AcceptableValueRange<float>(0.1f, 10f)));
    }

    /// <summary>True when the mod is on and the charging rate is actually meant to change.</summary>
    internal static bool WantsChargingChange =>
        EnableMod.Value && !Mathf.Approximately(EnergyCostMultiplier.Value, 1f);

    /// <summary>True when the mod is on and the crystal price is actually meant to change.</summary>
    internal static bool WantsPriceChange =>
        EnableMod.Value && !Mathf.Approximately(CrystalPriceMultiplier.Value, 1f);
}
