using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPlaneControl : MonoBehaviour
{
    public float turnAmount = 10;
    public float turnSpeed = 0.4f;
    public float timeCount = 0.0f;

    public GameObject playerBall;
    
    // Set the icon for the UI
    private Sprite Icon_LeftSelected;
    private Sprite Icon_RightSelected;
    private Sprite Icon_LeftIdle;
    private Sprite Icon_RightIdle;
    private Image LeftUI;
    private Image RightUI;
    public float vertical = 0;
    public float horizontal = 0;

    void Start()
    {
        LeftUI = GameObject.Find("LeftSide").GetComponent<Image>();
        RightUI = GameObject.Find("RightSide").GetComponent<Image>();
        playerBall = GameObject.FindGameObjectWithTag("Player");

        Icon_LeftIdle = Resources.Load<Sprite>("Sprites/Left_Idle");
        Icon_LeftSelected = Resources.Load<Sprite>("Sprites/Left_Selected");
        Icon_RightIdle = Resources.Load<Sprite>("Sprites/Right_Idle");
        Icon_RightSelected = Resources.Load<Sprite>("Sprites/Right_Selected");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        rotatePlane();

        changeHandUI();
    }

    public virtual void rotatePlane(){
        horizontal = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, horizontal * turnAmount * -1), timeCount * turnSpeed);

        timeCount = timeCount + Time.deltaTime;
    }

    void changeHandUI(){
        if (horizontal > 0)
        {
            RightUI.sprite = Icon_RightSelected;
            LeftUI.sprite = Icon_LeftIdle;
        }
        else if (horizontal < 0)
        {
            RightUI.sprite = Icon_RightIdle;
            LeftUI.sprite = Icon_LeftSelected;
        }
        else
        {
            RightUI.sprite = Icon_RightIdle;
            LeftUI.sprite = Icon_LeftIdle;
        }
    }
}
