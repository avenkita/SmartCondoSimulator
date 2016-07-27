using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ReadDoors : MonoBehaviour
{
   
    public static List<DoorClass> ListofDoors = new List<DoorClass>();
    public static List<Wall> ListofWalls = new List<Wall>();

    void Start()
    //gets the list of doors, loads a prefab and calls function to instantiate doors
    {
        object[] thisarray = getDoors();

        //TYPECASTING
        object doors = thisarray[0];
        List<DoorClass> DoorList;
        DoorList = (List<DoorClass>)doors;

        object walls = thisarray[1];
        List<Wall> WallsList;
        WallsList = (List<Wall>)walls;


        GameObject mycube = Resources.Load("Wallprefab") as GameObject;
        InstantiateDoors(mycube, DoorList);
        InstantiateWalls(mycube, WallsList);
    }


    public static void InstantiateDoors(GameObject doorprefab, List<DoorClass> DoorList)
    {
        GameObject emptyd = new GameObject("doors");
        for (int doorcount = 0; doorcount < DoorList.Count; doorcount++)
        {
            var cdoor = DoorList[doorcount];
            GameObject go = Instantiate(doorprefab) as GameObject;
            go.transform.parent = emptyd.transform;
            go.transform.position = new Vector3(cdoor.PositionXd, cdoor.PositionYd, cdoor.PositionZd);
            go.transform.localScale = new Vector3(cdoor.ScaleXd, cdoor.ScaleYd, cdoor.ScaleZd);
            go.name = cdoor.doorid;
            go.tag = "door";
        }
    }

    public static void InstantiateWalls(GameObject doorprefab, List<Wall> WallsList)
    {
        GameObject emptyw = new GameObject("walls");
        for (int wallcount = 0; wallcount < WallsList.Count; wallcount++)
        {
            var cwall = WallsList[wallcount];
            GameObject go = Instantiate(doorprefab) as GameObject;
            go.transform.parent = emptyw.transform;
            go.transform.position = new Vector3(cwall.PositionX, cwall.PositionY, cwall.PositionZ);
            go.transform.localScale = new Vector3(cwall.ScaleX, cwall.ScaleY, cwall.ScaleZ);
            go.tag = "wall";
        }
    }


    public static void ReadDoorsXML()
    {
        TextAsset textXML = (TextAsset)Resources.Load("doorlist", typeof(TextAsset));
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textXML.text);
        XmlNodeList transformList = xmldoc.GetElementsByTagName("doors");

        foreach (XmlNode transformInfo in transformList)
        {
            if (transformInfo.Name == "Doors")
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;
                DoorClass thisdoor = new DoorClass();
                foreach (XmlNode transformItems in transformcontent)
                {
                    if (transformItems.Name == "Door")
                    {
                        XmlNodeList transformcontent2 = transformItems.ChildNodes;
                        foreach (XmlNode transformItems2 in transformcontent2)
                        {
                          //  Debug.Log(transformItems2.InnerText);
                            if (transformItems2.Name == "Name") { thisdoor.doorid = transformItems2.InnerText; }
                            if (transformItems2.Name == "PositionX") { thisdoor.PositionXd = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "PositionY") { thisdoor.PositionYd = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "PositionZ") { thisdoor.PositionZd = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "ScaleX") { thisdoor.ScaleXd = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "ScaleY") { thisdoor.ScaleYd = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "ScaleZ") { thisdoor.ScaleZd = float.Parse(transformItems2.InnerText); }
                        }
                    }
                }
                ListofDoors.Add(thisdoor);
            }
            if (transformInfo.Name == "Walls")
            {
                XmlNodeList transformcontent = transformInfo.ChildNodes;
                foreach (XmlNode transformItems in transformcontent)
                {
                    if (transformItems.Name == "Wall")
                    {
                        XmlNodeList transformcontent2 = transformItems.ChildNodes;
                        Wall thiswall = new Wall();
                        foreach (XmlNode transformItems2 in transformcontent2)
                        {
                         //   Debug.Log(transformItems2.InnerText);
                            if (transformItems2.Name == "PositionX") { thiswall.PositionX = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "PositionY") { thiswall.PositionY = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "PositionZ") { thiswall.PositionZ = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "ScaleX") { thiswall.ScaleX = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "ScaleY") { thiswall.ScaleY = float.Parse(transformItems2.InnerText); }
                            if (transformItems2.Name == "ScaleZ") { thiswall.ScaleZ = float.Parse(transformItems2.InnerText); }
                        }
                        ListofWalls.Add(thiswall);
                    }
                }
            }
        }
    }

    public static object[] getDoors()
    {
        ReadDoorsXML();
        object[] wallsdoorslist = { ListofDoors, ListofWalls };
        return wallsdoorslist;
    }
    

}





