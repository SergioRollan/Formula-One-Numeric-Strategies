# Formula-One-Numeric-Strategies

PROGRAM THAT BRINGS AN INTUITIVE UI TO CREATE CUSTOM FORMULA ONE RACE SIMULATIONS, AS REALISTICALLY AS POSSIBLE YET STILL EASY TO SET AND EASY TO UNDERSTAND

AUTHORS: SERGIO JUAN ROLLÁN MORALEJO AND JAVIER MATEOS DEL AMO

The summary of the app's functionality can be classified in:
* CHAPTER 1: FILES & REQUIREMENTS TO RUN
* CHAPTER 2: DRIVERS DATA
* CHAPTER 3: TRACK DATA
* CHAPTER 4: IMPORT & EXPORT
* CHAPTER 5: PITSTOP DATA
* CHAPTER 6: RACE DATA & RESULTS
* CHAPTER 7: FUTURE WORK

--------------------------------------------------------
---------- CHAPTER 1: FILES & REQUIREMENTS TO RUN ----------
-

* In "FonsDemo.pptx" the user has a sequence of images that summarize the program's functionality.
* In the ".exe" file, the user that execute the program without needing Visual Studio.
* The app needs to have a "FilesFONS" folder *in the user's desktop* to be able to import and export drivers and tracks correctly. This repository has a folder with the same name that includes some examples for the users to have the opportunity to try the app quicker and more easily. If the user creates their drivers and/or tracks, wants to export them and this folder does not exist, it will be automatically created.
* All the source files can be found in the remaining folder. Visual Studio 2022 or later and Matlab 2022 or later are needed.
* This program uses a channel between C# and Matlab that was discontinued, so it does not work anymore. The user would have to execute the Matlab script by themselves. See Chapter 7 for more information about this.

--------------------------------------------------------
---------- CHAPTER 2: DRIVERS DATA ----------
-

* The drivers will have the following data stored:
  * Their number, which they will be identified by in the final Matlab image result.
  * Their name and surname, which they will be identified by in the rest of the menus of the app.
