# Netytar
A monophonic software ADMI (Accessible Digital Musical Instrument) playable hands-free through gaze and breath. It is dedicated to musicians who cannot control their hands (e.g. with quadriplegic disability). It has been developed as part of my Master Thesis in Computer Engineering, and maintained as part of my PhD studies and thesis.

Please check the [Netytar page](https://neeqstock.github.io/netytar/) on my personal website for instrument description and details!
Read carefully this Readme to be able to succesfully install and run it.

## System requirements
Netytar is only compatible with Microsoft Windows operating systems, at the present moment. It has been tested on Windows 10; I still didn't check its compatibility with previous Windows versions.

## Hardware requirements
In order to play Netytar as an instrument, the following hardware is required:
- A Tobii Eye Tracker. Netytar has been tested with Tobii EyeX, Tobii 4C (both of these are out of production) and should be compatible with the newer [Tobii Eye Tracker 5](https://gaming.tobii.com/product/eye-tracker-5/);
- (Optional, but required if the user doesn't have control over their hands): Netytar's Breath Sensor. It is buildable for around 20€/25$ without any specific knowledge in electronics using Arduino microcontroller, a sensor, and some parts you can probably find in your local DIY/Bricolage store. A [guide to build it](https://neeqstock.github.io/sensors/) is available in my personal website;

If you don't have one or both these components, you can still download and test Netytar on your PC using your mouse instead of the eye tracker to select notes, and your keyboard to play them in the place of the breath sensor.

## Software requirements and dependencies
According to your configuration, you may need:
- Microsoft [.NET Framework Runtime](https://go.microsoft.com/fwlink/?LinkId=2085155). Download and install it normally;
- [Digital-7 Font](https://www.dafont.com/digital-7.font), downloadable from DaFont. Click on "Download", extract the archive then right click on each font file and Install it for your user or all users;

## Give it a sound!
Netytar is a mute MIDI controller/interface. Windows operating systems already To give it a sound, you can:
- Create a MIDI loop inside your PC, in order to use it as an audio synthesizer. Check the [VST guide](https://neeqstock.github.io/vst_guide/) in my personal website;
- Use an external hardware synthesizer, connecting it to your PC's (or an external) sound card through a MIDI cable.

## Download and install it
Download the latest [Netytar Release](https://github.com/LIMUNIMI/Netytar/releases), **extract it** and run Netytar.exe. No installation is required (it is a portable application). Be careful to before install the **Software requirements and dependencies** listed above!

Have fun!

# Developer? Want to modify the code or compile it by yourself?
Netytar's source code is released under [GNU GPLv3](https://www.gnu.org/licenses/gpl-3.0.en.html) Open Source license, thus you can access, modify, fork, republish its source code complying with that license (e.g. derived works should be published under the same license).

Netytar has been developed using C# programming language, and has dependencies to Windows .NET Framework 4.8 libraries and others. You may need [Microsoft Visual Studio IDE](https://visualstudio.microsoft.com/) in order to load the project and edit it.
Netytar has a **strict dependency** to my NeeqDMIs Digital Musical Instrument prototyping framework. You should download it and replace the reference within Netytar project with NeeqDMIs path in your PC (or just place NeeqDMIs in the same root folder as Netytar's main folder.

Don't hesitate to contact me or open an Issue on this repo if you encounter any problem loading the project.
