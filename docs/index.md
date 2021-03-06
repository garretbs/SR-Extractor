# An inside look at Sega Genesis Collection
Today we'll be looking at three games: Sega Genesis Collection for PlayStation 2, Sega Genesis Collection for PlayStation Portable, and Sonic's Ultimate Genesis Collection for PlayStation 3.

## Table of contents
1. [The folder structures](#folders)
	1. [PlayStation 2](#ps2folders)
	2. [PlayStation Portable](#pspfolders)
	3. [PlayStation 3](#ps3folders)
2. [.SR archives](#srarchives)
3. [Files within .SR archives](#filesinarchives)
	1. [Files within .SR archives for games](#filesinarchivesgames)
	2. [.68k.QZ files](#68kfiles)
4. [Directly booting the executables](#executables)
5. [Some last things](#lastthings)
6. [Conclusion](#conclusion)
	1. [Why do that?](#whydothat)
	2. [Are you certain it was done that way?](#areyousure)
7. [Summary](#summary)

<a name = "folders"></a>
## The folder structures
<a name = "ps2folders"></a>
### PlayStation 2
We have the root directory:

![PS2 version root directory](sgc1.png)

* 0APRP310.IMG is a standard PS2 reset file. Okay, I don't actually know that for sure, but it's a binary .IMG file starting with the string "RESET" that I find in every PS2 game I examine.
* SLUS_215.42 is a standard PS2 ELF executable found in every game. We'll touch more on this later.
* SYSTEM.CNF contains boot information in plaintext. It points to the above file as the main executable and specifies the region and version of the game.
* 0B has two important files: SEGA_FFE.SR and SEGA_FW.SR. More on that later.
* 0CLASSIC has three subfolders representing alphabetical divisions of A-E, F-R, and S-Z. This is where the games' archive files are found. Again, we will return to this.
* 0VIDEOS has the EXTRAS and OTHER subfolders. The former is a collection of the game's interviews; the latter is for miscellaneous things such as splash logos and the credits. These video files are in .PSS format, the standard for full-motion video on PlayStation 2.
* The last three folders match what is found in the 0CLASSIC folder. Except, instead of archive files, they are all ELF files! As mentioned before, ELF is the executable format for PlayStation systems. How interesting.

<a name = "pspfolders"></a>
### PlayStation Portable
The PSP version is mostly the same, albeit minor differences.

![PSP version "root" directory](sgcpsp2.png)

* These files are not in the root directory, but in the USRDIR folder. The root directory contains system files for the PSP.
* The videos within 0VIDEOS are nearly identical to those found in the PS2 version, but they are in .PMF format. This is the case with any video for PSP.
* There is a .PRX file for every game in this folder. If you were to examine its contents, you would see the familiar ELF header.

<a name = "ps3folders"></a>
### PlayStation 3
This is where things get different. The PS3 "version" is in fact different software with some of the same games. However, it is made by mostly the same people. The differences could offer insight into the development of its predecessor.

![PS3 version "root" directory](sugc5.png)

* Like the PSP version, this is not the root directory. The root contains system files.
* "fmv" contains video files. Curiously, they're not divided into subfolders for interviews and miscellaneous as with the other versions; they are all together. They are .MP4 files as well rather than a proprietary Sony format.
* In "sounds" you will find a "music" folder containing a single file, retro_dreams.msf. This is presumably an archive of music files for PS3. I have yet to find a way to extract its contents.
* "streams" has two files, global_binary.spr and ui.spr. Again, their contents are a mystery to me.
* EBOOT.BIN is your typical PS3 boot file.
* The two .FPG files are also archive files which I cannot peak into.
* pad.file is weird. It's a large file, but its contents are just the null character over and over. Given its name and contents, I would say it serves as padding to fill out the disc space.

<a name = "srarchives"></a>
## .SR archives
When I first popped open the PS2 copy of Sega Genesis Collection, I was looking for one simple thing: the awesome music from the main menu. It's the only song of the collection itself, so there was no official soundtrack release. Best I could find was compressed recordings on YouTube from years ago. Sadly, nothing in the file structure stood out as a music file, but I went and examined things in a text editor anyway. Maybe I'd find some secrets. Well, I did.

![SEGA_FFE.SR header (PS2)](sr1.png)

Say, the contents of the SEGA_FFE.SR file look interesting. The beginning of the file is a regular list, with some binary junk after it. It's like some sort of archive. The first number is slightly bigger than the first number from the previous line. The last bit part of a line is obviously a filename. Oh, and the 44100? I recognize that number. It's a standard sampling frequency for audio data. And it's found on the same line as... a .WAV file!

Every .SR file is laid out the same way:
* First line containing the number of files, the number 80, and the byte offset to the start of the raw data. All in plaintext
* Lines for individual files with the byte offset to the file data, the size of the file in bytes, three special numbers, then the filename. Again, this all in plaintext. For most files the special numbers are 100 and two 0s. Those don't seem to be useful. For audio files, however, the special numbers represent the number of channels, the sampling frequency, and the bit depth. The data in the header of .SR archive contains information that should go in the header of a .WAV file.

<a name = "filesinarchives"></a>
## Files within .SR archives
[I wrote an extractor for files from .SR archives](https://github.com/garretbs/SR-Extractor). I really wanted that music, you know. Let's take a look at what we find in SEGA_FW.SR:

![SEGA_FW.SR contents (PS2)](sgc5.png)

* The .WAV files are just the sounds you here when moving around in the menus.
* Presumably, the .IA files are icon graphics for the PS2 controller's buttons, though I haven't been able to figure out its header to find more information. They all share the same first few characters
* There are various files without an extension (the file list was too large to be shown here). These have that same ELF header yet again. Curiously, trying to boot them individually in PCSX2 causes a memory error.
* The PSP version's SEGA_FW.SR has mostly the same contents. The "ps2_.ia" files instead have "psp_.ia" filenames. The extension-less ELF files are missing as well.

![SEGA_FFE.SR contents (PS2)](sgc2.png)

SEGA_FFE.SR contains the real meat. Finally, we begin to see some insight into how this collection really runs.

* BG.M2V is a video file. It is what appears in the background of the menus. It's just a small looping blue background. In the PSP version, it uses the .PMF format.
* There are various .SWF files representing the menus. That's right, the interactive menus are just flash files being played on the console.
* There are multiple .IA files per game here. I do not believe it is data from the games themselves, but rather metadata for the collection. The menus show text detailing the background behind the games' history.
* The awesome menu music is found here in .WAV format. It even loops perfectly. In the PSP version, it is .AT3. That was the standard audio format for portable Sony devices.
* The PSP version has .SWF files pertaining to its custom menus, such as multiplayer connectivity.

<a name = "filesinarchivesgames"></a>
### Files within .SR archives for games

Now we are at the fulcrum of where things get truly interesting. What files will we find in the .SR archive for a game? Let's take a quick peek at Vectorman (it's a good game by the way, consider giving it a try).

![VECTMAN.SR contents (PS2)](sgc4.png)

* ic_VECTMAN.PNG can be plainly opened in anything that supports PNG images. It's the icon you see in the menu when selecting the game. Given its filename, we can assume that files named ic_*.IA contain icon data.
* The audio data is curious. It's simply audio data from the game itself, sound effects and music. On the PS2 version, the lowercase .wav files are sound effects, and .WAV in uppercase with a lowercase m in the filename is for music. The PSP version uses .wav for sound effects and .AT3 for music. And a bonus: the music files in the PSP version actually have a higher sampling rate than their PS2 counterparts! They clock in with 44100 Hz, whereas PS2's game music kept it down to 22050 Hz. However, the PSP filesize is lower due to using a compressed format.
* Another note about the music files. Certain tracks are separated into two files: one for the prelude, and one that loops perfectly. This was most likely done because the the audio could not play part of one track once before looping another part.
* The file I suspiciously skipped over, VECTMAN.68k.QZ, gives us a clue as to what it is in its filename. 68k is the shorthand for the Motorola 68000, the CPU for the Sega Genesis. .68K is also the file extension for Genesis ROMs as found in [Sega Mega Drive and Genesis Classics](https://store.steampowered.com/app/34270/SEGA_Mega_Drive_and_Genesis_Classics/) on Steam. So presumably these .68k.QZ files are Genesis ROMS, right? Well, probably. Let's dedicate a section to what we'll find.

<a name = "68kfiles"></a>
### .68k.QZ files
These are most likely Genesis ROMs. What do they have in common?
* Similar naming scheme. Vectorman is named VECTMAN in both Sega Genesis Classics and the Steam version
* 68k in filename. Non-Genesis titles in the collection, such as Altered Beast (arcade) and Super Zaxxon, do not have .68k.QZ files. The former has three files: Charz.rom.qz, Program.rom.qz, and Sprites.rom.qz. The latter has GAMENAME.ROM.

What's different?
* Steam files are directly playable in emulator. PS2/PSP files are not.
* File sizes are different. VECTMAN_UE.68K in Steam comes in at 2048kB. VECTMAN.68k.QZ is only 1026kB. ROMs from Steam are stored in a folder named "uncompressed ROMs," suggesting that the ones in SGC are in fact compressed
* Steam ROMs include plaintext information in the header. PS2/PSP ROMs are just raw binary.

<a name = "executables"></a>
## Directly booting the executables
Recall what I said way back near the beginning when talking about folder structures. For each game in Sega Genesis Collection exists one .ELF file (.PRX on PSP, but still has ELF header). That is an executable. What happens if we try to, well, execute one?

![Directly booting VECTMAN.ELF in PCSX2](sgc6a.png) ![Directly booting VECTMAN.ELF in PCSX2](sgc6b.png)

It boots directly to the control prompt in PCSX2, which then opens the game! What if we try opening VECTMAN.PRX in PPSSPP!?

![Directly booting (and failing) VECTMAN.PRX in PPSSPP](sgcpsp1.png)

Oh. It fails to find the files it needs. I am not sure if this is reflective of changes in design or simple a fault in the emulator not shared by PCSX2.

<a name = "lastthings"></a>
## Some last things
I've dumped a lot of information on you so far. The payoff is soon. We just have some miscellaneous last bits of evidence to examine.

* Unique to the PS2/PSP collection are weird audio and graphical issues. These aren't simple glitches. When you get speed shoes in the two Sonic games, the music does not speed up like it should. The special stage music doesn't play at all. Some sound effects in Ristar are completely different from what they should be. Other Genesis compilation releases have no unique game glitches that I know of.
* Sonic the Hedgehog 3 is mysteriously absent in the PS2/PSP versions, but not the PS3 version. Note that Sonic 3 (& Knuckles) has notorious legal issues that prevent it from being released with alterations in any way. It only appears in forms of raw emulation. Pick any rerelease including Sonic 3, and you will find that it is 1:1 with the original game. Pick any rerelease compilation with some new features—3D Genesis titles on 3DS, Sega Genesis Classics with online functionality on PS4/XB1/PC—and Sonic 3 will be missing.

And to review:

* The audio files are plainly laid out in Sega Genesis Collection
* The PS2/PSP versions have executables for each game. So far, I have found no evidence of the same in the PS3 version

<a name = "conclusion"></a>
## Conclusion
You may have read this far and are thinking, "So what?" So what? SO WHAT? So... I think the games in Sega Genesis Collection are not emulated, but run natively.

<a name = "whydothat"></a>
### Why do that?
I can only guess. Perhaps it was an easier way of implementing the hidden games' unique unlock criteria such as beating the second level of Altered Beast. Although this would mean that they overcame that obstacle for the PS3 version. Could it be that they simply weren't allowed to use Sega's internal emulator already made for Sonic Mega Collection due to licensing reasons? Maybe the Sega Genesis Collection was planned for PSP during the PS2 compilation's development, so the developers didn't want to go through all the work of making an individual emulator for each arcade game twice, so they made their own framework to run the games pseudo-natively.

<a name = "areyousure"></a>
### Are you certain it was done that way?
Well, no. I did not work on the game myself, so I could be wrong. There are some tests for this theory. The easiest would be to swap audio offsets to see if different music played. Another would be to crack open the other asset files and do some rearranging with graphics. Unfortunately, I cannot get the game to boot after modifying its files this way.

The collection still seems to include ROMs that are redundant if I am right, but I have yet to make them work in an emulator. Still, a game playing the wrong music as a result of an emulator glitch is infinitesimally unlikely, especially when you consider the games' audio files are stored in modern formats unknown to the Genesis. It is possible that it is simply the asset files being managed independently of the game, or that they are being run at some middle ground between emulation and a ground-up remake. I find this unlikely given that games of this era had all of their code and assets coupled.

<a name = "summary"></a>
## Summary
* PS2/PSP versions are mostly the same game with only platform-specific alterations file formats. PS3 version is essentially different software.
* Games on PS2/PSP are run natively rather than emulated, have their audio files stored in .WAV and .AT3 formats. A music track is split into two files for the prelude, which plays once, and the main looping section.
* Menus are .SWF files played on top of looping video

