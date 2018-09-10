# An inside look at Sega Genesis Collections
Today we'll be looking at three games: Sega Genesis Collection for PlayStation 2, Sega Genesis Collection for PlayStation Portable, and Sonic's Ultimate Genesis Collection for PlayStation 3.
## The folder structures
### PlayStation 2 version
We have the root directory:
![PS2 version root directory](sgc1.png)
* 0APRP310.IMG is a standard PS2 reset file. Okay, I don't actually know that for sure, but it's a binary .IMG file starting with the string "RESET" that I find in almost every PS2 game I examine.
* SLUS_215.42 is a standard PS2 ELF executable found in every game. We'll touch more on this later.
* SYSTEM.CNF contains boot information in plaintext. It points to the above file as the main executable and specifies the region and version of the game.
* 0B has two important files: SEGA_FFE.SR and SEGA_FW.SR. More on that later.
* OCLASSIC has three subfolders representing alphabetical divisions of A-E, F-R, and S-Z. This is where the games' archive files are found. Again, we will return to this.
* 0VIDEOS has the EXTRAS and OTHER subfolders. The former is a collection of the game's interviews; the latter is for miscellaneous things such as splash logos and the credits. These video files are in .PSS format, the standard for full-motion video on PlayStation 2.
* The last three folders match what is found in the 0CLASSIC folder. Except, instead of archive files, they are all ELF files! As mentioned before, ELF is the executable for PlayStation. How interesting.
### PlayStation Portable version
### PlayStation 3 version
## .SR archives
