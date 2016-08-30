using UnityEngine;
using System.Collections;


// Code to instantiate four walls with four switches on them ~*~

// No XML file reading. Float values for wall transforms were obtained directed from Unity.
// Note that the switch instantiation parts are now obsolete
// THERE IS A RUNTIME ERROR: the thing you want to instantiate is null
// The switch prefabs which are being called have been deleted from Resources in Assets in Unity project tab.


public class SmartCondo : MonoBehaviour {

	void Start () {

        //loading prefab which will be used for instantiation of walls
        GameObject mycube = Resources.Load("Wallprefab") as GameObject;

        //wall instantiation: prefab is instantiated, name given, and the desired transform position and scale are changed manually.
        GameObject Bedroomwall = Instantiate(mycube);
        Bedroomwall.transform.position = new Vector3(-0.9638305f, 1.445f, 0.6506863f);
        Bedroomwall.transform.localScale = new Vector3(0.1f, 2.573039f, 6.29f);
        Bedroomwall.name = "Bedroom Wall";

        //switch instantiation: prefab loaded, instantiated and transform position changed manually.
        //Name is given, and the switch is parented to its wall.
        GameObject myswitch1 = Resources.Load("BedroomSwitch") as GameObject;
        GameObject switch1 = Instantiate(myswitch1);
        switch1.transform.parent = Bedroomwall.transform;
        switch1.transform.position = new Vector3(-0.9018305f, 1.445f, 0.6506863f);
        switch1.name = "Bedroom Switch";

        //same process repeated for rest of the walls and switches.
        GameObject Diningwall = Instantiate(mycube);
        Diningwall.transform.position = new Vector3(4.38617f, 1.445f, 3.845686f);
        Diningwall.transform.localScale = new Vector3(0.1f, 2.573039f, 10.6f);
        Diningwall.transform.Rotate(0f, 90f, 0f);
        Diningwall.name = "Dining Wall";

        GameObject myswitch2 = Resources.Load("DiningSwitch") as GameObject;
        GameObject switch2 = Instantiate(myswitch2);
        switch2.transform.parent = Diningwall.transform;
        switch2.transform.position = new Vector3(4.38617f, 1.445f, 3.789f);
        switch2.name = "Dining Switch";

        GameObject Bathroomwall = Instantiate(mycube);
        Bathroomwall.transform.position = new Vector3(4.38617f, 1.445f, -2.544314f);
        Bathroomwall.transform.localScale = new Vector3(0.1f, 2.573039f, 10.6f);
        Bathroomwall.transform.Rotate(0f, 90f, 0f); 
        Bathroomwall.name = "Bathroom Wall";

        GameObject myswitch3 = Resources.Load("BathroomSwitch") as GameObject;
        GameObject switch3 = Instantiate(myswitch3);
        switch3.transform.parent = Bathroomwall.transform;
        switch3.transform.position = new Vector3(4.38617f, 1.445f, -2.486f);
        switch3.name = "Bathroom Switch";

        GameObject Kitchenwall = Instantiate(mycube);
        Kitchenwall.transform.position = new Vector3(9.73617f, 1.445f, 0.6506863f);
        Kitchenwall.transform.localScale = new Vector3(0.1f, 2.573039f, 6.29f);
        Kitchenwall.name = "Kitchen Wall";

        GameObject myswitch4 = Resources.Load("KitchenSwitch") as GameObject;
        GameObject switch4 = Instantiate(myswitch4);
        switch4.transform.parent = Kitchenwall.transform;
        switch4.transform.position = new Vector3(9.664f, 1.445f, 0.6506863f);
        switch4.name = "Kitchen Switch";
    }

}
