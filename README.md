# SmartCondoSimulator

##SmartCondo details

The Smart Condo is a research facility located in the Edmonton Clinic Health Academy. It is used to simulate home visits, allowing healthcare professionals to increase their understanding of assisted living devices. The condo features intelligent technologies, such as sensors and smart appliances, providing opportunities for researchers and practitioners to learn how to communicate and collaborate with patients living in intelligent homes.
Visit http://www.hserc.ualberta.ca/Resources/Spaces/SmartCondo.aspx

##Contents

* Assets Folder (Empty. Not used.)
* Library Folder (Not used.)
* ProjectSettings Folder (Not used.)
* SmartCondoSimulator Folder (Unity project. Used.)
* README.md File
* simulationWorld_picture.jpg File (Image of Alexandr's 2D SmartCondo simulation)
* 2016-07-04-010720-sim.log File (Alexandr's simulation log. Not used, but would have been used for avatar and sensor simulation)
* floorplan.png (floorplan of the SmartCondo which I used last summer to make the 3D model, the one which Julia used for her Annotations porject)
* Obstacles.txt (Coordinates of the corners of objects (furniture, walls, cabinets, etc.) corresponding to the floorplan. This is where I got accurate measurements of the Condo and it can be used to modify the SmartCondoSimulator one, which is currently inaccurate in terms of measurements)

##What you need

Unity3d (version 5.3.0)
Microsoft Visual Studio (or you can use MonoDevelop, which comes with Unity)

**Important** please read through this whole file, and especially the last section with "**Problem areas and what to be wary of**"!

##SmartCondoSimulator Unity Project

**This is the project where the walls, doors, and sensors from Alexandr's simulation XML were instantiated**

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

The scene currently will instantiate walls, doors, door walls, and sensors from Alexandr's simulationWorldSC.xml file. For each component, there is a script called "Compile[Component].cs" which will read simulationWorldSC.xml and extract all the tags relating to that component. The information from the XML file is stored into objects and reused to write a new XML file. This new XML file will be Unity-compatible, and will be read in the script called "Read[Component].cs" and the components will be instantated. 

In simulationWorldSC.xml, a wall has xcoord and ycoord tags under point. The wall is treated as a line and the two point tags specify the coordinates of the two ends of the wall. In Unity, all GameObjects have transform components. Transform has position, scale and rotation in X, Y and Z. Position specifies the position of the center point of the object (by default; through parenting, local/world positioning and other manipulation techniques, this can be modified). Scale specifies the length of the object in the three directions. Rotation specifies the rotation of the object along the three axes. To translate between the two coordinate systems, some math must be used. The mean of the two xcoord tags will give us the position in X (middle point). The difference between the two xcoord tags will give us the scale in X. As a wall is derived from a simple cube prefab in Unity, it won't need to be rotated, just stretched and positioned into a Wall shape. In the new Unity-compatible XML, the nine values (PositionX, ScaleX, RotateX, PositionY, ScaleY, etc.) are the tags corresponding to a wall. These tags can be easily read and put into the Unity transform.

Using nested foreach loops and ChildNodes, the code iterates through all the tags (note that I saw suggestions online about using Linq and other methods of efficient XML reading, but I stayed with the looping method, and Victor Fernandez had given me sample codes using this method so I followed it). The code compiles all the information for each instance of the component into a class. The list of all of the class objects is retreived in the same code, and after some mathematical computation regarding object size, position, etc., all the information is written into a new, Unity compatible XML file.

For example, for the instantiation of sensors, CompileSensors.cs reads the simulationWorldSC.xml and retreives the "sensors" tag, then loops through each sensor tag. As each "sensor" corresponds to information for a single sensor, at this level of the nested foreach loops, an instance of the "Sensorclass.cs" class is created (*in Unity, all scripts are classes. The script name must be the same as the overall class name*). Sensorclass.cs has fields, like xcoord, ycoord, sensorid, etc. which correspond to the inner tags in sensor, which are xcoord, ycoord and sensorid respectively. When the inner foreach loop goes through the tags, if statements evaluate which tag is currently being read, and they append the text inside the tag into the appropriate field inside the object. The object with updated fields is added to the list of objects. This list is used in the start function. Some computations are conducted, and Unity-compatible values for the object are used (Position, Scale and Rotation are found). These values are also added into the same SensorClass instances and are then written into a new XML file, sensorlist.xml. ReadSensors.cs reads sensorlist.xml, appends the information to Sensorclass (which also has fiels such as PositionX, ScaleX, etc.), and creates a new list of SensorClass objects, and instantiates the sensors with the appropriate prefab and dimensions. The walls and door codes will stop at this point, but for sensors, I was able to codes some behaviours. They will be given a translucent material, sphere colliders with "is Trigger" checked, and the CapsuleTrigger.cs script is attached to each of them, which codes more specific trigger behaviours.

###XML file descriptions

Please see XMLguide.txt in the main Github project folder! I have described what all the XMLs do. XMLs marked with *** are the XMLs currently being used. The other ones are either obsolete or no longer necessary for the project.

###Layout of Scenes and description of codes

The scene "InstantiateSC.unity" should be run. In it, there are a few empty GameObjects (go) with scripts attached to them. Grey font means the go is disabled, which means that whatever is in the script (Start and Update and a few other functions) will be disabled. In Unity, it is possible to attach more than one script to a GameObject. For a script to work, it has to be attached to a go, or called from a script which is attached to a go. I just named the GameObjects the same as what their script names were to help me run the scene with different combinations of scripts. The capsule GameObject has a rigidbody component and is kinematic. The invisible floor is a flattened cube because the capsule was unnecessarily falling/mving without the floor there stopping it. When the scene is run (using the "play" button) the instantiated objects can be seen. The capsule can be moved around using the arrow keys and once it is in a sensor's area, the sensor will flash between translucent to yellow materials. Pressing the play button will stop the scene's run. Note that for the capsule to be controlled, you must be in the "Game" view (not "Scene" view) and must click on the game screen before moving if you had been working on some other view or folder beforehand. 

####Code List (Note that all codes have comments! Please see Problem areas and what to be wary of bullet 2 below for more information!!)
* CompileWalls.cs - reads simulationWorldSC.xml and writes into simulationWorldSCwritten.xml (in Resources) the wall information according to Unity values
* ReadWalls.cs - reads simulationWorldSCwritten.xml and instantiates walls with the correct dimensions
* SCWalls.cs - a wall class with fields for wall dimensions and names
* Wall.cs - another wall class with fields for wall dimensions and names
* CompileDoors.cs - reads simulationWorldSC.xml and writes into doorlist.xml (in Resources) the door and wall (these are the two walls on either side of the door) information according to Unity values
* ReadDoors.cs - reads doorlist.xml and instantiates walls and doors with the correct dimensions. 
*Please read the comments in the doors files as the doors scripts have the most potential for clean-up and modification!* 
* DoorClass.cs - a door class with fields for door dimensions and wall dimensions for the walls with doors in them
* CompileSensors - reads simulationWorldSC.xml and writes into sensorlist.xml (in Resources) the sensor information according to Unity values
* ReadSensors.cs - reads sensorlist.xml and instantiates sensors with the correct dimensions, and also adds some behaviours to them.
* SensorClass.cs - a sensor class with fields for sensor dimensions, id's and names.
* CapsuleTrigger.cs - a script attached to instantiated sensor which has sensor behaviour codes (OnTrigger functions)

##**Problem areas and what to be wary of**

* simulationWorldSC tree diagram is in the Resources folder and it will help us visualize the structure of simulationWorldSC.xml. The nested foreach loops work very specifically and coincide with the hierarchy in the tree diagram, and if the code or the XML file is modified, it will not work properly and could lead to errors or NullReferenceExceptions.
* **VERY IMPORTANT**
    * **CompileWalls.cs Line 25, CompileDoors.cs Line 28, and CompileSensors.cs Line 67: TextWriter writes into specified file in directory! Make sure your directory location corresponds to your Resources folder in your computer!**
    * **For the Read[Component].cs files, Unity can load a file FROM Resources in order to read it. So, the full directory location is not specified.**
    * When to use Compile[component] scripts: if simulationWorldSC.xml is ever changed (numbers changed, coordinates changed, more objects added, etc.) the Compile codes must be run again to reflect this. If simulationWorldSC.xml is not changed, it is sufficient to just run the Read[component] codes. Note that if you make a new XML in the Unity-compatible format, only the "Read[component]" codes need to be run, with the Unity-compatible XML loaded from Resources! This is why the Unity-compatible format was used, so that we can differentiate what info comes from Alexandr, and what info is required by Unity.
* Unity can Load files or prefabs only from the Resources folder (as far as I know) using Resources.Load("stringfilename"). 
* SmartCondo dimensions were from the XML file and correspond to Alexandr's simulation. The simulation is not accurate. The dimensions are 892 by 896 Unity units, whereas the actual SmartCondo is rectangular in shape and should be 10.6 by 6.3 units. 
* The walls and heights are along the Unity Y axis. You will see that in the "Compile[Component]" codes, they are given values in the code itself (not from simulatedWorldSC.xml because that file defines the 2D space!). They need to be modified manually if changes are made in the code.


##Extra
https://drive.google.com/open?id=0B-gl5x9nUBSpRktobXRPQS1sejg Please see this folder for more documentation! My log is a comprehensive document with many entries. Other Ideas/Extended To Do List talks about some of the shortcomings of the Unity projects and also things that are still left to do. Helpful Links has links to forums/Unity documentation sorted by topic. Extra code has some of the code lines I deleted but still wanted to preserve elsewhere. There is also a video of the capsule moving around and triggering the sensors, which also shows how to run the scene and view the game through Game and Scene views.


