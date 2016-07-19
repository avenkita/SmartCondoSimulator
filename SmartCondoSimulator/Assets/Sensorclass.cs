using UnityEngine;
using System.Collections;


//A class that goes along with CompileSensors.cs


public class SensorClass
{
    public float xcoord { get; set; }
    public float ycoord { get; set; }
    public string sensorid { get; set; } //equivalent to Name for the walls?
    public string sensortype { get; set; }
    public float radius { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public float ScaleZ { get; set; }

}
