LILY APK icon instructions

I added a stylized lily SVG to Assets/Icons/lily.svg as the placeholder icon for the APK. Unity does not use SVGs as APK icons by default unless you install the Vector Graphics package; to apply this icon for Android you should convert it to PNGs at the required sizes and assign them in Player Settings.

Quick steps to set the APK icon to the lily image
1. Convert the SVG to PNGs. Create PNGs at these recommended sizes (square):
   - 48x48, 72x72, 96x96, 144x144, 192x192, 512x512
   You can use any image tool (Inkscape, Photoshop, or an online converter) to export the PNGs.

2. Put the PNGs into the project:
   - Create Assets/Icons/PNG and add the converted PNG files (e.g., icon_512.png).

3. In Unity Editor:
   - Edit -> Project Settings -> Player -> Android tab -> Icon section.
   - Expand "Icon" and assign the PNGs for the different resolutions. Drag the appropriate PNG into each slot.

4. Save and build the APK (File -> Build Settings -> Build).

If you want, I can:
- Convert the SVG to PNGs and add them to the repo now if you tell me to (I can embed PNGs I generate here). Note: generated images will be royalty-free and created by me, so you have full rights to use them.
- Or you can provide a custom lily PNG and I will add it and configure the project to use it.

Which do you prefer? Reply with:
- "Auto-add PNGs" — I will generate PNG icon files from the SVG and push them to Assets/Icons/PNG and include instructions to set them in Player Settings.
- "I will provide PNG" — you will upload the PNG(s) and I will integrate them.
- "Do nothing" — I will leave the SVG placeholder and you can set icons later.
