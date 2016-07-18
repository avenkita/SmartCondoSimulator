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
    public static List<SCWalls> ListofWalls = new List<SCWalls>();


    void Start()
    {
        List<SCWalls> WallsList = getWalls();

        GameObject mycube = Resources.Load("Wallprefab") as GameObject;
        XmlTextWriter writer = new XmlTextWriter("C:\\Users\\Aishwarya\\Desktop\\SmartCondo\\SmartCondoSimulator\\SmartCondoSimulator\\Assets\\Resources\\simulationWorldSCwritten.xml", Encoding.UTF8);
        //XmlWriter writer = XmlWriter.Create("C:\\Users\\Aishwarya\\Desktop\\SmartCondo\\SmartCondo2D\\simulationWorldSCwritten.xml", Encoding.UTF8);
       // writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"no\"?> \n");
        writer.WriteStartElement("Walls");
        GameObject empty = new GameObject("SC"); //********
        for (int wallnumber = 0; wallnumber < WallsList.Count; wallnumber++)
        {
            var cw = WallsList[wallnumber];
            GameObject go = Instantiate(mycube) as GameObject;
            go.transform.parent = empty.transform; //********
            float xposition = (cw.Xcoord1 + cw.Xcoord2) / 2;
            float xscale = Mathf.Abs(cw.Xcoord1 - cw.Xcoord2);
            float zposition = (cw.Zcoord1 + cw.Zcoord2) / 2;
            float zscale = Mathf.Abs(cw.Zcoord1 - cw.Zcoord2);

            float angle /*= 0f*/;
            if ((xscale > 0) && (zscale > 0))
            {
                float length = Mathf.Sqrt(Mathf.Pow(xscale, 2) + Mathf.Pow(zscale, 2));
                angle = Mathf.Atan((cw.Zcoord1 - cw.Zcoord2) / (cw.Xcoord1 - cw.Xcoord2)) * 180 / Mathf.PI;
                // go.transform.Rotate(0f, angle + 360 - (2*angle), 0f); //rotation properties are limited!!!! CHECK IF ROTATION IS PROPER, 42.2 WRT X OR Z?
                angle = -angle + 360;
                xscale = length;
                zscale = 1f; //********
            }
            else
            {
                if (xscale == 0)
                {   xscale = zscale;
                    zscale = 1f; //********
                   // go.transform.Rotate(0f, 90f, 0f);
                    angle = 90;
                }
                //if (zscale == 0)
                else
                {
                    zscale = 1f; //********
                    angle = 0;
                }
            }

            go.transform.position = new Vector3(xposition, 50, zposition);
            go.transform.localScale = new Vector3(xscale, 118, zscale); //********
            go.transform.Rotate(0f, angle, 0f);
            string currentname = (wallnumber + 1).ToString();
            go.name = currentname;

            //float angleY = angle + 360f - (2* angle);

            cw.Name = wallnumber.ToString();
            cw.PositionX = xposition.ToString(); Debug.Log(cw.PositionX);
            cw.PositionY = "1.445";
            cw.PositionZ = zposition.ToString();
            cw.ScaleX = xscale.ToString();
            cw.ScaleY = "2.573039";
            cw.ScaleZ = zscale.ToString();
            cw.RotateX = "0";
            cw.RotateY = angle.ToString(); 
            cw.RotateZ = "0";

            //Debug.Log(cw.PositionX);


            writer.Formatting = Formatting.Indented;
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
        writer.WriteEndElement();
        writer.Close();
        
    }


    public static void ReadWallsAXML()
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
                if (transformItems.Name == "actions" || transformItems.Name == "agents" || transformItems.Name == "locations" || transformItems.Name == "sensors") { continue; }
                XmlNodeList transformcontent2 = transformItems.ChildNodes; //gets the tags inside the rooms tag (room)
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    XmlNodeList transformcontent3 = transformItems2.ChildNodes; //gets the tags inside the room tag (roomid and wall)
                    foreach (XmlNode transformItems3 in transformcontent3)
                    {
                        if (transformItems3.Name == "roomid" || transformItems3.Name == "obstacleid") { continue; }
                        XmlNodeList transformcontent4 = transformItems3.ChildNodes; //gets the tags inside the wall tag (point and door)
                        SCWalls wallsimulation = new SCWalls();
                        foreach (XmlNode transformItems4 in transformcontent4) 
                        {
                            if (transformItems4.Name == "door") { continue; } // disregard the door tag 
                            if ((varflag == false) && (transformItems4.Name == "point"))
                            {
                                XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                foreach (XmlNode transformItems5 in transformcontent5)
                                {
                                    if (transformItems5.Name == "xcoord")
                                    {
                                        wallsimulation.Zcoord1 = float.Parse(transformItems5.InnerText) /**0.009f*/; //********
                                     //   Debug.Log(wallsimulation.Zcoord1);
                                    }
                                    if (transformItems5.Name == "ycoord")
                                    {
                                        wallsimulation.Xcoord1 = float.Parse(transformItems5.InnerText) /**0.009f*/; //********
                                     //   Debug.Log(wallsimulation.Xcoord1);
                                    }
                                }
                            }
                            if ((varflag == true) && (transformItems4.Name == "point"))
                            {
                                XmlNodeList transformcontent5 = transformItems4.ChildNodes; //gets the tags inside point tag (xcoord and ycoord)
                                foreach (XmlNode transformItems5 in transformcontent5)
                                {
                                    if (transformItems5.Name == "xcoord")
                                    {
                                        wallsimulation.Zcoord2 = float.Parse(transformItems5.InnerText) /**0.009f*/; //********
                                     //   Debug.Log(wallsimulation.Zcoord2);
                                    }
                                    if (transformItems5.Name == "ycoord")
                                    {
                                        wallsimulation.Xcoord2 = float.Parse(transformItems5.InnerText) /**0.009f*/; //********
                                     //   Debug.Log(wallsimulation.Xcoord2);
                                    }
                                }
                            }
                            varflag = !varflag;
                        }
                        ListofWalls.Add(wallsimulation);
                    }
                }
            }
        }
    }


    public static List<SCWalls> getWalls()
    {
        ReadWallsAXML();
        return ListofWalls;
    }


    void Update() { }
}

