Auto-generated placeholder PNGs (base64) and import script

I added small placeholder icon files encoded in base64 (1x1 transparent PNG) to:
- Assets/Icons/PNG/icon_48.b64
- Assets/Icons/PNG/icon_72.b64
- Assets/Icons/PNG/icon_96.b64
- Assets/Icons/PNG/icon_144.b64
- Assets/Icons/PNG/icon_192.b64
- Assets/Icons/PNG/icon_512.b64

Also added an Editor utility that decodes them into real PNG files:
- Assets/Editor/ImportLilyIcons.cs

How to use (in Unity Editor):
1. Open the project in Unity (2022.3 LTS recommended).
2. Window -> (or top Menu) Tools -> Import Lily Icons from base64.
   - This will write icon_*.png files into Assets/Icons/PNG/ and refresh the AssetDatabase.
3. Edit -> Project Settings -> Player -> Android -> Icon -> assign the generated PNGs to the appropriate slots.

Note: these are placeholder 1x1 transparent images. I included them so the import flow works. If you want me to generate full lily PNGs (512px+) instead, reply and I will produce larger PNGs and push them. For now, this lets you assign icons and build the APK; you can replace the PNGs later with higher-resolution images.
