>BaseModel 
Allows access of all Model_Subsets to the rest of the programs. The access is mostly used in view, but is also used by files in the Main folder.

>Model_Subsets
Includes algorithms and data colleciton that can be used by alll other programs. Basically the gears for the entire program to function.

>Main
Includes the basics of the running program, including all the major loops. MainProgram uses all the other files in the program to accomplish its running task. These include
-Listen to user by calling api
-Act based off need
-Loop time
-Loop wifi
-Sleep if no actions were taken to an extent
SleepMode does very similar things to main but only looks for one command which is Hey Emma to wake up. Also wakes up if the program is minimized and opened. Lastly, maincommand just reacts to what was said by user if it includes the name emma.

>(Goals to be accomplished in this folder)
-Add email reader duing sleep mode
-Add exta modes [Still in debate] which are 
	-GameMode
	-Twitch/Full AI mode
-Detach as much as possible from the UI Thread
-Add more commands [With convinience]
-Remove API call to python with a python program that does not run "speratly".
-Most diverse file