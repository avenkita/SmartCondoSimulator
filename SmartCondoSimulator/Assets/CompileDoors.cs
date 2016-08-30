using UnityEngine;
using System.Collections;
using System.Collections.Generic; //this is where the List<T>() class comes from
using System.Xml;
using System.Text;
using System;
using System.ComponentModel;
using System.Linq;

//REWRITE
//A code which can read Alexandr's second XML file (SmartCondo), instantiate the doors and the walls which encompass them, and also write it into an XML file in the Unity-compatible format

//NOTE code currently only works for walls either along the x or z axis, cannot compute diagonal doors.
//Door offset is either 20 units from the first point, or 20 unity from the second point. Check the image (simulationWorld_picture in Resources) for more information
//Walls number 8, 14, 15, 19 have doors in them and are not shown in the Hierarchy under "Walls" but there are solid doors (11 where 8 was, 2 and 20 where 14 was, 1 where 15 was, 3 where 19 was) in the those spots nevertheless
//Alexandr's simulation interface would detect the duplicated walls and delete them accordingly. Note that in the XML, each room has all the walls it needs, meaning overlapping rooms would have two instances of the wall in the same position.
//door heights are as high as wall heights. There is no part of the wall above the door.
//doors are thin according to Alexandr's XML file, they can be wider as long as they don't do over the edges of the wall which encompasses them (if they do, the doors will instantiate but the new walls will be wonky)

public class CompileDoors : MonoBehaviour
{
    //initializes a list of DoorClass type
    public static List<DoorClass> ListofWalls = new List<DoorClass>();

