using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Testing : MonoBehaviour
{
    public float turnAmount = 10;
    public float turnSpeed = 0.4f;
    private float timeCount = 0.0f;
    [SerializeField] Sprite PurpleSelected;
    [SerializeField] Sprite YellowSelected;
    [SerializeField] Sprite PurpleIdle;
    [SerializeField] Sprite YellowIdle;
    [SerializeField] TextMeshProUGUI instruction;
    private Image PurpleUI;
    private Image YellowUI;
    [SerializeField] float vertical = 0;
    [SerializeField] float horizontal = 0;

    private GameObject playerBall;

    [Header("Testing Session")]
    public bool finishTestingLeft = false;
    public bool finishTestingRight = false;
    public bool isLeft;
    public bool isRight;

    public bool inTesting = false;
    public int sessionCountLeft = 0;
    public int sessionCountRight = 0;
    public int maxSessionNum = 5;

    [SerializeField] string instructionBreak = "NOW -- Please follow the following instruction";
    [SerializeField] string instructionLeft = "Imagine pressing the LEFT side of the plane using your LEFT hand.";
    [SerializeField] string instructionRight = "Imagine pressing the RIGHT side of the plane using your RIGHT hand.";

    void Start()
    {
        PurpleUI = GameObject.Find("PurpleSide").GetComponent<Image>();
        YellowUI = GameObject.Find("YellowSide").GetComponent<Image>();
        playerBall = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!finishTestingRight)
        {
            if (!finishTestingLeft)
            {//test left
                if (!inTesting && sessionCountLeft < maxSessionNum)
                {
                    inTesting = true;
                    StartCoroutine(startLeft());

                }
                else if (sessionCountLeft >= maxSessionNum)
                    finishTestingLeft = true;
            }
            else
            { //test right
                if (!inTesting && sessionCountRight < maxSessionNum)
                {
                    inTesting = true;
                    StartCoroutine(startRight());

                }
                else if (sessionCountRight >= maxSessionNum)
                    finishTestingRight = true;
            }
        }
        else
        {
            instruction.text = "Testing is done. Your are good to go.";
        }


        if (isLeft)
        {
            horizontal = -0.4f;
        }
        else if (isRight)
            horizontal = 0.4f;
        else
        {
            horizontal = 0;
            playerBall.transform.position = new Vector3(2, -2f, 0);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, horizontal * turnAmount * -1), timeCount * turnSpeed);

        timeCount = timeCount + Time.deltaTime;


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

    IEnumerator startLeft()
    {
        instruction.text = instructionBreak;
        yield return new WaitForSecondsRealtime(2f);
        instruction.text = instructionLeft;
        isLeft = true;
        isRight = false;
        
        yield return new WaitForSecondsRealtime(5f);
        isLeft = false;
        sessionCountLeft += 1;
        inTesting = false;
    }

    IEnumerator startRight()
    {
        instruction.text = instructionBreak;
        yield return new WaitForSecondsRealtime(2f);
        instruction.text = instructionRight;
        isLeft = false;
        isRight = true;
        
        yield return new WaitForSecondsRealtime(5f);
        isRight = false;
        sessionCountRight += 1;
        inTesting = false;
    }
}
