# LILY - Godot conversion (minimal)

This Godot project was generated to let you run and export your puzzle game without Unity licensing.

What is included
- scenes/MainMenu.tscn (main scene)
- scenes/Level1.tscn (level scene with placeholder Player and Crate nodes)
- scripts/Player.gd, scripts/Crate.gd, scripts/GameManager.gd
- README.md (this file)
- export_presets.cfg (notes / placeholder — you need Godot export templates installed)

How to open
1) Install Godot 4.x (recommended) from https://godotengine.org/download (choose the standard editor).  
2) Open Godot, click "Import" or "Open a Project" and select the folder that contains this project (the folder with project.godot).  
3) The project will appear in the project list. Open it.

How to use your Unity assets
- Copy PNG / WAV files from your Unity project branch (feature/pink-cat-puzzle) into res://assets/ in this project.  
- Use the Godot editor to replace the placeholder sprites/audio in the scenes with your real assets.

Running locally (desktop)
1) Open the project in Godot.  
2) Open scenes/MainMenu.tscn and press Play (F5) to run the project on desktop.

Exporting to Android (quick guide)
1) In Godot, install the export templates: Project -> Install Android Build Template or use the Editor download page.  
2) Android export also requires Android SDK/NDK and Java. See: https://docs.godotengine.org/en/stable/export/android.html
3) Create an Android export preset (Project -> Export -> Add -> Android). Configure the package/identifier (e.g., com.yourname.lily) and keystore if you plan to publish. For testing, Godot can use a debug keystore.
4) Click Export and save the .apk to your machine. Install on device via adb or copy the APK.

Notes
- This is a minimal, immediately runnable Godot skeleton. It uses simple placeholders; I recommend you replace assets with the real sprites and audio from your Unity branch.
- If you want, I can also populate res://assets/ with the PNG/WAV files from your repo — say "copy assets" and I will add them.

If you want instructions in French instead, reply "French instructions".
