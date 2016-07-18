using UnityEngine;
using System.Collections;


//Another code to just practice Deb.Log and transform components (based on videos and manuals examples)

public class Printingscript : MonoBehaviour {
    //int a = 5;
    
	// Use this for initialization
	void Start () {
       /* GameObject mycube = Resources.Load("Cube1") as GameObject;
        //Debug.Log(a);
        GameObject bedroomwall = Instantiate(mycube);
        bedroomwall.transform.position = new Vector3(-0.9638305f, 1.445f, 0.6506863f);
        bedroomwall.transform.localScale = new Vector3(0.1f, 2.573039f, 6.29f);
        GameObject nookwall = Instantiate(mycube);
        //     WHY DON'T i HAVE TO INSTANTIATE AS GAMEOBJECT? EVEN IN SENSORS??
        nookwall.transform.position = new Vector3(4.38617f, 1.445f, 3.845686f);
        nookwall.transform.localScale = new Vector3(0.1f, 2.573039f, 10.6f);
        nookwall.transform.Rotate(0f, 90f, 0f);
        GameObject bathroomwall = Instantiate(mycube);
        bathroomwall.transform.position = new Vector3(4.38617f, 1.445f, -2.544314f);
        bathroomwall.transform.localScale = new Vector3(0.1f, 2.573039f, 10.6f);
        bathroomwall.transform.Rotate(0f, 90f, 0f); //in degrees!
        GameObject kitchenwall = Instantiate(mycube);
        kitchenwall.transform.position = new Vector3(9.73617f, 1.445f, 0.6506863f);
        kitchenwall.transform.localScale = new Vector3(0.1f, 2.573039f, 6.29f); */
        //Vector3 forwardvar = bedroomwall.transform.forward;
        //Debug.Log(forwardvar); THIS PRINTS (0,0,1)
        //vector3 is a struct, therefore you need to create it instead of just assigning it
        //id.transform.position = new Vector3(1, 1, -1);
        //QUESTION: what happens if when the game is in "play" mode, you change the position. It doesn't remember your edits for the future. Why?
        //Destroy(id, 3f);
        //Vector3 rot = transform.position;
        //transform.position = new Vector3(0, 0, 0);
        //  Vector3 a = new Vector3(3,2,6);
        //Debug.Log(rot);
    }
    
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(2f * Time.deltaTime, 3f * Time.deltaTime, 1f * Time.deltaTime);
        //id.transform.Rotate(5f, 5f, 5f);
	}
}
