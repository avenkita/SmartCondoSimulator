using UnityEngine;
using System.Collections;

//A class which goes along with CompileDoors.cs and ReadDoors.cs
//Can hold information about the transform for a door, and the two walls which surround it

public class DoorClass {

    public string doorid { get; set; }
    public float dooroffset { get; set; }
    public float doorsize { get; set; }

    public float PositionXd { set; get; }
    public float PositionYd { set; get; }
    public float PositionZd { set; get; }
    public float ScaleXd { set; get; }
    public float ScaleYd { set; get; }
    public float ScaleZd { set; get; }
    public float RotateXd { set; get; }
    public float RotateYd { set; get; }
    public float RotateZd { set; get; }


    public float PositionXw1 { set; get; }
    public float PositionYw1 { set; get; }
    public float PositionZw1 { set; get; }
    public float ScaleXw1 { set; get; }
    public float ScaleYw1 { set; get; }
    public float ScaleZw1 { set; get; }
    public float RotateXw1 { set; get; }
    public float RotateYw1 { set; get; }
    public float RotateZw1 { set; get; }


    public float PositionXw2 { set; get; }
    public float PositionYw2 { set; get; }
    public float PositionZw2 { set; get; }
    public float ScaleXw2 { set; get; }
    public float ScaleYw2 { set; get; }
    public float ScaleZw2 { set; get; }
    public float RotateXw2 { set; get; }
    public float RotateYw2 { set; get; }
    public float RotateZw2 { set; get; }

}
