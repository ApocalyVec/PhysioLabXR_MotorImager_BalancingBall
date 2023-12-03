using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LSL;
using static Utils;
using UnityEngine.SceneManagement;

public class PlayerPlaneControl : MonoBehaviour
/// <summary>
/// This script is used to control the plane in the game during the testing session.
/// It is also the parent of the script Training_PlaneControl.cs
/// </summary>
{
    [HideInInspector]public float turnAmount = 10;
    [HideInInspector]public float turnSpeed = 0.4f;
    [HideInInspector]public float timeCount = 0.0f;

    [HideInInspector]public GameObject playerBall;
    
    // Set the icon for the UI
    private Sprite Icon_LeftSelected;
    private Sprite Icon_RightSelected;
    private Sprite Icon_LeftIdle;
    private Sprite Icon_RightIdle;
    private GameObject LeftHand;
    private GameObject RightHand;
    private Image LeftUI;
    private Image RightUI;
    
    [HideInInspector]public float vertical = 0;

    [HideInInspector]public float horizontal_bci = 0;
    [HideInInspector]public float horizontal_default = 0;

    private LSLOutlet lslOutlet;
    private LSLInletInterface lslInlet;
    private StreamOutlet outlet;

    public virtual void Start()
    {
        LeftHand = GameObject.Find("WorldCanvas/LeftSide");
        RightHand = GameObject.Find("WorldCanvas/RightSide");
        LeftUI = LeftHand.GetComponent<Image>();
        RightUI = RightHand.GetComponent<Image>();
        playerBall = GameObject.FindGameObjectWithTag("Player");

        Icon_LeftIdle = Resources.Load<Sprite>("Sprites/Left_Idle");
        Icon_LeftSelected = Resources.Load<Sprite>("Sprites/Left_Selected");
        Icon_RightIdle = Resources.Load<Sprite>("Sprites/Right_Idle");
        Icon_RightSelected = Resources.Load<Sprite>("Sprites/Right_Selected");

        lslOutlet = GameObject.Find("NetworkController").GetComponent<LSLOutlet>();
        outlet = lslOutlet.Outlet;

        lslInlet = GameObject.Find("NetworkController").GetComponent<LSLInletInterface>();
        Debug.Log("lslInlet: " + lslInlet);

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Game")
            outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.EvalStart });
    }

    // Update is called once per frame
    public virtual void Update()
    {
        planeControl();

        changeHandUI();
    }

    public virtual void planeControl(){

        lslInlet.pullSample();
        if (lslInlet.frameTimestamp != 0.0){
            //Debug.Log("frameTimestamp: " + lslInlet.frameTimestamp);
            float prediction_sample = lslInlet.frameDataBuffer[0];
            //Debug.Log("prediction_sample: " + prediction_sample);
            float new_sample = prediction_sample * 2 - 1;

            if (Mathf.Abs(new_sample - horizontal_bci) > 0.1){
                horizontal_bci = new_sample;
            }
        } 

        lslInlet.clearBuffer();

        horizontal_default = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, (horizontal_default + horizontal_bci) * turnAmount * -1), timeCount * turnSpeed);

        timeCount = timeCount + Time.deltaTime;
    }

    void changeHandUI(){
        if (horizontal_default > 0)
        {
            RightUI.sprite = Icon_RightSelected;
            LeftUI.sprite = Icon_LeftIdle;
        }
        else if (horizontal_default < 0)
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
