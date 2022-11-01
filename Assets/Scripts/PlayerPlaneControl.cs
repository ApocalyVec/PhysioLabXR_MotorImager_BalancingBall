using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneControl : MonoBehaviour
{
    public float turnSpeed = 10;
    public float vertical;
    public float horizontal;
    public bool isGrounded;
    public GameObject obj;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Euler(vertical * turnSpeed, 1, horizontal * turnSpeed);


    }
}