    void Start()
    {
        //instance of XmlTextWriter is created. File will load into Assets\Resources
        XmlTextWriter writer = new XmlTextWriter("C:\\Users\\Aishwarya\\Desktop\\SmartCondoSimulator\\SmartCondoSimulator\\SmartCondoSimulator\\Assets\\Resources\\doorlist.xml", Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.WriteStartElement("Environment");

        //WallsList is returned by the getWalls function
        List<DoorClass> WallsList = getWalls();
        //WallsList has instances of DoorClass
        //Each instance of DoorClass holds information about the door's transform, and information for the two walls beside it

        //This for loop loops through WallsList and extracts information about the doors only. This is put under the "Doors" parent in XML.
        writer.WriteStartElement("Doors");
        for (int doornumber = 0; doornumber < WallsList.Count; doornumber++)
        {
            var dw = WallsList[doornumber];

            writer.WriteStartElement("Door");
            writer.WriteStartElement("id"); writer.WriteString(dw.doorid); writer.WriteEndElement();
            writer.WriteStartElement("PositionX"); writer.WriteString(dw.PositionXd.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString("50"); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(dw.PositionZd.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(dw.ScaleXd.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString("100"); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(dw.ScaleZd.ToString()); writer.WriteEndElement();
            writer.WriteEndElement();
        }
        writer.WriteEndElement();

        //This for loop loops through WallsList and extracts information about the two walls only. This is put under the "Walls" parent in XML.
        writer.WriteStartElement("Walls");
        for (int count = 0; count < WallsList.Count; count++)
        {
            var dw = WallsList[count];

            writer.WriteStartElement("Wall");
            writer.WriteStartElement("PositionX"); writer.WriteString(dw.PositionXw1.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString("50"); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(dw.PositionZw1.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(dw.ScaleXw1.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString("100"); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(dw.ScaleZw1.ToString()); writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("Wall");
            writer.WriteStartElement("PositionX"); writer.WriteString(dw.PositionXw2.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString("50"); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(dw.PositionZw2.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(dw.ScaleXw2.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString("100"); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(dw.ScaleZw2.ToString()); writer.WriteEndElement();
            writer.WriteEndElement();
        }
        writer.WriteEndElement();

        writer.WriteEndElement(); //closing "Environment"
        writer.Close(); //close writer instance. Will give errors if this is not done!
    }


    public static void ReadDoors()
    {
        //boolean variable varflag is used to separate the two points within each wall. 
        bool varflag = false;
        //XML file is loaded and the XmlDocument class is used.
        TextAsset textXML = (TextAsset)Resources.Load("simulationWorldSC", typeof(TextAsset));
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textXML.text);

        XmlNodeList transformList = xml.GetElementsByTagName("simulatedworld");
        foreach (XmlNode transformInfo in transformList) //has just the simulated world tag!
        {
            XmlNodeList transformcontent = transformInfo.ChildNodes; //gets the tags inside the simulatedworld tag (rooms)
            foreach (XmlNode transformItems in transformcontent)
            {
                if (transformItems.Name != "rooms") { continue; }
                XmlNodeList transformcontent2 = transformItems.ChildNodes; //gets the tags inside the rooms tag (room)
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    XmlNodeList transformcontent3 = transformItems2.ChildNodes; //gets the tags inside the room tag (roomid and wall)
                    foreach (XmlNode transformItems3 in transformcontent3)
                    {
                        if (transformItems3.Name == "roomid") { continue; }
                        XmlNodeList transformcontent4 = transformItems3.ChildNodes; //gets the tags inside the wall tag (point and door)
                        float xc1 = 0; float zc1 = 0; float xc2 = 0; float zc2 = 0; //initialize
                        foreach (XmlNode transformItems4 in transformcontent4)
                        {
                            if (transformItems4.Name == "point") 
                            {
                                if (varflag == false) //when varflag is false, the first point in the XML is extracted in the loop
                                {
                                    XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                    foreach (XmlNode transformItems5 in transformcontent5)
                                    {   //coordinate values are put into the initialized variables. Will later be used if this wall has a door.
                                        if (transformItems5.Name == "xcoord") { zc1 = float.Parse(transformItems5.InnerText); }
                                        if (transformItems5.Name == "ycoord") { xc1 = float.Parse(transformItems5.InnerText); }
                                    }
                                }
                                if (varflag == true) //when varflag is true, the second point in the XML is extracted
                                {
                                    XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                    foreach (XmlNode transformItems5 in transformcontent5)
                                    {   //coordinate values are put into the initialized variables. Will later be used if this wall has a door.
                                        if (transformItems5.Name == "xcoord") { zc2 = float.Parse(transformItems5.InnerText); }
                                        if (transformItems5.Name == "ycoord") { xc2 = float.Parse(transformItems5.InnerText); }
                                    }
                                }
                                varflag = !varflag; //at the end of each loop, the varflag value is set to the opposite
                            }
                            else // (transformItems4.Name == "door") i.e. if there is a door in this wall
                            {
                                DoorClass dw = new DoorClass();
                                XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside door tag (doorid, dooroffset, doorsize)
                                foreach (XmlNode transformItems5 in transformcontent5)
                                {
                                    //append values into the field
                                    if (transformItems5.Name == "doorid") { dw.doorid = transformItems5.InnerText; }
                                    if (transformItems5.Name == "dooroffset") { dw.dooroffset = float.Parse(transformItems5.InnerText); }
                                    if (transformItems5.Name == "doorsize") { dw.doorsize = float.Parse(transformItems5.InnerText); }
                                }

                                if (xc1 != xc2) //if door is in a wall not constant in x
                                {
                                    dw.ScaleXd = dw.doorsize;
                                    //finding middle point of door and two wall dimensions based on where the door is located
                                    if (xc2 > xc1) 
                                    {
                                        dw.PositionXd = xc2 - dw.dooroffset;
                                        float doorend1 = dw.PositionXd + (dw.ScaleXd / 2);
                                        float doorend2 = dw.PositionXd - (dw.ScaleXd / 2);
                                        dw.PositionXw1 = (xc2 + doorend1) / 2;
                                        dw.ScaleXw1 = Mathf.Abs(xc2 - doorend1);
                                        dw.PositionXw2 = (xc1 + doorend2) / 2;
                                        dw.ScaleXw2 = Mathf.Abs(xc1 - doorend2);
                                    }
                                    else //if (xc2 < xc1)
                                    {
                                        dw.PositionXd = xc2 + dw.dooroffset;
                                        float doorend1 = dw.PositionXd + (dw.ScaleXd / 2);
                                        float doorend2 = dw.PositionXd - (dw.ScaleXd / 2);
                                        dw.PositionXw1 = (xc1 + doorend1) / 2;
                                        dw.ScaleXw1 = Mathf.Abs(xc1 - doorend1);
                                        dw.PositionXw2 = (xc2 + doorend2) / 2;
                                        dw.ScaleXw2 = Mathf.Abs(xc2 - doorend2);
                                    }

                                }
                                else //if wall is constant in z
                                { dw.PositionXd = xc1; dw.PositionXw1 = xc1; dw.PositionXw2 = xc1; dw.ScaleXd = 1; dw.ScaleXw1 = 1; dw.ScaleXw2 = 1; }

                                if (zc1 != zc2) //if door is in a wall not constant in z
                                {
                                    dw.ScaleZd = dw.doorsize;
                                    //finding middle point of door and two wall dimensions based on where the door is located
                                    if (zc1 > zc2)
                                    {
                                        dw.PositionZd = zc1 - dw.dooroffset;
                                        float doorend1 = dw.PositionZd + (dw.ScaleZd / 2);
                                        float doorend2 = dw.PositionZd - (dw.ScaleZd / 2);
                                        dw.PositionZw1 = (zc1 + doorend1) / 2;
                                        dw.ScaleZw1 = Mathf.Abs(zc1 - doorend1);
                                        dw.PositionZw2 = (zc2 + doorend2) / 2;
                                        dw.ScaleZw2 = Mathf.Abs(zc2 - doorend2);
                                    }
                                    else //if (zc1 < zc2)
                                    {  
                                        dw.PositionZd = zc1 + dw.dooroffset;
                                        float doorend1 = dw.PositionZd + (dw.ScaleZd / 2);
                                        float doorend2 = dw.PositionZd - (dw.ScaleZd / 2);
                                        dw.PositionZw1 = (zc2 + doorend1) / 2;
                                        dw.ScaleZw1 = Mathf.Abs(zc2 - doorend1);
                                        dw.PositionZw2 = (zc1 + doorend2) / 2;
                                        dw.ScaleZw2 = Mathf.Abs(zc1 - doorend2);
                                    }
                                }
                                else //if wall is constant in z
                                { dw.PositionZd = zc1; dw.PositionZw1 = zc1; dw.PositionZw2 = zc1; dw.ScaleZd = 1; dw.ScaleZw1 = 1; dw.ScaleZw2 = 1; }

                                ListofWalls.Add(dw); //the instance of the DoorClass class with all information from the point tags is appended into the list.
                            }
                        }
                    }
                }
            }
        }
    }


    //the getWalls function initiates the ReadDoors function and returns the list of doors and walls with info obtained from the XML file.
    public static List<DoorClass> getWalls()
    {
        ReadDoors();
        return ListofWalls;
    }

}



