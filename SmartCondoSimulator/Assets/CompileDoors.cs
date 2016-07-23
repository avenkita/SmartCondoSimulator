using UnityEngine;
using System.Collections;
using System.Collections.Generic; //this is where the List<T>() class comes from
using System.Xml;
using System.Text;
using System;
using System.ComponentModel;
using System.Linq;

//REWRITE
//A code which can read Alexandr's second XML file (SmartCondo), instantiate the walls, and also write it into an XML file in the Unity-compatible format
//Point tags in the XML had xcoord and ycoord tag inside


//NOTE code currently only works for walls either along the x or z axis, cannot compute diagonal doors!!

//review null thing
//initialize point vars instead of putting them in objects of DoorClass
//remember door is 20 from the first point!!
//foreach loop is there a command to loop around the same interation again
//why error when transformItems5 loop was put around all the if's rather than just the first if? varflag probably
//four doors at two spots: look at the points

//current problems
//door walls from here not where they should be
//door walls from SmartCondoWalls.cs are still there! 8, 14, 15, 19 are not shown in the hierarchy but there are solid doors in the proper spots nevertheless
//door heights
//what to do about door XML? <-- trying to compile but it doesn't finish the wall writing. stops mid-tag

    //doors are thin according to Alexandr's XML file, they can be wider as long as they don't do over the edges of the wall which encompasses them (if they do, the doors will instantiate but the new walls will be wonky

public class CompileDoors : MonoBehaviour
{

    public static List<DoorClass> ListofWalls = new List<DoorClass>();

