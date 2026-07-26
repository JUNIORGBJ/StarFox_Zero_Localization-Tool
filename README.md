<p align="center">
  <a href="README.pt_BR.md">🇧🇷 Português</a> | 🇺🇸 English
</p>

<h1 align="center"><figure>
  <img src="Fox.png">
</figure></h1>

<p align="center">
  WinForms editor for <code>.mcd</code> files from <strong>Star Fox Zero</strong>, focused on localization, text review, charset remapping, and support for <code>.dat</code> packages.
</p>

<p align="center">
  <img alt="Platform" src="https://img.shields.io/badge/Platform-Windows-0078D4?style=for-the-badge">
  <img alt=".NET" src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge">
  <img alt="UI" src="https://img.shields.io/badge/UI-WinForms-0C7CD5?style=for-the-badge">
  <img alt="Visual Studio" src="https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?style=for-the-badge">
</p>

## Overview

The **StarFox Zero Localization Tool** was created to facilitate editing of the game's text files with a native **C# + WinForms** interface.

In addition to string editing, the project also offers tools for:

- validating charset coverage;
- importing and exporting texts via CSV;
- remapping characters and language flags;
- creating and adjusting new glyphs;
- exporting and reimporting textures associated with the font atlas;
- extracting and repacking `.dat`, `.dtt`, `.eff` and `.evn` files.

## Highlights

- Native desktop interface, no web dependencies
- Compatible with Visual Studio 2022 Designer
- Direct workflow to open, edit, validate and save `.mcd` files
- CSV round‑trip translation
- Visual preview of the atlas and glyphs
- Integrated tool for repacking game files

## Features

### MCD Editor

- opening and saving `.mcd` files;
- closing the current file with full UI state reset;
- navigation through events and strings in a tree view;
- direct editing of selected text;
- textual search with navigation between results;
- individual and batch replacement.

### Localization and charset

- validation of missing characters in the charset;
- exporting strings to CSV;
- importing translations from CSV;
- source‑to‑destination character remapping;
- updating language flags of characters;
- creating new characters with visual glyph area selection;
- fine‑tuning width, height and position of the selection.

### Textures and preview

- preview of the texture atlas linked to the glyph;
- enlarged view of the selected region;
- exporting the texture to DDS;
- importing DDS while preserving the current tool workflow.

### DAT Files

The DAT helper tool allows:

- extracting `.dat`, `.dtt`, `.eff` and `.evn` files;
- repacking while respecting the original layout.

## Stack

- **C#**
- **.NET 9.0**
- **Windows Forms**
- **Visual Studio 2022**

## How to Build

### Visual Studio 2022

1. Open `StarFoxZeroLocalizationTool.sln`
2. Select the desired configuration
3. Build normally via Visual Studio

## Usage Flow

1. Open a `.mcd` file
2. Browse entries in the side tree
3. Edit the desired texts
4. Validate charset to locate missing characters
5. Export or import CSV when needed
6. Adjust glyphs and atlas textures if the file requires it
7. Save the new `.mcd`
8. If working with game packages, use the `.dat` tool to extract or repack

## Credits
- Extraction script for `.dat`, `.wta`, `.wtp` files based on code by Brice Videau (BSD 2-Clause License).
- Remaining parts of the project developed in C# by JuniorGBJ.

## License
This project uses the BSD 2-Clause License. See the LICENSE file for more details.
