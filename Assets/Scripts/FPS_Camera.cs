using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Camera : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 90f;
    private float rotX;

    // Start is called before the first frame update
    void Start()
    {
        //Hide mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Taking mouse input
        rotX += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;

        rotX = Mathf.Clamp(rotX, -90f, 90f);
        
        transform.localEulerAngles = new Vector3(-rotX, transform.localEulerAngles.y + mouseX, 0);
    }
}
