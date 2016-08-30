# SmartCondoSimulator

##Contents

* Assets Folder (Empty. Not used.)
* Library Folder (Not used.)
* ProjectSettings Folder (Not used.)
* SmartCondoSimulator Folder (Unity project. Used.)
* README.md File
* simulationWorld_picture.jpg File (Image of Alexandr's 2D SmartCondo simulation)
* 2016-07-04-010720-sim.log File (Alexandr's simulation log. Not used, but would have been used for avatar and sensor simulation)

##SmartCondoSimulator Unity Project

*This is the project where the walls, doors, and sensors from Alexandr's simulation XML were instantiated*

###Folder Descriptions

* 3DModels (no longer used, contains some materials and textures)
* Editor (no longer used, contains a script which is not used for the current scene)
* Materials (no longer used, contains two color images and their materials)
* Practice (no longer used, contains codes which I used for practice when I was learning how to script using Instantiate() and transforms)
* **Resources** (used: contains materials, XML files and prefabs which are used in the current working codes)
* **Sensor Scripts** (used: contains extra scripts and prefabs which are used in the current scene)
* SmartCondo Scripts (no longer used: contains SmartCondo instantiation scripts based on previous versions of XMLs)
* Standard Assets (not used but still saved: contains some Unity files)

###How the project works

The scene currently will instantiate walls, doors, door walls, and sensors from Alexandr's simulationWorldSC.xml file. For each component, there is a script called "Compile[component].cs" which will read simulationWorldSC.xml and extract all the tags relating to that component. The information from the XML file is stored into objects and reused to write a new XML file. This new XML file will be Unity-compatible. For example, in simulationWorldSC.xml, a wall has <xcoord> and <ycoord> tags under <point>. The wall is treated as a line and the two <points> specify the coordinates of the two ends of the wall. In Unity, all GameObjects have transform components. Transform has position, scale and rotation in X, Y and Z. Position specifies the position of the center point of the object (by default. Through parenting and other manipulation, this can be modified). Scale specifies the length of the object in the three directions. Rotation specidies the rotation of the object along the three axes. 

Using foreach loops and ChildNodes, the code iterates through the tags (note that I saw suggestions online about using Linq and other methods of efficient XML reading, but I stayed with the looping method). The code compiles all the information for each instance of the component into a class. The list of all of the class objects is retreived in the same code, and after some mathematical computation regarding object size, position, etc., all the information is written into a new, Unity compatible XML file.
For example, for the instantiation of sensors, CompileSensors.cs reads the simulationWorldSC.xml and retreives the "sensors" tag, then loops through each <sensor> tag. As each "sensor" corresponds to information for a single sensor, at this level of the nested foreach loops, an instance of the "Sensorclass.cs" class is created (in Unity, all scripts are classes. The script name must be the same as the overall class name). Sensorclass.cs has fields, like xcoord, ycoord, sensorid, etc. which correspond to the inner tags in <sensor>, which are <xcoord>, <ycoord> and <sensorid> respectively. When the inner foreach loop goes through the tags, if statements appent the text inside the tag into the appropriate field. The object with updated fields is added to the list of objects. This list is used in the start function. Some computations are conducted, and Unity-compatible values for the object are used. for example, 

###XML file descriptions


###Layout of Scenes and description of codes



###Problem areas and what to be wary of

simulationWorldSC tree diagram
Users/Aishwarya/Desktop



