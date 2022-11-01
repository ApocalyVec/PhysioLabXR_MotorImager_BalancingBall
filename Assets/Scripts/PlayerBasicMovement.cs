using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBasicMovement : MonoBehaviour
{
    private Rigidbody rb;
    public int lifeCount = 3;
    public float friction;
    public float vertical;
    public float horizontal;
    public bool isGrounded;
    public Vector3 spawnPos;
    public GameObject otherObj;

    [SerializeField] TextMeshProUGUI LifeUI;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPos = new Vector3(0, 3, 0);
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = spawnPos;
            lifeCount--;
            LifeUI.text = lifeCount.ToString();
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(-friction * rb.velocity.normalized);
        rb.AddTorque(-friction * rb.angularVelocity.normalized);
    }

    void OnCollisionEnter(Collision collision)
    {
        otherObj = collision.gameObject;
        if (otherObj.tag == ("Ground"))
        {
            isGrounded = true;
        }
    }
    // This function is a callback for when the collider is no longer in contact with a previously collided object.
    void OnCollisionExit(Collision collision)
    {
        otherObj = collision.gameObject;
        if (collision.gameObject.tag == ("Ground"))
        {
            isGrounded = false;
        }
    }
}
