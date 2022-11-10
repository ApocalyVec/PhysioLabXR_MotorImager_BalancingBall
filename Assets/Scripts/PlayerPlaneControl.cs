using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPlaneControl : MonoBehaviour
{
    public float turnAmount = 10;
    public float turnSpeed = 0.4f;
    private float timeCount = 0.0f;
    [SerializeField] Sprite PurpleSelected;
    [SerializeField] Sprite YellowSelected;
    [SerializeField] Sprite PurpleIdle;
    [SerializeField] Sprite YellowIdle;
    private Image PurpleUI;
    private Image YellowUI;
    [SerializeField] float vertical = 0;
    [SerializeField] float horizontal = 0;

    void Start()
    {
        PurpleUI = GameObject.Find("PurpleSide").GetComponent<Image>();
        YellowUI = GameObject.Find("YellowSide").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");


        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, horizontal * turnAmount * -1), timeCount * turnSpeed);

        timeCount = timeCount + Time.deltaTime;

        //change UI image based on inputs
        //if (vertical != 0)
        //    PurpleUI.sprite = PurpleSelected;
        //else
        //    PurpleUI.sprite = PurpleIdle;

        if (horizontal > 0)
        {
            YellowUI.sprite = YellowSelected;
            PurpleUI.sprite = PurpleIdle;
        }
        else if (horizontal < 0)
        {
            YellowUI.sprite = YellowIdle;
            PurpleUI.sprite = PurpleSelected;
        }
        else
        {
            YellowUI.sprite = YellowIdle;
            PurpleUI.sprite = PurpleIdle;
        }
    }
}
