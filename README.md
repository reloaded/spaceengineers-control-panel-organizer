# Space Engineers - Control Panel Organizer

Overview
========
Iterates over all control panel groups and renames the terminal blocks within each one with the format of "<Singularized Group Name> [Script Hooks] <Auto Increment ID>". You will now have a more organized and easily searchable control panel list. Only terminal blocks that are assigned to a group will be renamed otherwise they are ignored. 

Notes: 
* I never tested this against blocks with multiple groups, but I suspect the terminal block will inherit the name of the last group the block is assigned to (last block group that was iterated). 
* This script was built so it works with the Automated Inventory Sorting Mod's "script hooks" https://steamcommunity.com/sharedfiles/filedetails/?id=321588701 


Script Hooks
============
Some mods use strings within square brackets "[" and "]" that are extracted and parsed to provide script/mod functionality. CPO respects anything within the square brackets and it supports them being present in the group name as well as block name where the group name script hook takes precedence. 

When the group name contains a script hook the script hook will be copied into the name of each block that's in the group. In the scenario where there is a script hook present in the block name and group name the script hook in the group name takes precedence. When the block name has a script hook and the group name doesn't then the block name will retain the script hook assigned to it. 

For example 
Group Name: Cargo Containers [Ingot/Iron] 

Group Blocks: Cargo Container ; Cargo Container [Ingot/Gold] 

After Rename: Cargo Container [Ingot/Iron] 1 ; Cargo Container [Ingot/Iron] 2 


Plural and Singular Names
=========================
There is basic plural detection in this script so your terminal blocks will receive a singularized name. All you need to do is give the block group a plural name and the script will singularize it when it's renaming each terminal block in the group. When the block group name is already singular it will just use the group name as is. This has only been tested with the english language. 


Default Naming Convention: 
The default naming convention for each terminal block is "%GROUP_NAME% %SCRIPT_HOOKS% %ID%". For example given a group called "Cargos [Ingot/Iron]" each block would get a name of "Cargo [Ingot/Iron] #" where # is replaced by an auto incremented number. 


Examples
========

Interior Lights
---------------
Group name: New Lights 

Block name outcome: 
New Light 1 
New Light 2 
... 


Cargo Containers
----------------
Group name: Cargo Steel Plates 

Block name outcome: 
Cargo Steel Plate 1 
Cargo Steel Plate 2 
... 


Cargo Containers with AIS 
-------------------------
Group name: Cargo Containers [SteelPlate:P3] 

Block name outcome: 
Cargo Container [SteelPlate:P3] 1 
Cargo Container [SteelPlate:P3] 2 
... 


Cargo Containers with AIS #2
----------------------------
Group name: [Components:Split] Components 

Block name outcome: 
Component [Components:Split] 1 
Component [Components:Split] 2 
... 

Assemblers
----------
Group name: Computer assemblers 

Block name outcome: 
Computer assembler 1 
Computer assembler 2 
... 


Community Development
=====================
If you would like to contribute toward this ingame script please feel free to fork the repository and create a pull request on Github.

https://github.com/reloaded/spaceengineers-control-panel-organizer

Notes:
* I am not sure how the SE development community manages unit tests when building in game scripts. I left commented out code that can be uncommented in order to mimmick unit testability.