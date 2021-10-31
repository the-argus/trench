using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class player_movement : MonoBehaviour
{
    [SerializeField] float moveStrength = 10;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // float values between -1 and 1
        float zin = Input.GetAxis("Vertical");
        float xin = Input.GetAxis("Horizontal");

        Vector3 input = Vector3.Normalize(new Vector3(xin, 0, zin));

        float rot = transform.localEulerAngles.y;

        input = Quaternion.Euler(0, rot, 0) * input;
        input *= moveStrength * Time.deltaTime;

        rb.AddForce(input);
    }
}
