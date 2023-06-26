# ExamplesVR
A selection of VR Examples for Unity and Mirror.  
(Please note, this repo is currently a work in progress)

# Setup
1: Download Unity - this example currently uses 2021 LTS  
2: Download VR software - this guide uses Quest 2, https://www.meta.com/gb/quest/setup/  
3: Connect Headset to PC, play with your headset and controllers, get familiar with it.  
4: Browse the Docs and Guides whilst having a Biscuit and Tea break, https://developer.oculus.com/documentation/unity/unity-env-device-setup/  
5: Import this ExampleVR project into a fresh unity Project, packages and plugins should all be included.  
6: Open the scene, MirrorExamplesVR/Scenes/SceneExamplesVR  
![MirrorExamplesVR1](https://github.com/MirrorNetworking/ExamplesVR/assets/57072365/4b567f57-0105-47d5-8d12-a80df94b13a4)  
Edit your Unitys window layout if needed.  
7: Play test, press play in editor, use WASD for movement and mouse for UI selection, click "Auto Start", this checks for broadcasting servers on LAN, if there is none, it starts itself as a Host.  
Interaction is currently limited without headset, you can use ParrelSync to test locally, a secondary Editor or build, clicking Auto Start should automaticlly join the first games server as long as all devices are unblocked and on the same Network.  
(For joining across the internet, use the manual connection address box, relays or dedicated server)  
8: Build to device(s), you can use a mixture of builds or editors if you are limited on VR hardware.
You should be able to see VR players moving around.  
![2023-06-19 at 10 12 16 - VR2b](https://github.com/MirrorNetworking/ExamplesVR/assets/57072365/37c94b4a-1608-4624-9f8e-2b4029213711)  
In the above gif, we have a Quest 2 broadcasted to screen on the Left (Player 2), and Unity Editor (player 1) on the right which is viewing the VR player.

# Notes
1: Quest 2 Mac software has fewer features than Windows as of typing, you cannot Link headset to Editor for direct movement, a build is required.  
2: FIREWALLS - Take into account any anti virus/firewall blocking, along with additional settings for your particular platform (broadcast address iOS, Network Discovery sharing settings to on in Windows etc).  
3: Default XR controller trigger when held down, rapidly closes and opens Quest 2 built-in keyboard on TMP inputfields.  (its a pain, but not something we'l currently sort in this network-focused project)

# To-do
1: Everyone currently spawns at 0,0,0 position and rotation.  
(investigate if force moving them messes with guardian/calibrations?)
