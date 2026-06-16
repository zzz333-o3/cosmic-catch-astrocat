# 🪐 Cosmic Catch — Astrocat

A 2D mobile casual game built with Unity where you control a space cat catching falling fruits, avoiding slimes, and using power-ups across 10 cosmic-themed planets.

---

## 🎮 Gameplay

- **Catch** fruits falling from the sky to earn coins
- **Avoid** slimes — each one costs you a life
- **Use power-ups**: Magnet, Freeze, and Frenzy
- **Collect stars** to unlock new levels and progress through the galaxy
- **Earn coins** to buy skins, trails, and pet companions in the shop

---

## 🌍 Levels (10 Planets)

| # | Planet | Difficulty | Min Stars to Win |
|---|--------|-----------|-----------------|
| 1 | GreenTerra | Easy | ⭐ |
| 2 | OrangeDune | Easy | ⭐ |
| 3 | Mechano | Easy | ⭐ |
| 4 | NeonCity | Medium | ⭐⭐ |
| 5 | IceIgloo | Medium | ⭐⭐ |
| 6 | Crystallia | Medium | ⭐⭐ |
| 7 | Aquamarine | Hard | ⭐⭐ |
| 8 | MagmaPrime | Hard | ⭐⭐ |
| 9 | FoggyVoid | Final | ⭐⭐⭐ |
| 10 | HeartofGalaxy | Final | ⭐⭐⭐ |

---

## 🛍️ Shop

- **Skins** (5): Classic · Gold · Diamond · Pink · Iron
- **Trails** (4): Stars · Diamonds · Rainbow · Fire
- **Pets** (6): Apple Picker · Banana Catcher · Orange Seeker · Grape Gatherer · Slime Hunter · Diamond Miner

Pets unlock at specific level milestones and help collect items automatically!

---

## ⚡ Power-Ups

| Power-Up | Effect | Duration |
|----------|--------|----------|
| 🧲 Magnet | Pulls all fruits toward the player | 7s |
| ❄️ Freeze | Pauses the countdown timer | 7s |
| 🌟 Frenzy | Massively increases item spawn rate | 7s |

---

## 🛠️ Tech Stack

- **Engine**: Unity 2D (C#)
- **UI**: Unity UI + TextMesh Pro
- **Persistence**: Unity PlayerPrefs
- **Audio**: Unity AudioManager (3-source system)
- **Target Platform**: Android / iOS

---

## 📁 Project Structure

```
unity_project/
├── Assets/
│   ├── Scripts/       # All 16 C# game scripts
│   ├── Scenes/        # Game scenes (MainMenu, LoadingScene, 10 levels)
│   ├── Sprites/       # Game art assets
│   ├── Audio/         # Music and SFX
│   └── Prefabs/       # Items, pets, trails, UI prefabs
├── Packages/
└── ProjectSettings/
```

---

## 📜 Scripts

| Script | Role |
|--------|------|
| `GameManager.cs` | Core game state — score, lives, timer, win/lose |
| `AudioManager.cs` | Music & SFX management |
| `LevelManager.cs` | Level order, loading, progression |
| `PlayerController.cs` | Player movement, skins, pets, power-up states |
| `ItemSpawner.cs` | Spawning fruits, slimes, diamonds, power-ups |
| `FallingItem.cs` | Item physics, magnet, collision |
| `PowerUpItem.cs` | Power-up trigger handler |
| `PetAbility.cs` | Pet AI — hunt, retrieve, follow |
| `MainMenuManager.cs` | Menu, shop, settings |
| `LevelButton.cs` | Level select button logic |
| `LevelStarDisplay.cs` | Star display + lock overlay |
| `ButtonSound.cs` | Auto click sound on buttons |
| `AsyncLoader.cs` | Async scene loading |
| `LoadingAnimator.cs` | Loading screen animation |
| `CameraScaler.cs` | Responsive camera scaling |
| `ObjectResizer.cs` | Responsive world object scaling |

---

## 📖 Documentation

Full technical documentation is available in [Confluence](https://gravityfusion.atlassian.net/wiki/spaces/MG/pages/3928817692/Cosmic+Catch+Technical+Documentation).
