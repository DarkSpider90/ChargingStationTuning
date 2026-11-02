# ChargingStationTuning

ChargingStationTuning lets you decide how expensive it is to charge your gear in R.E.P.O.

The truck's charging station runs on energy crystals, and the game decides how far one crystal goes. If you carry a lot of equipment — and especially if you use mods that sell you more of it — that energy runs out long before your gear is topped up. This mod puts the rate in your hands.

## What You Get

- **Cheaper charging.** By default your gear costs half the energy it normally does, so one crystal goes twice as far.
- **Or more expensive.** The same setting works in the other direction if you want charging to hurt.
- **Crystal prices, separately.** You can raise what a crystal costs in the shop to pay for the cheaper charging, or leave prices exactly as they are. The two are separate settings, so you choose each one on its own.
- **Nothing else touched.** The crystal count on the station, the charge bar and everything else behave the way the game intends.

## Settings

The config file is created after you launch the game once with the mod installed:

`BepInEx/config/DarkSpider90.ChargingStationTuning.cfg`

You can also change everything in-game through REPOConfig if you have it.

Every setting reads as "what the game does, times this number", so **1.00 is always exactly vanilla** and you never have to guess which way it goes.

### Charging

**Energy Cost Multiplier** — how much truck energy charging your gear costs. The default is `0.50`, which is half the energy for the same charge. `1.00` is the normal game. `2.00` makes charging twice as expensive. This is the setting the mod exists for.

### Shop

**Crystal Price Multiplier** — what an energy crystal costs in the shop. The default is `1.00`, which leaves prices alone. Raise it if you would rather pay for the cheaper charging than get it for free.

### General

**Enable Mod** — turn the whole mod off without uninstalling it.

**Debug Logs** — write a line to the log for every change the mod makes.

## Multiplayer

Only the host needs the mod. The charging station belongs to the host, so its rate applies to everybody in the lobby.

## Install

Install with a mod manager, or drop `ChargingStationTuning.dll` into your `BepInEx/plugins` folder.

Requires BepInEx. Nothing else.

## ☕ Support ChargingStationTuning

---

> If ChargingStationTuning has kept your gear running, you can support its continued development and compatibility work.
>
> ## [❤️ Support DarkSpider on Ko-fi →](https://ko-fi.com/darkspider)
>
> Every coffee helps. Thank you for supporting the mod.

---

## Licence

MIT. See `LICENSE`.
