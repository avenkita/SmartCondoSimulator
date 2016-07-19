using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

//A code to read Alexandr's SmartCondo XML, instantiate the sensors alone, and compile sensor info into a Unity-compatible form

public class CompileSensors : MonoBehaviour {
    public static List<SensorClass> ListofSensors = new List<SensorClass>();

    public static void ReadSensors()
    {
        TextAsset textXML = (TextAsset)Resources.Load("simulationWorldSC", typeof(TextAsset));
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textXML.text);
        XmlNodeList transformList = xml.GetElementsByTagName("simulatedworld");
 

        foreach (XmlNode transformInfo in transformList) //has just the simulated world tag!
        {
            XmlNodeList transformcontent = transformInfo.ChildNodes; //gets the tags inside the simulatedworld tag (rooms)
            foreach (XmlNode transformItems in transformcontent)
            {
                if (transformItems.Name == "actions" || transformItems.Name == "agents" || transformItems.Name == "locations" || transformItems.Name == "rooms" || transformItems.Name == "obstacles") { continue; }
                XmlNodeList transformcontent2 = transformItems.ChildNodes; //gets the tags inside the rooms tag (room)
                foreach (XmlNode transformItems2 in transformcontent2)
                {
                    SensorClass SCsensors = new SensorClass();
                    XmlNodeList transformcontent3 = transformItems2.ChildNodes; //gets the tags inside the room tag (roomid and wall)
                    foreach (XmlNode transformItems3 in transformcontent3)
                    {
                        if (transformItems3.Name == "id") { SCsensors.sensorid = transformItems3.InnerText; }
                        if (transformItems3.Name == "type") { SCsensors.sensortype = transformItems3.InnerText; }
                        if (transformItems3.Name == "radius") { SCsensors.radius = float.Parse(transformItems3.InnerText); }
                        if (transformItems3.Name == "point")
                        {
                            XmlNodeList transformcontent4 = transformItems3.ChildNodes;
                            foreach (XmlNode transformItems4 in transformcontent4)
                            {
                                if (transformItems4.Name == "xcoord") { SCsensors.xcoord = float.Parse(transformItems4.InnerText); }
                                if (transformItems4.Name == "ycoord") { SCsensors.ycoord = float.Parse(transformItems4.InnerText); }
                            }
                        }
                    }
                    ListofSensors.Add(SCsensors);
                }
            }
        }
    }

    static List<SensorClass> getSensors()
    {
        ReadSensors();
        return ListofSensors;
    }

    void Start()
    {
        XmlTextWriter writer = new XmlTextWriter("C:\\Users\\Aishwarya\\Desktop\\SmartCondo\\SmartCondoSimulator\\SmartCondoSimulator\\Assets\\Resources\\sensorlist.xml", Encoding.UTF8);
        writer.WriteStartElement("Sensors");
        List<SensorClass> SensorsList = getSensors();
        for (int sensorcount = 0; sensorcount < SensorsList.Count; sensorcount++)
        {
            var cs = SensorsList[sensorcount];
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
        writer.WriteEndElement();
        writer.Close();
    }
}
