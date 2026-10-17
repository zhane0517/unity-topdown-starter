# Unity Top-Down Starter

This repository contains a Unity-friendly set of C# scripts for a 2D top-down starter template: singleplayer-ready and written with clear hooks for networking (Mirror / Netcode / Photon).

What is included

- Assets/Scripts/
  - PlayerController.cs
  - Weapon.cs
  - Projectile.cs
  - EnemyAI.cs
  - Health.cs
  - GameManager.cs
  - UIHealthBar.cs
- Assets/Scripts/Networking/
  - MirrorPlayerController.cs
  - MirrorWeapon.cs
  - MirrorProjectile.cs
  - NetworkHealth.cs

Note: This repo does NOT include binary Unity Scenes or prefabs. Place the scripts under `Assets/` in a Unity project (or clone the repo into a Unity project folder) and follow the assembly steps below.

Quick start (manual assembly)

1. Create a new Unity 2D project (recommended Unity LTS 2022.3 / 2023.3).
2. Copy the `Assets/` folder from this repository into your Unity project (or clone the repo inside your Unity Projects folder and open it in Unity Hub).
3. Install packages via Package Manager: Input System, Cinemachine (optional), Mirror (if you want networking).
4. Create prefabs and a scene as described in the project README in chat or the original conversation. Key prefabs:
   - Player prefab: SpriteRenderer, Rigidbody2D (freeze rotation), Collider2D, PlayerController, Health, Weapon, PlayerInput (Send Messages), child FirePoint.
   - Projectile prefab: SpriteRenderer, Rigidbody2D (Kinematic or Dynamic), CircleCollider2D (isTrigger=true), Projectile component.
   - Enemy prefab: SpriteRenderer, Rigidbody2D, Collider2D, EnemyAI, Health.
5. Configure Input Actions (right-click Create > Input Actions). Create `Player` map with Move (Vector2), Fire (Button), Dash (Button). Set PlayerInput component to use this asset and Behavior = Send Messages.

Mirror integration

- The `Assets/Scripts/Networking` folder contains minimal Mirror-compatible components. To use them:
  - Install Mirror via Package Manager or add the Mirror package.
  - Replace your Player prefab script with `MirrorPlayerController` (inherit from NetworkBehaviour) and set up NetworkManager.
  - Ensure server spawns projectiles via `NetworkServer.Spawn`.

If you want, I can:
- Add example InputActions JSON file and a minimal Unity scene.
- Push prefabs and a sample .unity scene (this requires committing Unity binary files — I can do that if you want a ready-to-open project).

How to get these files to your machine

- Recommended: use Git clone:
  git clone https://github.com/zhane0517/unity-topdown-starter.git

- Or download ZIP from the GitHub repository page.

If your downloaded folder is empty or only contains README.md, that means I haven't yet pushed the scripts — I just pushed them now. To pull the newest files, run `git pull` in the cloned folder or re-download the ZIP.
