using UnityEngine;
using System.Collections;
using System.Collections.Generic; //this is where the List<T>() class comes from
using System.Xml;


//Code to read Alexandr's first XML file (it describes a space which is not the SmartCondo, it is called "simulationWorld.xml") and instantite just the walls.
//AXML.cs goes with this code.
//Code only reads XML, doesn't write a new one.

public class AXMLReadWalls : MonoBehaviour
{
    //public list of values with type AXML is initialized
    public static List<AXML> ListofWalls = new List<AXML>();


    void Start()
    {
        //WallsList is returned by the getWalls function
        List<AXML> WallsList = getWalls();

        //prefab is loaded
        GameObject mycube = Resources.Load("Wallprefab") as GameObject;

        //for loop will loop through all the items in WallsList, which contains instances of AXML class
        for (int wallnumber = 0; wallnumber < WallsList.Count; wallnumber++)
        {
            var cw = WallsList[wallnumber]; //cw denotes the current AXML instances which the loop is on 
            GameObject go = Instantiate(mycube) as GameObject; //prefab is instantiated
            //for position, the mean of the two coordinate values is taken
            //for scale, the distance between the two coordinates is taken
            float xposition = (cw.Xvalue1 + cw.Xvalue2) / 2;
            float xscale = Mathf.Abs(cw.Xvalue1 - cw.Xvalue2);
            float zposition = (cw.Zvalue1 + cw.Zvalue2) / 2;
            float zscale = Mathf.Abs(cw.Zvalue1 - cw.Zvalue2);

            float angle; //declaring variable
            //this if statement evaluates walls which are diagonal (not constant in both x and y)
            if ((xscale > 0) && (zscale > 0))
            {
                float length = Mathf.Sqrt(Mathf.Pow(xscale, 2) + Mathf.Pow(zscale, 2)); //use Pythagorus for finding length of the hypotenuse
                angle = Mathf.Atan((cw.Zvalue1 - cw.Zvalue2) / (cw.Xvalue1 - cw.Xvalue2)) * 180 / Mathf.PI; //finding the angle between the two walls
                go.transform.Rotate(0f, -angle, 0f); //rotation properties can be tricky, but -angle works for some reason
                //wall is created with xscale as length and zscale as thickness, then rotated in y.
                xscale = length; 
                zscale = 1;
            }
            else //if the wall is constant in either x or y
            {
                //because walls are represented as lines, if they are constant in one direction, they will have a thickness in that direction.
                if (xscale == 0) { xscale = 1; }
                if (zscale == 0) { zscale = 1; }
                angle = 0f;
            }

            //taking the computed information and putting it into the transform for instantiation
            go.transform.position = new Vector3(xposition, 12f, zposition);
            go.transform.localScale = new Vector3(xscale, 24f, zscale);
            go.name = (wallnumber + 1).ToString(); //wall is named in the hierarchy
        }
    }


    //Function ReadWallsAXML reads simulationWorld.xml and puts information into instances of the AXML class
    public static void ReadWallsAXML()
    {
        //boolean variable varflag is used to separate the two points within each wall. 
        //XML file is loaded and the XmlDocument class is used.
        bool varflag = false;
        TextAsset textXML = (TextAsset)Resources.Load("simulationWorld" /*"AXMLwallsinfo"*/, typeof(TextAsset));
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textXML.text);

        //using the XmlNodeList class to read the XML based on tag names and tag levels: for this code, only the "room" tags will be found.
        //note that because of this, the walls in "obstacles" are not accounted for.
        //it is possible to account for all walls by changed the tag names which will be found, and by added more foreach loops.
        XmlNodeList transformList = xml.GetElementsByTagName("room");

        //This foreach loops through the outer level of the XML (parent tags)
        foreach (XmlNode transformInfo in transformList)
        {
            XmlNodeList transformcontent = transformInfo.ChildNodes; //ChildNodes will get the next inner level of tags, so "roomid" and "wall"
            //This foreach loops through the "roomid" and "wall" level
            foreach (XmlNode transformItems in transformcontent)
            {
                if (transformItems.Name == "roomid") { continue; } //this is because the "wall" tags are only needed
                XmlNodeList transformcontent2 = transformItems.ChildNodes; //inner level, including "point" and "door" tags
                AXML wallsimulation = new AXML(); //creating an instance of the AXML class which will store the necessary values.
                //This foreach loops through the "point" and "door" tags
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    if (transformItems2.Name == "door") { continue; } //this is because the "point" tags are only needed
                    //when varflag is false, the first point in the XML is extracted in the loop
                    //when varflag is true, the second point in the XML is extracted
                    //at the end of each loop, the varflag value is set to the opposite
                    //the two numbers separated by a space in each point tag are taken in as a string and saved into the point1 and point2 fields using InnerText
                    if ((varflag == false) && (transformItems2.Name == "point")) { wallsimulation.point1 = transformItems2.InnerText; }
                    if ((varflag == true) && (transformItems2.Name == "point")) { wallsimulation.point2 = transformItems2.InnerText; }
                    varflag = !varflag;
                }
                ListofWalls.Add(wallsimulation); //the instance of the AXML class with all information from the point tags is appended into the list.
            }
        }

        //This foreach loops through all the AXML instances in the list and uses the splitstring function to separate the two coordinate
        foreach (AXML thiswall in ListofWalls)
        {
            float[] coord1 = splitstring(thiswall.point1);
            thiswall.Xvalue1 = coord1[0];
            thiswall.Zvalue1 = coord1[1];
            float[] coord2 = splitstring(thiswall.point2);
            thiswall.Xvalue2 = coord2[0];
            thiswall.Zvalue2 = coord2[1];
        }
    }

    //the splitstring function accepts a string (the InnerText of the point tag), separates the string at the space character and returns an array of the two coordinate floats.
    //the function is very specific to this particular XML and the string setup. If used for something else, it could error!
    public static float[] splitstring(string var)
    {
        string[] pointsplit = var.Split(' ');  
        float Xcoord = float.Parse(pointsplit[0]);
        float Zcoord = float.Parse(pointsplit[1]);
        float[] coordinates = new float[] { Xcoord, Zcoord };
        return coordinates;
    }

    //the getWalls function initiates the ReadWallsAXML function and returns the list of walls with info obtained from the XML file.
    public static List<AXML> getWalls()
    {
        ReadWallsAXML();
        return ListofWalls;
    }

}
