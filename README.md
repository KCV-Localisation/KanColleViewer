﻿KanColleViewer! (提督業も忙しい！)
--

**Old versions (check the Releases section to verify which version is the current) are not (and will not be) supported.**

KCV is a browser utility that makes it easy to play Kantai Collection.
This is the translation project of the original [KanColleViewer](http://grabacr.net/kancolleviewer)

##### Main Contributors
* [@Zharay](http://twitter.com/Zharay) -- English Version
* [@Grabacr07](https://twitter.com/Grabacr07) -- Original author
* [silfumus](https://github.com/silfumus) -- Continued Work (defunct)
* [southrop](https://github.com/southrop) -- Original text file translation code (defunct)
* [m-kc](https://github.com/m-kc) -- Rankings section
* [taihou](https://github.com/taihou) -- Logging option
* [FreyYa](https://github.com/FreyYa) -- Custom Sound

## Downloads
All builds can be found in the following links below.

* [GitHub Releases](https://github.com/Yuubari/KanColleViewer/releases) -- Major revision binaries

## To Do
* An update of the horizontal version of KCV — work in progress
* Some changes to the way construction/drop/development logging is handled — postponed

## Frequently Asked Questions

Refer to the standalone [FAQ](https://github.com/Yuubari/KanColleViewer/blob/master/FAQ.md) if your question is not answered below.

#### Is KCV safe?
Yes. You are mainly playing off Internet Explorer (IE). The program itself wraps itself around IE and captures all network traffic coming in and out. It only looks for incoming data and uses that for displaying game information such as ships, expeditions, quests, and so on. **This does not change the game itself in any way**. **KCV does not change the packets in any way**.

#### How do I use KCV to play Kancolle without a VPN?
Before or after logging in, press the "Set Regional Cookie" button; note: DMM.com should be fully loaded for it to work, don't click it too fast or the script will fail to execute. This will make DMM think of you as of a user located in Japan. It doesn't change your IP or do anything else except change a flag value on your visitor cookie.

#### Is this cookie safe?
By all means it is. But it does mean that it makes playing Kancolle a ton easier for regions that DMM does not support. From their twitter responses, they are alright with foreigners playing the game, but they will not provide any support for those outside of Japan. Until they change this rather open method of delegating foreigners from non-foreigners, this is probably the easiest way to play (and possibly register for) the game itself.

#### My game is choppy when it plays fine in Chrome/Firefox/Flash Projector
Update Internet Explorer to the latest version available for your build of Windows. Windows 7 can go all the way to [Internet Explorer 11](http://windows.microsoft.com/en-us/internet-explorer/ie-11-worldwide-languages) which is by far the fastest version of IE they've made in years.

Also be sure to have the latest version of [Flash for Internet Explorer](http://get.adobe.com/flashplayer/otherversions/) installed!

#### The game will not start or KCV makes me download a flash file instead of opening the link
Install [Flash for Internet Explorer](http://get.adobe.com/flashplayer/otherversions/). You may also have to disable any programs that have been installed on to IE without your consent such as antivirus software and other programs.

#### I'm on Windows 7 and the program does not run!
You need to have [.NET Framework 4.5](http://www.microsoft.com/en-us/download/details.aspx?id=30653) installed for your version of Windows. Those on Windows 8 do not have to do this.

#### I'm on Windows XP/Mac/Linux and I want to have fun too!
Sadly, Microsoft doesn't fully support .NET 4.5. There may be 3rd party ways of getting support for the package, but Microsoft has basically abandoned the XP and other OSs in terms of support. Your only two choices realistically is to either get to Windows 7/8 or use [Logbook](https://github.com/Zharay/logbook) which is another Kancolle project that is being worked on that uses Java and is multi-platform also being worked on by [@Zharay](http://twitter.com/Zharay). It is much simpler, but does an awesome job at what it does.

Important note: while Microsoft has announced that they will be open-sourcing the .NET Core Run-time (which means functional run-times for *NIX systems will be soon developed), porting KCV to *NIX/Linux systems (BSD, OS X, Linux) is not currently possible. KCV depends on two Windows x86/x86_64-only components: Livet and Fiddler. Reimplementing these frameworks would take unrealistic amounts of time.

#### I have a white screen / error message about being unable to connect.
Please let me know if this is happening and tell me what OS you are using and the version of IE. My suspicion is an outdated version of IE. Reports have also shown that clearing your cache+cookies and re-installing flash will also fix this issue.

If you are getting connection errors, try changing the program's proxy server port within KanColleViewer.exe.config. Find the number 37564 and replace it with 37565 or another random number. Restart KCV and see if the problem goes away.

#### My Fleets and Expeditions don't show/update!
Clear your cache and relaunch KCV again. Especially after updates or playing between versions of KCV, this should fix any issues regarding this.

#### I want the original Japanese names for ships/items/etc.
Easiest way is to just disable translations in the options menu. You can also set the UI's language to Japanese which will disable all translations by default. If you only want certain things translated, deleting the corresponding XML file in the translation folder is your best bet (just be sure to disable auto update).

#### I'm missing XXXX translations! (XXXX is in Japanese)
Please help me with these when you run into them. The translation engine has the ability to add untranslated text to the corresponding XML file found in translations. Find the untranslated parts at the bottom of the list and attach them to a new issue on GitHub.

#### Custom Sound Notifications - What?
These are sounds that play immediately when a normal windows notification is to be displayed. Supported formats are WAV and MP3. You must place them in the "Sounds" and under the sub-directory corresponding to the notification you require. It doesn't matter the name or the number you have; a random file will be chosen to be played every time.

Note! For those not in English UI, you may need to place them in the folders that are generated *after* a notification is played once. Any missing folders will be created for you.

#### What is the difference between this and the original KCV by Grabarc07?
* Horizontal version is unique to this project. 
* The translation of all equipment, ships, and quests
* Detailed equipment information
* Logging features
* Ranking information
* EXP Calculator
* Auto updating and version checking
* Regional cookie setting
* Flash quality settings
* Custom sound notifications
* Extra stat display on ships and equipment

Other than the above, this version is the same with some tweaks to make it fit for an English translation. The plan is to hopefully add some features into the main project for all to enjoy.

## About This Project
KanColleViewer uses the web browser component of the Windows Presentation Foundation (WPF) in combination of [FiddlerCore](http://fiddler2.com/fiddlercore) to capture communication packets in-between the server and the page itself.

Of course, we do not change the contents of the packets in anyway and is used to provide information to the program's components.

### About The Translation
Main work on the translation of ships, equipment, and quests fall solely on the now defunct [silfumus' version of KanColleViewer](https://github.com/silfumus/KanColleViewer) and those who contributed to it. Zharay manually merged the changes to this version of the fork and went from there.

The way the translation works is that it manually loads the translation from several text files which holds both the Japanese name and the English translation of the item in question. This can theoretically be used to translate the contents of the game easily to any language needed.

### Main Functions
* Real-time display of elements such as instant repair and build materials.
* Real-time display of the number of Ship Girls and equipment held at the HQ
* List of all ship girls currently in your fleets
* Repair and Construction end timers and notifications.
* List of all quests currently active as well as a listing of available daily and weekly quests.
* Screenshot saving
* Sound muting

### Requirements
* Windows 8 or later (including 8.1 and 10)
* Windows 7 (limited functionality)
* [.NET Framework 4.5](http://www.microsoft.com/ja-jp/download/details.aspx?id=30653) (pre-installed in Windows 8, 8.1 and 10)

Developed and tested mainly in Windows 10 Technical Preview. Windows 7 can also be used to run the program, but you will not have the built in notification system that is found in Windows 8. 

Windows 7 requires that you install .NET Framework 4.5. Windows 8 already has this installed by default.

### Development Environment, Language, Libraries
This is currently being developed using C# + WPF in Windows 10 Technical Preview and Visual Studio 2013.4 Community Edition.

* [Reactive Extensions](http://rx.codeplex.com/)
* Interactive Extensions
* [Windows API Code Pack](http://archive.msdn.microsoft.com/WindowsAPICodePack)
* [Livet](http://ugaya40.net/livet) (MVVM Infrastructure)
* [DynamicJson](http://dynamicjson.codeplex.com/) (JSON deserialization processing)
* [FiddlerCore](http://fiddler2.com/fiddlercore) (Network capture)


#### License
* MIT License

To be released under the MIT license as an open source / free software.
