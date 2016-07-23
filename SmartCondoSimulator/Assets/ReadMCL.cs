using UnityEngine;
using System.Collections;
using System.Collections.Generic; //this is where the List<T>() class comes from
using System.Xml;

public class ReadMCL : MonoBehaviour {

    public static List<Wall> ListofWalls = new List<Wall>();


    void Start()
    {
        List<Wall> WallsList = getWalls();

        GameObject mycube = Resources.Load("Wallprefab") as GameObject;
        for (int wallnumber = 0; wallnumber < WallsList.Count; wallnumber++)
        {
            var cwall = WallsList[wallnumber];
            GameObject go = Instantiate(mycube) as GameObject;
            go.transform.position = new Vector3(cwall.PositionX, cwall.PositionY, cwall.PositionZ);
            go.transform.localScale = new Vector3(cwall.ScaleX, cwall.ScaleY, cwall.ScaleZ);
            Debug.Log(cwall.ScaleZ);
            go.transform.Rotate(cwall.RotateX, cwall.RotateY, cwall.RotateZ);
            go.name = cwall.Name;
        }
    }


    public static void ReadWallsXML()
    {
        TextAsset textXML = (TextAsset)Resources.Load("MCLspaceUnity", typeof(TextAsset));
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textXML.text);
        XmlNodeList transformList = xmldoc.GetElementsByTagName("Walls");

        foreach (XmlNode transformInfo in transformList)
        {

            XmlNodeList transformcontent = transformInfo.ChildNodes;
            foreach (XmlNode transformItems in transformcontent)
            {

                XmlNodeList transformcontent2 = transformItems.ChildNodes;
                Wall w = new Wall();
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    if (transformItems2.Name == "Name") { w.Name = transformItems2.InnerText; }
                    if (transformItems2.Name == "PositionX") { w.PositionX = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "PositionY") { w.PositionY = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "PositionZ") { w.PositionZ = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "ScaleX") { w.ScaleX = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "ScaleY") { w.ScaleY = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "ScaleZ") { w.ScaleZ = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "RotateX") { w.RotateX = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "RotateY") { w.RotateY = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "RotateZ") { w.RotateZ = float.Parse(transformItems2.InnerText); }
                }
                ListofWalls.Add(w);
            }
        }
    }


    public static List<Wall> getWalls()
    {
        ReadWallsXML();
        return ListofWalls;
    }
}
