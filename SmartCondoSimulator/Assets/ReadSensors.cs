using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

//A code which can read the XML format which has Unity compatible values and instantiate the sensor (sensorlist.xml)


public class ReadSensors : MonoBehaviour
{
    //initializes a list of SensorClass type
    public static List<SensorClass> ListofSensors = new List<SensorClass>();

    void Start()
        //gets the list of sensors, loads a prefab and calls function to instantiate sensors
    {
        List<SensorClass> SensorList = getSensors();
        GameObject mysensor = Resources.Load("Sphere") as GameObject;
        InstantiateSensors(mysensor, SensorList);
    }   
   
    //function InstantiateSensors has arguments of the prefab and the list of sensors, and instantiates the sensors in the list
    public static void InstantiateSensors(GameObject sensorprefab, List<SensorClass> SensorList)
    {
        GameObject emptys = new GameObject("Sensors"); //parenting
        Material newMat = Resources.Load("thismat", typeof(Material)) as Material; //loading a material which will be added to the prefabs so that their colors can be easily changed.

        for (int sensorcount = 0; sensorcount < SensorList.Count; sensorcount++)
        {
            var csensor = SensorList[sensorcount];
            GameObject go = Instantiate(sensorprefab) as GameObject;
            go.transform.parent = emptys.transform; //all sensors parented under "Sensors" GameObject
            go.transform.position = new Vector3(csensor.PositionX, csensor.PositionY, csensor.PositionZ);
            go.transform.localScale = new Vector3(csensor.ScaleX, csensor.ScaleY, csensor.ScaleZ);
            go.name = csensor.sensorid;
            go.tag = "sensor"; //tag added for future use if need be
            go.AddComponent<CapsuleTrigger>(); //adding CapsuleTrigger.cs as a component
            go.GetComponent<Collider>().isTrigger = true; //enabling "Is Trigger" within the collider section
            go.GetComponent<Renderer>().material = newMat; //assigning the material  
            go.GetComponent<Renderer>().material.color = Color.red; 
        }
    }

    public static void ReadSensorsXML()
    {
        //XML file is loaded and the XmlDocument class is used.
        TextAsset textXML = (TextAsset)Resources.Load("sensorlist", typeof(TextAsset));
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textXML.text);
        XmlNodeList transformList = xmldoc.GetElementsByTagName("Sensors"); //get all the innertext

        foreach (XmlNode transformInfo in transformList)
        {
            XmlNodeList transformcontent = transformInfo.ChildNodes; //gets each "Sensor" tag's innertext
            foreach (XmlNode transformItems in transformcontent)
            {
                XmlNodeList transformcontent2 = transformItems.ChildNodes; //gets tags within "Sensor"
                SensorClass thissensor = new SensorClass(); //creating an instance of the Sensorclass class which will store the necessary values.
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    //appending values from the XML into the fields of the class
                    if (transformItems2.Name == "Name") { thissensor.sensorid = transformItems2.InnerText; }
                    if (transformItems2.Name == "PositionX") { thissensor.PositionX = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "PositionY") { thissensor.PositionY = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "PositionZ") { thissensor.PositionZ = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "ScaleX") { thissensor.ScaleX = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "ScaleY") { thissensor.ScaleY = float.Parse(transformItems2.InnerText); }
                    if (transformItems2.Name == "ScaleZ") { thissensor.ScaleZ = float.Parse(transformItems2.InnerText); }
                }
                ListofSensors.Add(thissensor); //the instance of the Sensorclass class with all information is appended into the list.
            }
        }
    }

    //the getSensors function initiates the ReadSensorsXML function and returns the list of sensors with info obtained from the XML file.
    public static List<SensorClass> getSensors()
    {
        ReadSensorsXML();
        return ListofSensors;
    }

}



