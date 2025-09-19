🎮 CubeMerge 3D — TestTaskRaccoonsGames
<p align="center"> <a href="https://www.youtube.com/watch?v=4GVzAM5tUFs&t=13s" target="_blank"> <img src="https://img.youtube.com/vi/4GVzAM5tUFs/0.jpg" alt="Watch the demo" /> </a> </p> <p align="center"> <a href="https://www.youtube.com/watch?v=4GVzAM5tUFs&t=13s" target="_blank"> <img src="https://img.shields.io/badge/Watch%20on-YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white" alt="Watch on YouTube" /> </a> </p>

📝 Description
- A classic 2048 clone in a 3D hyper-casual style:
- The player launches cubes with numbers onto a rectangular board.
- Cubes can be moved left/right before release.
- On collision, cubes with the same value merge into a bigger one.
- Score is granted based on the merged value.
- The game loop is simple, fast, and optimized for hyper-casual gameplay.

🏗️ Architecture
🎯 Game State Machine (high-level flow)
BootstrapState — sets up DI, loads static data, registers services.
LoadProgressState — loads player progress or creates defaults.
LoadMenuState — opens the main menu / UI.
LoadLevelState — loads the level scene and initializes controllers.
GamePlayState — runs gameplay loop (spawning cubes, handling input, merge logic).

Transitions (simplified):
Bootstrap → LoadProgress → LoadMenu → LoadLevel → GamePlay → (Menu | Next Level | Exit)

## 📁 Project Structure

```
Assets/
├── Code/
│   ├── Infrastructure/    # State machines, installers, services
│   ├── Logic/             # Gameplay (Cubes, Merge, Score, Input)
│   ├── UI/                # Views/Presenters
│   ├── StaticData/        # ScriptableObjects (cube values, settings)
│   └── Window/            # Menu & HUD windows
├── Scenes/
└── Plugins/
```

🧰 Tech Stack
Unity 6 / 2022.3 LTS
Zenject — dependency injection
UniTask — async/await for Unity
DOTween — tweening & animations
Odin Inspector — editor tooling
TextMeshPro — typography
(Optional) Addressables — asset management
