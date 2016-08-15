using UnityEngine;
using System.Collections;
using System.Collections.Generic; //this is where the List<T>() class comes from
using System.Xml;
using System.Text;
using System;
using System.ComponentModel;
using System.Linq;

//A code which can read Alexandr's second XML file (SmartCondo), instantiate the walls, and also write it into an XML file in the Unity-compatible format
//Point tags in the XML had xcoord and ycoord tag inside

public class SmartCondoWalls : MonoBehaviour
{
    //initializes a list of SCWalls type
    public static List<SCWalls> ListofWalls = new List<SCWalls>();


    void Start()
    {
        //WallsList is returned by the getWalls function
        List<SCWalls> WallsList = getWalls();

        //instance of XmlTextWriter is created. File will load into Assets\Resources
        XmlTextWriter writer = new XmlTextWriter("C:\\Users\\Aishwarya\\Desktop\\SmartCondoSimulator\\SmartCondoSimulator\\SmartCondoSimulator\\Assets\\Resources\\simulationWorldSCwritten.xml", Encoding.UTF8);
        writer.WriteStartElement("Walls");

        for (int wallnumber = 0; wallnumber < WallsList.Count; wallnumber++) //loops through all walls in wallslist
        {
            var cw = WallsList[wallnumber];
            if (cw == null) { continue; } //if the wall has a door, it is set to null. This statement avoids a NullReferenceException.
            //for position, the mean of the two coordinate values is taken
            //for scale, the distance between the two coordinates is taken
            float xposition = (cw.Xcoord1 + cw.Xcoord2) / 2;
            float xscale = Mathf.Abs(cw.Xcoord1 - cw.Xcoord2);
            float zposition = (cw.Zcoord1 + cw.Zcoord2) / 2;
            float zscale = Mathf.Abs(cw.Zcoord1 - cw.Zcoord2);

            float angle; //declaring variable
            //this if statement evaluates walls which are diagonal (not constant in both x and y)
            if ((xscale > 0) && (zscale > 0))
            {
                float length = Mathf.Sqrt(Mathf.Pow(xscale, 2) + Mathf.Pow(zscale, 2));
                angle = Mathf.Atan((cw.Zcoord1 - cw.Zcoord2) / (cw.Xcoord1 - cw.Xcoord2)) * 180 / Mathf.PI;
                angle = -angle + 360; //add 360 to make the angle positive for string writing
                //wall is created with xscale as length and zscale as thickness, then rotated in y.
                xscale = length;
                zscale = 1f; 
            }
            else //if the wall is constant in either x or y
            {
                if (xscale == 0)
                {
                    xscale = zscale;
                    zscale = 1f;
                    angle = 90;
                }
                else //if (zscale == 0)
                {
                    zscale = 1f;
                    angle = 0;
                }
            }

            //adding in other properties (Unity compatible) into the current wall class
            cw.Name = wallnumber.ToString();
            cw.PositionX = xposition.ToString();
            cw.PositionY = "50";
            cw.PositionZ = zposition.ToString();
            cw.ScaleX = xscale.ToString();
            cw.ScaleY = "100";
            cw.ScaleZ = zscale.ToString();
            cw.RotateX = "0";
            cw.RotateY = angle.ToString();
            cw.RotateZ = "0";

            //Writing into the XML
            writer.Formatting = Formatting.Indented; //Indent the tags
            writer.WriteStartElement("Wall");
            writer.WriteStartElement("Name"); writer.WriteString(cw.Name); writer.WriteEndElement();
            writer.WriteStartElement("PositionX"); writer.WriteString(cw.PositionX); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString(cw.PositionY); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(cw.PositionZ); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(cw.ScaleX); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString(cw.ScaleY); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(cw.ScaleZ); writer.WriteEndElement();
            writer.WriteStartElement("RotateX"); writer.WriteString(cw.RotateX); writer.WriteEndElement();
            writer.WriteStartElement("RotateY"); writer.WriteString(cw.RotateY); writer.WriteEndElement();
            writer.WriteStartElement("RotateZ"); writer.WriteString(cw.RotateZ); writer.WriteEndElement();
            writer.WriteEndElement();

        }
        writer.WriteEndElement(); //end "Walls"
        writer.Close(); //close Writer just in case there's a reading component.

    }


