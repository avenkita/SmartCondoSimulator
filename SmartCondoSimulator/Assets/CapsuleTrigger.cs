using UnityEngine;
using System.Collections;


//This code is attached as a component to each instantiated sensor in ReadSensor.cs
//When the capsule object (rigid body) enters the sensor, the sensor will flash (alternate between translucent and yellow)
//When the capsule object (rigid body) leaves the sensor, the sensor will become translucent again

public class CapsuleTrigger : MonoBehaviour {

    public Color colorvar = Color.white; //initiating a white material color


    void Start()
    {
        colorvar.a = 0.1f; //the float value makes it translucent
    }

    void OnTriggerEnter(Collider var)
    {
        Debug.Log(gameObject.name + " is triggered"); //console message
        StartCoroutine("Blink"); //starting the coroutine Blink() (IEnumerator)
    }

    void OnTriggerExit()
    {
        Debug.Log(gameObject.name + " no longer triggered"); //console message
        StopCoroutine("Blink"); //coroutine Blink() (IEnumerator) is stopped
        gameObject.GetComponent<Renderer>().material.color = colorvar; //sensor is translucent again
    }


    IEnumerator Blink() //IEnumerator because the yield command is called within it.
    {
        while (true) //ensures that the sequence of color change (which simulates the flashing) will keep going until the coroutine is stopped.
        {
            //the cones change from yellow to translucent at 0.2 second intervals
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            yield return new WaitForSeconds(0.2f);
            gameObject.GetComponent<Renderer>().material.color = colorvar;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
