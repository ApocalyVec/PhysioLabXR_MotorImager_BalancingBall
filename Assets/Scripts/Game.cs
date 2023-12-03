using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Game : MonoBehaviour
{
    
    public int scoreCount = 0;
    public float friction;
    public float vertical;
    public float horizontal;

    private Rigidbody rb;
    private int lifeMax = 3;
    private int lifeCount = 3;
    private bool isGrounded;
    private Vector3 spawnPos;
    private GameObject otherObj;
    [HideInInspector]public bool gameStop;

    [SerializeField] GameObject LifeUI;
    [SerializeField] GameObject FinishedScreen;
    [SerializeField] GameObject FadeScreen;
    private CanvasGroup FinishedScreenCG;
    private TextMeshProUGUI ScoreNum;
    public Transform spawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPos = spawnPoint.position;

        FinishedScreenCG = FinishedScreen.GetComponent<CanvasGroup>();
        ScoreNum = FinishedScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        FinishedScreenCG.alpha = 0;
    }

    void Update()
    {
        if (gameStop)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                FinishedScreenCG.alpha = 0;
                gameStop = false;
                Time.timeScale = 1;
            }

        }

        if (transform.position.y < -10)
        {
            Fader(); //hit screen effect
            transform.position = spawnPos;
            // reset the momentum of the ball
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (lifeCount < 1)
            {
                lifeCount = 3;
                ScoreNum.text = scoreCount.ToString();
                FinishedScreenCG.alpha = 1;
                gameStop = true;
                Time.timeScale = 0;
                scoreCount = 0;
            }

            else
            {
                lifeCount--;
                LifeUI.GetComponentInChildren<Image>().fillAmount = (float) lifeCount / lifeMax;
            }
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
 
    public void Fader()
    {
        FadeScreen.GetComponent<CanvasGroup>().alpha = 1;
        FadeScreen.GetComponent<CanvasGroup>().DOFade(0, 1).SetUpdate(true);
    }

}
