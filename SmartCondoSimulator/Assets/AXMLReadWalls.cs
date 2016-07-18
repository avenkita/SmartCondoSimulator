using UnityEngine;
using System.Collections;
using System.Collections.Generic; //this is where the List<T>() class comes from
using System.Xml;


//Code to read Alexandr's first XML file (not SmartCondo) and instantite just the walls.
//The point tags in this file also consisted of two numbers separated by a space, hence the need for a function to split the strings
//function only reads XML, doesn't write


public class AXMLReadWalls : MonoBehaviour
{
    public static List<AXML> ListofWalls = new List<AXML>();


    void Start()
    {
        List<AXML> WallsList = getWalls();

        GameObject mycube = Resources.Load("Wallprefab") as GameObject;
       
        for (int wallnumber = 0; wallnumber < WallsList.Count; wallnumber++)
        {
            var cw = WallsList[wallnumber];
            GameObject go = Instantiate(mycube) as GameObject;
            float xposition = (cw.Xvalue1 + cw.Xvalue2) / 2;
            float xscale = Mathf.Abs(cw.Xvalue1 - cw.Xvalue2);
            float zposition = (cw.Zvalue1 + cw.Zvalue2) / 2;
            float zscale = Mathf.Abs(cw.Zvalue1 - cw.Zvalue2);

            float angle;
           
            if ((xscale > 0) && (zscale > 0))
            { float length = Mathf.Sqrt(Mathf.Pow(xscale, 2) + Mathf.Pow(zscale, 2));
                angle = Mathf.Atan((cw.Zvalue1 - cw.Zvalue2) / (cw.Xvalue1 - cw.Xvalue2)) * 180/Mathf.PI;
                go.transform.Rotate(0f, -angle, 0f); //rotation properties are limited!!!! CHECK IF ROTATION IS PROPER, 42.2 WRT X OR Z?
                xscale = length;
                zscale = 1;
            }
            else
            {
                if (xscale == 0) { xscale = 1; }
                if (zscale == 0) { zscale = 1; }
                angle = 0f;
            }

            go.transform.position = new Vector3(xposition, 12f, zposition);
            go.transform.localScale = new Vector3(xscale, 24f, zscale);
            string currentname = (wallnumber+1).ToString();
            go.name = currentname;

        }
    }


    public static void ReadWallsAXML()
    {
        bool varflag = false;
        TextAsset textXML = (TextAsset)Resources.Load("simulationWorld" /*"simulationWorld"*/ /*"AXMLwallsinfo"*/, typeof(TextAsset));
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textXML.text);
        XmlNodeList transformList = xml.GetElementsByTagName("room"); //NEED TO ACCOUNT FOR OBSTACLES. tag was previously just "simulatedworld"
        
        foreach (XmlNode transformInfo in transformList)
        {
            Debug.Log("transformInfo: " + transformInfo.InnerText); //
            XmlNodeList transformcontent = transformInfo.ChildNodes;
            foreach (XmlNode transformItems in transformcontent)
            {
                Debug.Log("transformItems: " + transformItems.InnerText); //
                XmlNodeList transformcontent2 = transformItems.ChildNodes;
                if (transformItems.Name == "roomid") { continue; } //
                AXML wallsimulation = new AXML();
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    if (transformItems2.Name == "door") { continue; } //
                    Debug.Log("transformItems2: " + transformItems2.InnerText); //
                    if ((varflag == false) &&(transformItems2.Name == "point"))
                    {wallsimulation.point1 = transformItems2.InnerText;
                        Debug.Log("Point2 is " + wallsimulation.point1);
                    }
                    if ((varflag == true) && (transformItems2.Name == "point"))
                    {wallsimulation.point2 = transformItems2.InnerText;
                        Debug.Log("Point1 is " + wallsimulation.point2);
                    }
                    varflag = !varflag;
                }
                ListofWalls.Add(wallsimulation);
            }
        }

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

    public static float[] splitstring(string var) //why static
        {
        string[] pointsplit = var.Split(' '); // notice this could error!!
        float Xcoord = float.Parse(pointsplit[0]);
        float Zcoord = float.Parse(pointsplit[1]);
        float[] coordinates = new float[] { Xcoord, Zcoord };
        return coordinates;
    }

    public static List<AXML> getWalls()
    {
        ReadWallsAXML();
        return ListofWalls;
    }


    void Update() { }
}