    public static void ReadWallsAXML()
    {
        //boolean variable varflag is used to separate the two points within each wall. 
        bool varflag = false;
        //XML file is loaded and the XmlDocument class is used.
        TextAsset textXML = (TextAsset)Resources.Load("simulationWorldSC", typeof(TextAsset));
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textXML.text);

        //using the XmlNodeList class to read the XML based on tag names and tag levels: for this code, as simulatedworld is the big parent, all tags will be found
        XmlNodeList transformList = xml.GetElementsByTagName("simulatedworld"); 

        foreach (XmlNode transformInfo in transformList) //has just the simulated world tag!
        {
            XmlNodeList transformcontent = transformInfo.ChildNodes; //gets the tags inside the simulatedworld tag (rooms)
            foreach (XmlNode transformItems in transformcontent)
            {
                //gets only the rooms and obstacles tags
                if (transformItems.Name == "actions" || transformItems.Name == "agents" || transformItems.Name == "locations" || transformItems.Name == "sensors") { continue; }
                XmlNodeList transformcontent2 = transformItems.ChildNodes; //gets the tags inside the rooms or obstacles tag (room or obstacle)
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    XmlNodeList transformcontent3 = transformItems2.ChildNodes; //gets the tags inside the room or obstacle tag (roomid, obstacleid, wall)
                    foreach (XmlNode transformItems3 in transformcontent3)
                    {
                        if (transformItems3.Name == "roomid" || transformItems3.Name == "obstacleid") { continue; } //disregard roomid and obstacleid tags
                        XmlNodeList transformcontent4 = transformItems3.ChildNodes; //gets the tags inside the wall tag (point and door)
                        SCWalls wallsimulation = new SCWalls(); //creates a new instance of SCWalls class
                        foreach (XmlNode transformItems4 in transformcontent4)
                        {
                           if ((varflag == false) && (transformItems4.Name == "point")) //when varflag is false, the first point in the XML is extracted in the loop
                            {
                                XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                foreach (XmlNode transformItems5 in transformcontent5)
                                {
                                    if (transformItems5.Name == "xcoord") { wallsimulation.Zcoord1 = float.Parse(transformItems5.InnerText); }
                                    if (transformItems5.Name == "ycoord") { wallsimulation.Xcoord1 = float.Parse(transformItems5.InnerText); }
                                }
                            }
                            if ((varflag == true) && (transformItems4.Name == "point")) //when varflag is true, the second point in the XML is extracted
                            {
                                XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                foreach (XmlNode transformItems5 in transformcontent5)
                                {
                                    if (transformItems5.Name == "xcoord") { wallsimulation.Zcoord2 = float.Parse(transformItems5.InnerText); }
                                    if (transformItems5.Name == "ycoord") { wallsimulation.Xcoord2 = float.Parse(transformItems5.InnerText); }
                                }
                            }
                            varflag = !varflag; //at the end of each loop, the varflag value is set to the opposite
                            //code "CompileDoors" will instantiate doors and the walls in which they are embedded. Those walls are disregarded here.
                            if (transformItems4.Name == "door") { wallsimulation = null; } 
                        }
                        ListofWalls.Add(wallsimulation); //the instance of the SCWalls class with all information from the point tags is appended into the list.
                    }
                }
            }
        }
    }

    //the getWalls function initiates the ReadWallsAXML function and returns the list of walls with info obtained from the XML file.
    public static List<SCWalls> getWalls()
    {
        ReadWallsAXML();
        return ListofWalls;
    }

}

