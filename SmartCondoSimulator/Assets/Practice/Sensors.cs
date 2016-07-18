using UnityEngine;
using System.Collections;


//code which Victor initially gave me to understand how instantiation and prefabs work


public class Sensors : MonoBehaviour {

	// Use this for initialization
	void Start () {
          GameObject prefab = Resources.Load("LightSwitch") as GameObject;
          for (int i=0; i < 100; i++)
          {
          GameObject go = Instantiate(prefab);
          go.transform.position = new Vector3(i-10, 2, -1);
         }
    }
    //GameObject myObject = Resources.Load("Cube1") as GameObject;
    //myObject.Instantiate;
    //GameObject cube = Instantiate(myObject);


    // Update is called once per frame
    void Update () {
	
	}
}
