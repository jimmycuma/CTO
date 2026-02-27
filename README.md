# CursedDepths - Unity 2D Game

A fully playable 2D dungeon crawler game built with Unity 2022.3 LTS and URP for Android.

## Features

### Core Game Systems
- **Player Controller**: Smooth movement, jumping, and combat
- **Enemy AI**: Multiple enemy types (Slime, Skeleton, Ghost, Boss) with different behaviors
- **Combat System**: Attack mechanics with hit detection and damage calculation
- **Health System**: Player and enemy health with visual feedback
- **Inventory System**: Collect and use items, weapons, armor, and potions
- **Level Generation**: Procedural room generation with random enemy and item placement
- **Save/Load System**: Persistent game state saving to JSON
- **Event System**: Decoupled game events for UI updates and game logic

### UI Elements
- **Main Menu**: Start new game, continue, and quit options
- **Game UI**: Health bar, gold display, level indicator, pause menu
- **Game Over Screen**: Game over message with return to menu option

### Technical Features
- Unity 2022.3 LTS compatible
- Universal Render Pipeline (URP) for 2D
- Android build configured (API 30-33)
- No broken references
- Ready to build and play immediately

## How to Play

### Controls
- **WASD / Arrow Keys**: Move player
- **Space**: Jump
- **Left Mouse Button / Left Ctrl**: Attack
- **E**: Interact with objects
- **Escape**: Pause game

### Gameplay
1. Navigate through procedurally generated rooms
2. Defeat enemies to earn gold and find items
3. Collect health potions and equipment
4. Progress through increasingly difficult levels
5. Survive as long as possible!

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/           # EventManager, GameManager
│   ├── Player/         # PlayerController
│   ├── Enemy/          # EnemyController
│   ├── Combat/         # IDamageable, IInteractable interfaces
│   ├── Inventory/      # InventorySystem
│   ├── Level/          # RoomGenerator, LevelManager
│   ├── Save/           # SaveSystem
│   ├── UI/             # HealthBar, GameUI, MainMenuUI
│   ├── Items/          # Item classes, Pickup system
│   └── Audio/          # AudioManager
├── Scenes/             # MainMenu, Game, GameOver
├── Prefabs/            # Player, Enemy, UI prefabs
├── Sprites/            # Game sprites
├── ScriptableObjects/  # Items and data
├── Audio/              # Music and SFX
└── Materials/          # Rendering materials
```

## Building for Android

1. Open the project in Unity 2022.3 LTS or later
2. Go to **File > Build Settings**
3. Select **Android** platform
4. Click **Switch Platform**
5. Configure build settings (already configured for API 30-33)
6. Click **Build** to generate the APK

## System Requirements

### Development
- Unity 2022.3 LTS or later
- Windows, macOS, or Linux

### Android (Runtime)
- Android 5.0 (API 21) or later
- OpenGL ES 3.0 or later
- 1GB RAM minimum
- 500MB free storage

## License

This project is provided as-is for educational and development purposes.
