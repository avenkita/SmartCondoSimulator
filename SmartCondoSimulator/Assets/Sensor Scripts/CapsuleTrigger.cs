using UnityEngine;
using System.Collections;

public class CapsuleTrigger : MonoBehaviour {

	void Start () {
       
    }
	
	void Update () {

	}

    void OnTriggerEnter(Collider var)
    { 
        Debug.Log(gameObject.name + " is triggered");
        gameObject.GetComponent<Renderer>().material.color = Color.cyan;
    }

    void OnTriggerExit()
    {
        Debug.Log(gameObject.name + " no longer triggered");
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

}
