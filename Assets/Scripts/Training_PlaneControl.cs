using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LSL;
using System.Linq;
using static Utils;

public class Training_PlaneControl : PlayerPlaneControl
/// <summary>
/// This script is used to control the training procedure in the training session.
/// It inherits from PlayerPlaneControl.cs
/// </summary>
{

    [HideInInspector]
    [SerializeField] TextMeshProUGUI instruction;
    private Transform spawnPoint;
    private Vector3 spawnPos;

    [Header("[DEBUG]")]
    public bool isLeft;
    public bool isRight;
    public bool inTesting = false;
    public bool isTakingBreak = false;
    public int sessionCountLeft = 0;
    public int sessionCountRight = 0;

    [Header("Parameters (Max Session Number means for each hand)")]
    public int maxSessionNum = 5;
    public float breakTime = 3f;

    private LSLOutlet networkController;
    private StreamOutlet outlet;
    private bool hasSentStartMarker = false;
    private bool hasSentEndMarker = false;
    private bool hasSentLeftStartMarker = false;
    private bool hasSendLeftEndMarker = false;
    private bool hasSentRightStartMarker = false;
    private bool hasSendRightEndMarker = false;

    private string instructionBreak = "NOW -- Please follow the instruction";
    private string instructionLeft = "Imagine pressing the LEFT side of the plane using your LEFT hand.";
    private string instructionRight = "Imagine pressing the RIGHT side of the plane using your RIGHT hand.";

    [Tooltip("This is ahead of the current trial.")]
    public TrialType trial = TrialType.Start;

    private List<TrialType> trialSequence;
    private int currentTrialIndex = 0;


    public override void Start()
    {
        base.Start();
        var canvas = GameObject.Find("Canvas");
        instruction = canvas.transform.Find("Instruction").GetComponent<TextMeshProUGUI>();
        networkController = GameObject.Find("NetworkController").GetComponent<LSLOutlet>();
        outlet = networkController.Outlet;
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        spawnPos = spawnPoint.position;

        // Determine the trial sequence
        trialSequence = new List<TrialType>();
        for (int i = 0; i < maxSessionNum; i++) {
            trialSequence.Add(TrialType.LeftTrial);
            trialSequence.Add(TrialType.RightTrial);
        }

        // Shuffle the trial sequence
        int seed = 12345; 
        UnityEngine.Random.InitState(seed);
        trialSequence = trialSequence.OrderBy(a => UnityEngine.Random.value).ToList();
        trialSequence.Insert(0, TrialType.Start);

        // foreach (var trial in trialSequence) {
        //     Debug.Log(trial);
        // }

        trial = TrialType.Start;
    }

    

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        train();
    }

    public override void planeControl(){
        //override the planeControl() in PlayerPlaneControl.cs
    }

    void train(){
        planeRotation();

        if (currentTrialIndex < trialSequence.Count) {
            trial = trialSequence[currentTrialIndex];
        } else {
            trial = TrialType.End;
        }

        if (!inTesting) {

            switch (trial){
                case TrialType.Start:
                    instruction.text = "Training is starting.";
                    
                    inTesting = true;
                    StartCoroutine(startTraining());
                    currentTrialIndex = 1;
                    
                    break;

                case TrialType.LeftTrial:
                    inTesting = true;
                    StartCoroutine(startLeft());
                    currentTrialIndex++;
                    
                    break;

                case TrialType.RightTrial:
                    inTesting = true;
                    StartCoroutine(startRight());
                    currentTrialIndex++;
                    
                    break;

                case TrialType.End:
                    inTesting = true;
                    isLeft = false;
                    isRight = false;

                    if (!hasSentEndMarker) {
                        hasSentEndMarker = true;
                        outlet.push_sample(new float[] { -(float)Utils.EventMarker_BallGame.TrainStart });
                    }
                    instruction.text = "Training is done. Please wait.";
                    inTesting = false;

                    break;
                }

        }
    }
    void planeRotation(){
        
        if (isLeft)
            horizontal_default = -0.4f;
        else if (isRight)
            horizontal_default = 0.4f;
        else
        {
            horizontal_default = 0;
            // re-initialize the ball position
            playerBall.transform.position = spawnPos;
            // reset the momentum of the ball
            playerBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, horizontal_default * turnAmount * -1), timeCount * turnSpeed);

        timeCount = timeCount + Time.deltaTime;
    }
    
    IEnumerator startTraining(){
        if (!hasSentStartMarker){
            hasSentStartMarker = true;
            yield return new WaitForSecondsRealtime(breakTime);
            outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.TrainStart });
            inTesting = false;
        }
    }
    
    IEnumerator startLeft()
    {
        instruction.text = instructionBreak;
        yield return new WaitForSecondsRealtime(breakTime);
        instruction.text = instructionLeft;
        isLeft = true;
        isRight = false;

        outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.LeftHandTrialStart });
        yield return new WaitForSecondsRealtime(5f);
        outlet.push_sample(new float[] {- (float)Utils.EventMarker_BallGame.LeftHandTrialStart });

        isLeft = false;
        sessionCountLeft += 1;
        inTesting = false;
    }

    IEnumerator startRight()
    {
        instruction.text = instructionBreak;
        yield return new WaitForSecondsRealtime(breakTime);
        instruction.text = instructionRight;
        isLeft = false;
        isRight = true;
        
        outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.RightHandTrialStart });
        yield return new WaitForSecondsRealtime(5f);
        outlet.push_sample(new float[] { -(float)Utils.EventMarker_BallGame.RightHandTrialStart });

        isRight = false;
        sessionCountRight += 1;
        inTesting = false;
    }

}
