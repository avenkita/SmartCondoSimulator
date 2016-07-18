using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

//A code which can read the XML format which has Unity compatible values and instantiate the sensor (sensorlist.xml)


public class ReadSensors : MonoBehaviour {
        public static List<Sensorclass> ListofSensors = new List<Sensorclass>();

        void Start()
        {
            List<Sensorclass> SensorList = getSensors();

            GameObject mysensor = Resources.Load("Spheresensor") as GameObject;
            for (int sensorcount = 0; sensorcount < SensorList.Count; sensorcount++)
            {
                var csensor = SensorList[sensorcount];
                GameObject go = Instantiate(mysensor) as GameObject;
                go.transform.position = new Vector3(csensor.PositionX, csensor.PositionY, csensor.PositionZ);
                go.transform.localScale = new Vector3(csensor.ScaleX, csensor.ScaleY, csensor.ScaleZ);
                // Debug.Log(csensor.RotateY);
              //  go.transform.Rotate(csensor.RotateX, csensor.RotateY, csensor.RotateZ);
                go.name = csensor.sensorid;
            }
        }


        public static void ReadWallsXML()
        {
            TextAsset textXML = (TextAsset)Resources.Load("sensorlist", typeof(TextAsset));
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textXML.text);
            XmlNodeList transformList = xmldoc.GetElementsByTagName("Sensors");

            foreach (XmlNode transformInfo in transformList)
            {

                XmlNodeList transformcontent = transformInfo.ChildNodes;
                foreach (XmlNode transformItems in transformcontent)
                {

                    XmlNodeList transformcontent2 = transformItems.ChildNodes;
                    Sensorclass thissensor = new Sensorclass();
                    foreach (XmlNode transformItems2 in transformcontent2)
                    {
                        Debug.Log(transformItems2.InnerText);
                        if (transformItems2.Name == "Name") { thissensor.sensorid = transformItems2.InnerText; }
                        if (transformItems2.Name == "PositionX") { thissensor.PositionX = float.Parse(transformItems2.InnerText); }
                        if (transformItems2.Name == "PositionY") { thissensor.PositionY = float.Parse(transformItems2.InnerText); }
                        if (transformItems2.Name == "PositionZ") { thissensor.PositionZ = float.Parse(transformItems2.InnerText); }
                        if (transformItems2.Name == "ScaleX") { thissensor.ScaleX = float.Parse(transformItems2.InnerText); }
                        if (transformItems2.Name == "ScaleY") { thissensor.ScaleY = float.Parse(transformItems2.InnerText); }
                        if (transformItems2.Name == "ScaleZ") { thissensor.ScaleZ = float.Parse(transformItems2.InnerText); }
                    }
                    ListofSensors.Add(thissensor);
                }
            }
        }


        public static List<Sensorclass> getSensors()
        {
            ReadWallsXML();
            return ListofSensors;
        }

        void Update() { }
    }



