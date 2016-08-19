using UnityEngine;
using System.Collections;

public class camMouseLook : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    GameObject character;

	// Use this for initialization
	void Start ()
    {
        character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing) ;
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing) ;
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);
        mouseLook += smoothV;

        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Debug.DrawLine(ray.origin, Camera.main.transform.forward * 0.01f, Color.red);
    }
}
