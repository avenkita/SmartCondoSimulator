using UnityEngine;
using System.Collections;

public class cubemove : MonoBehaviour {

    public float speed = 0.00001f;
   
    void Start()
    {

    }


    void Update()
    {
        transform.Translate(0, speed*Time.deltaTime, 0);
    }
}

