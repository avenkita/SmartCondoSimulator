using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ReadDoors : MonoBehaviour
{

    //initializes a list of DoorClass type
    public static List<DoorClass> ListofDoors = new List<DoorClass>();
    //initializes a list of Wall type
    public static List<Wall> ListofWalls = new List<Wall>();

    void Start()
    //gets the list of doors and walls separately, loads a prefab and calls function to instantiate doors
    {
        object[] thisarray = getDoors();

        //Typecasting
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

    //function InstantiateDoors has arguments of the prefab and the list of doors, and instantiates the doors in the list
    public static void InstantiateDoors(GameObject doorprefab, List<DoorClass> DoorList)
    {
        GameObject emptyd = new GameObject("Doors");
        for (int doorcount = 0; doorcount < DoorList.Count; doorcount++)
        {
            var cdoor = DoorList[doorcount];
            GameObject go = Instantiate(doorprefab) as GameObject;
            go.transform.parent = emptyd.transform; //all doors parented under "Doors" GameObject
            go.transform.position = new Vector3(cdoor.PositionXd, cdoor.PositionYd, cdoor.PositionZd);
            go.transform.localScale = new Vector3(cdoor.ScaleXd, cdoor.ScaleYd, cdoor.ScaleZd);
            go.name = cdoor.doorid;
        }
    }

    //function InstantiateWalls has arguments of the prefab and the list of walls, and instantiates the walls in the list
    public static void InstantiateWalls(GameObject doorprefab, List<Wall> WallsList)
    {
        GameObject emptyw = new GameObject("Walls beside Doors");
        for (int wallcount = 0; wallcount < WallsList.Count; wallcount++)
        {
            var cwall = WallsList[wallcount];
            GameObject go = Instantiate(doorprefab) as GameObject;
            go.transform.parent = emptyw.transform; //all walls parented under "Walls" GameObject
            go.transform.position = new Vector3(cwall.PositionX, cwall.PositionY, cwall.PositionZ);
            go.transform.localScale = new Vector3(cwall.ScaleX, cwall.ScaleY, cwall.ScaleZ);
        }
    }


    public static void ReadDoorsXML()
    {
        //XML file is loaded and the XmlDocument class is used
        TextAsset textXML = (TextAsset)Resources.Load("doorlist", typeof(TextAsset));
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textXML.text);

        XmlNodeList transformList = xmldoc.GetElementsByTagName("Environment"); //get all the innertext
        foreach (XmlNode transformInfo in transformList)
        {
            XmlNodeList transformcontent = transformInfo.ChildNodes; //gets tags inside Environment (Doors, Walls)
            foreach (XmlNode transformItems in transformcontent)
            {
                if (transformItems.Name == "Doors") //doors information will be added into instances of Doorclass class
                {
                    XmlNodeList transformcontent1 = transformItems.ChildNodes;
                    foreach (XmlNode transformItems1 in transformcontent1)
                    {
                        if (transformItems1.Name == "Door")
                        {
                            XmlNodeList transformcontent2 = transformItems1.ChildNodes;
                            DoorClass thisdoor = new DoorClass();
                            foreach (XmlNode transformItems2 in transformcontent2)
                            {
                                if (transformItems2.Name == "id") { thisdoor.doorid = transformItems2.InnerText; }
                                if (transformItems2.Name == "PositionX") { thisdoor.PositionXd = float.Parse(transformItems2.InnerText); }
                                if (transformItems2.Name == "PositionY") { thisdoor.PositionYd = float.Parse(transformItems2.InnerText); }
                                if (transformItems2.Name == "PositionZ") { thisdoor.PositionZd = float.Parse(transformItems2.InnerText); }
                                if (transformItems2.Name == "ScaleX") { thisdoor.ScaleXd = float.Parse(transformItems2.InnerText); }
                                if (transformItems2.Name == "ScaleY") { thisdoor.ScaleYd = float.Parse(transformItems2.InnerText); }
                                if (transformItems2.Name == "ScaleZ") { thisdoor.ScaleZd = float.Parse(transformItems2.InnerText); }
                            }
                            ListofDoors.Add(thisdoor);
                        }
                    }
                }

                if (transformItems.Name == "Walls") //walls information will be added into instances of Wall class
                {
                    XmlNodeList transformcontent1 = transformItems.ChildNodes;
                    foreach (XmlNode transformItems1 in transformcontent1)
                    {
                        if (transformItems1.Name == "Wall")
                        {
                            XmlNodeList transformcontent2 = transformItems1.ChildNodes;
                            Wall thiswall = new Wall();
                            foreach (XmlNode transformItems2 in transformcontent2)
                            {
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
    }

    //the getDoors function initiates the ReadDoorsXML function and returns a list of doors and a list of walls with info obtained from the XML file.
    //the two lists are stored in an object array for the return statement, and are typecasted in the Start function
    public static object[] getDoors()
    {
        ReadDoorsXML();
        object[] wallsdoorslist = { ListofDoors, ListofWalls };
        return wallsdoorslist;
    }
    

}





