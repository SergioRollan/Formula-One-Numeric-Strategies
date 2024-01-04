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
* This program uses a channel between C# and Matlab that was discontinued. Therefore, it does not work anymore. The user would have to execute the Matlab script by themselves. See Chapter 7 for more information about this.

--------------------------------------------------------
---------- CHAPTER 2: DRIVERS DATA ----------
-

* The drivers will have the following data stored:
  * Their number, which they will be identified by in the final Matlab image result.
  * Their name and surname, which they will be identified by in the rest of the menus of the app.
  * Their qualifying time, which will be used as their race pace during the race simulation.
  * They starting grid position, which is not necessarily the qualifying times sorted, because any driver could have received a penalty.
  * Every set of tyres they have available in store for the race.
* For each tyre, we will store:
  * The type of compound. '3' is Soft, fastest but least durable. '1' is Hard, slowest but most durable. '2' is Medium which is a point in between.
  * The number of laps it has already raced.

--------------------------------------------------------
---------- CHAPTER 3: TRACK DATA ----------
-

* "Race Pace Difference" reflects how much time the drivers usually lose when they change from going all-out one lap in qualifying to managing for 50+ laps in the race. It will be bigger the longer the track is or the more walls it has (urban circuits for example).
* "Grid Position Time Loss" reflects how much time a drivers lose from starting first, second, third, etc at the start of the race because of the distance there is between their starting lines, in seconds.
* "Standing Start Time Loss" reflects how much time drivers lose in the first lap compared to the others because they start at 0 km/h, in seconds.
* "Starting fuel" reflects how much fuel the drivers have at the start of the race, in kilograms (kg).
* "Fuel Time Loss" reflects how much time a driver with X kg of fuel is faster than a driver with X+1 kg, in seconds.
* "Fuel Mass Loss" reflects how much fuel the drivers lose per lap, in kg.
* "Overtaking Delta Time" reflects how much faster a driver has to be compared to the driver ahead of them to be able to overtake them, in seconds. Just with being faster in itself is not enough, there has to be a bigger difference.
* "Overtaking Time Loss" reflects how much time the driver who overtakes loses because of going out of the fastest line, in seconds.
* "Overtaken Time Loss" reflects how much time the driver who is overtaken loses because of having to fall behind the other driver, in seconds.
* "DRS Time Gain" reflects how much time the drivers gain per lap when they can open the DRS, in seconds.
* "Pit Stop Time Loss (Green Flag)" reflects how much time a driver loses when they make a pit stop while everyone else goes at full speed, in seconds.
* "Pit Stop Time Loss (Yellow Flag)" reflects how much time a driver loses when they make a pit stop while everyone else goes at delta VSC speed, in seconds.
* "Delta VSC Time" reflects the time per lap drivers have to go at when there is a VSC or a SC in that lap, in seconds.
* "Medium Compound Time Diff" reflects the time difference between a driver who wears Soft tyres and a driver who wears Medium Tyres when everything else is the same, in seconds.
* "Hard Compound Time Diff" reflects the time difference between a driver who wears Soft tyres and a driver who wears Hard Tyres when everything else is the same, in seconds.
* "Soft/Medium/Hard Time Loss Per Lap" reflect how much time a driver with X laps of a certain tyre compound is faster than a driver with X+1 laps, in seconds.

--------------------------------------------------------
---------- CHAPTER 4: IMPORT & EXPORT ----------
-

* This feature makes use of the "FilesFONS" directory the user will need to have in their Desktop.
* When using any of the "Export" options (drivers, track or both) the user will write a string [name] and a file named [[name].fons] will be created in the "FilesFONS" folder.
* When using any of the "Import" options (drivers, track or both) the user will write a string [name], the program will search for a [[name].fons] in the "FilesFONS" folder. If the folder does not exist, a file with that name does not exist, it does not match the info the user is looking for (the user is looking for driver and the file stores a track), or the file contains unreadable information, an error message will be shown.

--------------------------------------------------------
---------- CHAPTER 5: PITSTOP DATA ----------
-

* For every driver, the user will be able to set their starting tyres, all their pitstop laps and the tyres they will change to among the available they were set in the drivers data window.
* If a same tyre is chosen twice (for example, as the starting tyres and after the second pitstop), when they are used again, they will have as many laps as they had when they were changed last time (if they had 2 laps of usage at the start and the first pitstop happened in lap 10, when they are worn again, they will start at 12 laps).
* If a pitstop is set for a lap higher than the total laps the race has, the pitstop will never happen.

--------------------------------------------------------
---------- CHAPTER 6: RACE DATA & RESULTS ----------
-

* The user can set the number of laps the driver will have to race
* The user can set periods of accident and the laps they each will last.
  * Yellow flags will not slow drivers down, but overtakes will be forbidden.
  * VSC will slow drivers down to delta pace and overtakes will be forbidden.
  * SC will slow the driver in first place, and everyone else will go at normal pace until they reach the race head.

--------------------------------------------------------
---------- CHAPTER 7: FUTURE WORK ----------
-

* At first, Matlab worked well with the C# channel, as the code has it written in.
* As Matlab discontinued this work, a placeholder example image was placed to always show no matter the data.
* It is unlikely that we get to continue this project, but the first thing to do would be searching for another way of conecting the backend of the app with the matlab commands.
* Safer programming should be implemented. For example, we could check if the VSC pace is faster than the normal pace, or if the overtaking time loss is higher than the overtaken time loss, among other stuff.



