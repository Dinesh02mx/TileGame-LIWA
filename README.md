# LIWA interactive floor — 33 × 12

Complete Windows x64 player, editable Unity source, floor map, hardware profile and tests. 396 touch tiles, six chains of 66 on controller ports 1–6. The supplied Windows player is the customized build used on the original installation. The clean Editor project uses the same gameplay and network code; it does not depend on the original supplier scene.

## Run on another PC

1. Download this repository as ZIP and extract **all files**, or clone it. Do not run from inside the ZIP. Windows 10/11 x64 is required; this is not a Mac/Linux/Android build.
2. Double-click **Play.cmd** for simulation. Register a name, select 30/60/90 seconds and a mode, then Start. Click the floor preview to simulate tile touches.
3. For the physical floor, connect the controller/receiver and configure the dedicated Ethernet adapter to the installation's static address **192.168.4.1 / 255.255.255.0**. Avoid duplicate addresses. Close other floor software that uses UDP 7800.
4. Open a terminal in this folder and run `Play.cmd -Mode Hardware`. For another address on a /24 floor subnet, use `Play.cmd -Mode Hardware -LocalAddress 192.168.4.2`. Controller/receiver must be on that subnet. Profiles are under Config; the launcher writes the selected profile beside the executable.
5. Connect monitors as **extended displays before launching**. Display 1: registration/live score; display 2: leaderboard; display 3: rules. Windows monitor enumeration can differ between PCs. Restart after changing monitors. With one monitor, local tabs remain usable.
6. Optional desktop shortcut: run `powershell -NoProfile -ExecutionPolicy Bypass -File Tools\CreateDesktopShortcut.ps1`. Move the folder first, then create the shortcut. Default shortcut launches simulation.

The launcher does not change Windows network settings or firewall rules. If Windows requests network permission, allow the game on the trusted dedicated floor network. Do not run two floor apps simultaneously. The extracted folder must be writable for configuration, logs and scores. New PCs start with an empty leaderboard; to migrate existing results, copy LeaderboardTouchCounter.json from the old game folder while both games are closed.

## Games

- **Random Lava Waves:** moving partial red waves with orange warnings; eight single green safe tiles relocate every five seconds. Red contact or missed nine-second safe checkpoint costs two points and one of three lives. One-second damage cooldown; zero lives ends the game. Safe checkpoint awards five points. Receiver loss pauses play.
- **Colour Rush:** blue +1, purple +3, red −2 and combo reset; five targets earn +3 combo bonus. Targets move along the long floor, with a green centre route.
- **Touch Counter:** each new press changes colour and increases total/per-tile counts. Release before pressing again.

All modes support 30/60/90 seconds plus a preparation countdown. Idle floor shows green START letters. Register first, then step on a letter to start. Leaderboards separate mode, duration and simulation/live results.

## Unity project and rebuilding

Open **UnityProject** in Unity Hub with **Unity 2021.3.26f1**, with Windows Build Support (Mono). The original supplier runtime was 2021.3.26f1; its SDK project listed the China-editor variant 2021.3.26f1c1. The clean project targets the standard editor API.

Use **LIWA → Create Boot Scene** once, then Play in the Editor. Bootstrap creates the camera and game UI. Use **LIWA → Build Windows Player** to create Builds/RebuiltWindows/Light.exe plus configuration. Or run the Editor with `-batchmode -quit -projectPath "<repo>\UnityProject" -executeMethod BuildGame.Build -logFile build.log`. Supply your installed Unity.exe path. A Unity license and package restore may be needed. The supplied prebuilt player does not require the Editor.

Source of truth is UnityProject/Assets/Scripts. LegacyPlayerSource preserves the extra components required by the supplied player scene. When changing shared gameplay files, also copy them to LegacyPlayerSource before rebuilding the legacy assembly. Tools/BuildLegacyAssembly.ps1 compiles that assembly using references in the supplied player; it does not silently overwrite a running game. A clean Unity rebuild is preferred for future releases.

Run `powershell -NoProfile -ExecutionPolicy Bypass -File Tools\Test.ps1` for gameplay, mapping, timing, touch-edge and UDP frame tests. Test executables use Windows .NET Framework's compiler. Test output goes to .test-output.

## Hardware and troubleshooting

ELC-408 / HC08 protocol. Default PC 192.168.4.1/24; broadcast 192.168.4.255; receive UDP 7800, send UDP 4626. Receiver wire ID 0; controller wire ID 1. Ports 1–6: 66 tiles each; 7–8 unused. TileMap33x12.csv records every address. Use Mapping / Network in the game to inspect chain boundaries and physical touch addresses before public use. The example mappingVerified flag records the original commissioning configuration; recheck after rewiring.

If disconnected: check the dedicated adapter still has the configured address, close competing floor apps, check controller/receiver power and data link, and inspect Builds/Windows/Game.log and the in-game network status. Repeated black output is not proof of a software fault: verify the supply voltage at the tiles under load. The launcher checks the selected local IP exists; it cannot repair physical wiring. No automatic IP changes are made.

## Package verification and limits

The prebuilt player was physically tested in the original installation before packaging. Packaging tests are recorded in Docs/Validation.txt. A fresh clean-project Unity build requires the Editor and must be verified on the destination hardware before deployment. Hardware, Windows firewall configuration, screen ordering and writable disk access are machine-specific; copying files cannot guarantee operation on every PC.

See THIRD_PARTY_NOTICES.md before redistribution. No personal leaderboards, runtime logs or credentials are included.