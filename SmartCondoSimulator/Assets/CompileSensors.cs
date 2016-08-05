using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

//A code to read Alexandr's SmartCondo XML, get information about the sensors alone, and compile sensor info into a Unity-compatible form

public class CompileSensors : MonoBehaviour {

    //initializes a list of SensorClass type
    public static List<SensorClass> ListofSensors = new List<SensorClass>();

    public static void ReadSensors()
    {
        //XML file is loaded and the XmlDocument class is used.
        TextAsset textXML = (TextAsset)Resources.Load("simulationWorldSC", typeof(TextAsset));
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textXML.text);
        XmlNodeList transformList = xml.GetElementsByTagName("simulatedworld");
 

        foreach (XmlNode transformInfo in transformList) //has just the simulated world tag
        {
            XmlNodeList transformcontent = transformInfo.ChildNodes; //gets the tags inside the simulatedworld tag
            foreach (XmlNode transformItems in transformcontent)
            {
                //only the "sensors" tag will be found
                if (transformItems.Name == "actions" || transformItems.Name == "agents" || transformItems.Name == "locations" || transformItems.Name == "rooms" || transformItems.Name == "obstacles") { continue; }
                XmlNodeList transformcontent2 = transformItems.ChildNodes; //gets the tags inside the sensors tag (sensor)
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    SensorClass SCsensors = new SensorClass(); //creates a new instance of SensorClass class
                    XmlNodeList transformcontent3 = transformItems2.ChildNodes; //gets the tags inside the sensor tag
                    foreach (XmlNode transformItems3 in transformcontent3)
                    {
                        //append all info to the class
                        if (transformItems3.Name == "id") { SCsensors.sensorid = transformItems3.InnerText; }
                        if (transformItems3.Name == "type") { SCsensors.sensortype = transformItems3.InnerText; }
                        if (transformItems3.Name == "radius") { SCsensors.radius = float.Parse(transformItems3.InnerText); }
                        if (transformItems3.Name == "point")
                        {
                            XmlNodeList transformcontent4 = transformItems3.ChildNodes; //gets the tags inside the point tag (xcoord and ycoord)
                            foreach (XmlNode transformItems4 in transformcontent4)
                            {
                                if (transformItems4.Name == "xcoord") { SCsensors.xcoord = float.Parse(transformItems4.InnerText); }
                                if (transformItems4.Name == "ycoord") { SCsensors.ycoord = float.Parse(transformItems4.InnerText); }
                            }
                        }
                    }
                    ListofSensors.Add(SCsensors); //the instance of the SensorClass class with all information about the sensor appended into the list.
                }
            }
        }
    }

    //the getSensors function initiates the ReadSensors function and returns the list of sensors
    static List<SensorClass> getSensors()
    {
        ReadSensors();
        return ListofSensors;
    }

    void Start()
    {
        //instance of XmlTextWriter is created. File will load into Assets\Resources
        XmlTextWriter writer = new XmlTextWriter("C:\\Users\\Aishwarya\\Desktop\\SmartCondo\\SmartCondoSimulator\\SmartCondoSimulator\\Assets\\Resources\\sensorlist.xml", Encoding.UTF8);
        writer.WriteStartElement("Sensors");

        List<SensorClass> SensorsList = getSensors(); //SensorsList is returned by the getSensors function

        for (int sensorcount = 0; sensorcount < SensorsList.Count; sensorcount++) //loops through all sensors in sensorslist
        {
            var cs = SensorsList[sensorcount];
            //Writing into the XML
            writer.Formatting = Formatting.Indented;
            writer.WriteStartElement("Sensor");
            writer.WriteStartElement("Name"); writer.WriteString(cs.sensorid); writer.WriteEndElement();
            writer.WriteStartElement("Type"); writer.WriteString(cs.sensortype); writer.WriteEndElement();
            writer.WriteStartElement("PositionX"); writer.WriteString(cs.ycoord.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("PositionY"); writer.WriteString("0"); writer.WriteEndElement();
            writer.WriteStartElement("PositionZ"); writer.WriteString(cs.xcoord.ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleX"); writer.WriteString(((cs.radius)*2).ToString()); writer.WriteEndElement();
            writer.WriteStartElement("ScaleY"); writer.WriteString("20"); writer.WriteEndElement();
            writer.WriteStartElement("ScaleZ"); writer.WriteString(((cs.radius)*2).ToString()); writer.WriteEndElement();
            writer.WriteEndElement();
        }
        writer.WriteEndElement(); //end "Sensors"
        writer.Close(); //close Writer just in case there's a reading component.
    }
}