    void Start()
    {
        XmlTextWriter writer = new XmlTextWriter("C:\\Users\\Aishwarya\\Desktop\\SmartCondoSimulator\\SmartCondoSimulator\\SmartCondoSimulator\\Assets\\Resources\\doorlist.xml", Encoding.UTF8);
        writer.Formatting = Formatting.Indented;

        GameObject mycube = Resources.Load("Wallprefab") as GameObject;
        GameObject empty = new GameObject("Doors");
        GameObject emptyw = new GameObject("Walls");


        List<DoorClass> WallsList = getWalls();

        writer.WriteStartElement("Doors");
        for (int doornumber = 0; doornumber < WallsList.Count; doornumber++)
        {
            var dw = WallsList[doornumber];

         //   if (dw.doorid == null) { continue; }
            GameObject go = Instantiate(mycube) as GameObject;
            go.transform.parent = empty.transform;
            go.transform.position = new Vector3(dw.PositionXd, 80, dw.PositionZd);
            go.transform.localScale = new Vector3(dw.ScaleXd, 160, dw.ScaleZd);
            go.name = dw.doorid;

            
       //     Debug.Log(dw.PositionXd); Debug.Log(dw.PositionZd); Debug.Log(dw.PositionXw1); Debug.Log(dw.PositionZw1); Debug.Log(dw.PositionXw2); Debug.Log(dw.PositionZw2);
            writer.WriteStartElement("Door");
            writer.WriteStartElement("id"); writer.WriteString(dw.doorid); writer.WriteEndElement();
            writer.WriteStartElement("PositionX"); writer.WriteString(dw.PositionXd.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString("80"); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(dw.PositionZd.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(dw.ScaleXd.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString("160"); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(dw.ScaleZd.ToString()); writer.WriteEndElement();
            writer.WriteEndElement();
        }
        writer.WriteEndElement();

        writer.WriteStartElement("Walls");
        for (int doornumber = 0; doornumber < WallsList.Count; doornumber++)
        {
            var dw = WallsList[doornumber];

            GameObject Wall1 = Instantiate(mycube) as GameObject;
            Wall1.transform.parent = emptyw.transform;
            Wall1.transform.position = new Vector3(dw.PositionXw1, 80, dw.PositionZw1);
            Wall1.transform.localScale = new Vector3(dw.ScaleXw1, 160, dw.ScaleZw1);

            GameObject Wall2 = Instantiate(mycube) as GameObject;
            Wall2.transform.parent = emptyw.transform;
            Wall2.transform.position = new Vector3(dw.PositionXw2, 80, dw.PositionZw2);
            Wall2.transform.localScale = new Vector3(dw.ScaleXw2, 160, dw.ScaleZw2);

            writer.WriteStartElement("Wall");
            writer.WriteStartElement("PositionX"); writer.WriteString(dw.PositionXw1.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString("80"); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(dw.PositionZw1.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(dw.ScaleXw1.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString("160"); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(dw.ScaleZw1.ToString()); writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("Wall");
            writer.WriteStartElement("PositionX"); writer.WriteString(dw.PositionXw2.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString("80"); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(dw.PositionZw2.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(dw.ScaleXw2.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString("160"); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(dw.ScaleZw2.ToString()); writer.WriteEndElement();
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }


    public static void ReadDoors()
    {
        bool varflag = false;
        TextAsset textXML = (TextAsset)Resources.Load("simulationWorldSC", typeof(TextAsset));
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textXML.text);
        XmlNodeList transformList = xml.GetElementsByTagName("simulatedworld" /*room*/); //NEED TO ACCOUNT FOR OBSTACLES. tag was previously just "simulatedworld"

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
                        float xc1 = 0; float zc1 = 0; float xc2 = 0; float zc2 = 0;
                        foreach (XmlNode transformItems4 in transformcontent4)
                        {
                            if (transformItems4.Name == "point")
                            {
                                if (varflag == false)
                                {
                                    XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                    foreach (XmlNode transformItems5 in transformcontent5)
                                    {
                                        if (transformItems5.Name == "xcoord") { zc1 = float.Parse(transformItems5.InnerText); }
                                        if (transformItems5.Name == "ycoord") { xc1 = float.Parse(transformItems5.InnerText); }
                                    }
                                }
                                if (varflag == true)
                                {
                                    XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                    foreach (XmlNode transformItems5 in transformcontent5)
                                    {
                                        if (transformItems5.Name == "xcoord") { zc2 = float.Parse(transformItems5.InnerText); }
                                        if (transformItems5.Name == "ycoord") { xc2 = float.Parse(transformItems5.InnerText); }
                                    }
                                }
                                varflag = !varflag;
                            }
                            else /* (transformItems4.Name == "door") */
                            {
                                DoorClass dw = new DoorClass();
                                XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)

                                foreach (XmlNode transformItems5 in transformcontent5)
                                {
                                    if (transformItems5.Name == "doorid") { dw.doorid = transformItems5.InnerText; }
                                    if (transformItems5.Name == "dooroffset") { dw.dooroffset = float.Parse(transformItems5.InnerText); }
                                    if (transformItems5.Name == "doorsize") { dw.doorsize = float.Parse(transformItems5.InnerText); }
                                }

                                if (xc1 != xc2)
                                {
                                    dw.ScaleXd = dw.doorsize;
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
                                    else {
                                        dw.PositionXd = xc2 + dw.dooroffset;
                                        float doorend1 = dw.PositionXd + (dw.ScaleXd / 2);
                                        float doorend2 = dw.PositionXd - (dw.ScaleXd / 2);
                                        dw.PositionXw1 = (xc1 + doorend1) / 2;
                                        dw.ScaleXw1 = Mathf.Abs(xc1 - doorend1);
                                        dw.PositionXw2 = (xc2 + doorend2) / 2;
                                        dw.ScaleXw2 = Mathf.Abs(xc2 - doorend2);
                                    }

                                }
                                else { dw.PositionXd = xc1; dw.PositionXw1 = xc1; dw.PositionXw2 = xc1; dw.ScaleXd = 1; dw.ScaleXw1 = 1; dw.ScaleXw2 = 1; }

                                if (zc1 != zc2)
                                {
                                    dw.ScaleZd = dw.doorsize;
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
                                    else {
                                        dw.PositionZd = zc1 + dw.dooroffset;
                                        float doorend1 = dw.PositionZd + (dw.ScaleZd / 2);
                                        float doorend2 = dw.PositionZd - (dw.ScaleZd / 2);
                                        dw.PositionZw1 = (zc2 + doorend1) / 2;
                                        dw.ScaleZw1 = Mathf.Abs(zc2 - doorend1);
                                        dw.PositionZw2 = (zc1 + doorend2) / 2;
                                        dw.ScaleZw2 = Mathf.Abs(zc1 - doorend2);
                                    }

                                }
                                else { dw.PositionZd = zc1; dw.PositionZw1 = zc1; dw.PositionZw2 = zc1; dw.ScaleZd = 1; dw.ScaleZw1 = 1; dw.ScaleZw2 = 1; }

                                ListofWalls.Add(dw);
                            }
                        }
                    }
                }
            }
        }
    }


    public static List<DoorClass> getWalls()
    {
        ReadDoors();
        return ListofWalls;
    }


    void Update() { }
}



