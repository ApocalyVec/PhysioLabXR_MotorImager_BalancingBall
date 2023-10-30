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
    
    [Header("Testing Session [DEBUG]")]
    // public bool finishTestingLeft = false;
    // public bool finishTestingRight = false;
    public bool isLeft;
    public bool isRight;
    public bool inTesting = false;
    public bool isTakingBreak = false;
    public int sessionCountLeft = 0;
    public int sessionCountRight = 0;

    [Header("Testing Parameters")]
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

    [SerializeField] string instructionBreak = "NOW -- Please follow the instruction";
    [SerializeField] string instructionLeft = "Imagine pressing the LEFT side of the plane using your LEFT hand.";
    [SerializeField] string instructionRight = "Imagine pressing the RIGHT side of the plane using your RIGHT hand.";

    [Tooltip("This is ahead of the current trial.")]
    public TrialType trial = TrialType.Start;

    private List<TrialType> trialSequence;
    private int currentTrialIndex = -1;


    public override void Start()
    {
        base.Start();
        var canvas = GameObject.Find("Canvas");
        instruction = canvas.transform.Find("Instruction").GetComponent<TextMeshProUGUI>();
        networkController = GameObject.Find("NetworkController").GetComponent<LSLOutlet>();
        outlet = networkController.Outlet;


        // Determine the trial sequence
        trialSequence = new List<TrialType>();

        for (int i = 0; i < maxSessionNum; i++) {
            trialSequence.Add(TrialType.LeftTrial);
            trialSequence.Add(TrialType.RightTrial);
        }

        // Shuffle the trial sequence
        trialSequence = trialSequence.OrderBy(a => UnityEngine.Random.value).ToList();

        foreach (var trial in trialSequence) {
            Debug.Log(trial);
        }

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

        if (currentTrialIndex == -1) {
            trial = TrialType.Start;
        }
        else {
            if (currentTrialIndex < trialSequence.Count) {
                trial = trialSequence[currentTrialIndex];
            } else {
                trial = TrialType.End;
            }
        }

        switch (trial){
            case TrialType.Start:
                Debug.Log("sequence: " + currentTrialIndex);
                instruction.text = "Training is starting.";
                if (!hasSentStartMarker){
                    hasSentStartMarker = true;
                    outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.TrainStart });
                    currentTrialIndex++;
                }
                break;

            case TrialType.LeftTrial:
                Debug.Log("sequence: " + currentTrialIndex);
                // if (!hasSentLeftStartMarker) {
                //     hasSentLeftStartMarker = true;
                //     outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.LeftBlockStart });
                // }                
                if (!inTesting) {
                    inTesting = true;
                    StartCoroutine(startLeft());
                    currentTrialIndex++;
                } 
                break;

            case TrialType.RightTrial:
                Debug.Log("sequence: " + currentTrialIndex);
                // if (!hasSentRightStartMarker) {
                //     hasSentRightStartMarker = true;
                //     outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.RightBlockStart });
                // }

                if (!inTesting) {
                    inTesting = true;
                    StartCoroutine(startRight());
                    currentTrialIndex++;
                }
                break;

            case TrialType.End:
                isLeft = false;
                isRight = false;

                if (!hasSentEndMarker) {
                    hasSentEndMarker = true;
                    outlet.push_sample(new float[] { -(float)Utils.EventMarker_BallGame.TrainStart });
                }
                instruction.text = "Training is done. Please wait.";
                break;
            }


        /*
        switch (trial)
        {
            case TrialType.Start:
                instruction.text = "Training is starting.";
                if (!hasSentStartMarker){
                    hasSentStartMarker = true;
                    outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.TrainStart });
                    trial = TrialType.LeftTrial;
                }
                break;
            case TrialType.LeftTrial:
                if (!hasSentLeftStartMarker) {
                    hasSentLeftStartMarker = true;
                    outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.LeftBlockStart });
                }
                if (!inTesting && sessionCountLeft < maxSessionNum){
                    inTesting = true;
                    StartCoroutine(startLeft());
                } 

                // finish if max session number is reached
                if (!inTesting && sessionCountLeft >= maxSessionNum) {

                    trial = TrialType.LeftFinish;
                }
                break;


            case TrialType.RightTrial:
                if (!hasSentRightStartMarker) {
                    hasSentRightStartMarker = true;
                    outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.RightBlockStart });
                }
                if (!inTesting && sessionCountRight < maxSessionNum){
                    inTesting = true;
                    StartCoroutine(startRight());
                }
                
                // finish if max session number is reached
                if (!inTesting && sessionCountRight >= maxSessionNum) {
                    trial = TrialType.RightFinish;
                }

                break;


            case TrialType.LeftFinish:
                if (!hasSendLeftEndMarker){
                    hasSendLeftEndMarker = true;
                    outlet.push_sample(new float[] { -(float)Utils.EventMarker_BallGame.LeftBlockStart });
                }
                finishTestingLeft = true;
                instruction.text = instructionBreak;
                StartCoroutine(takeBreak());
                break;


            case TrialType.RightFinish:
                if (!hasSendRightEndMarker) {
                        hasSendRightEndMarker = true;
                        outlet.push_sample(new float[] { -(float)Utils.EventMarker_BallGame.RightBlockStart });
                    }
                finishTestingRight = true;
                instruction.text = instructionBreak;
                StartCoroutine(takeBreak());
                break;

            case TrialType.End:
                if (!hasSentEndMarker) {
                    hasSentEndMarker = true;
                    outlet.push_sample(new float[] { -(float)Utils.EventMarker_BallGame.TrainStart });
                }
                instruction.text = "Training is done. Please wait.";
                break;


        }
        */
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
            playerBall.transform.position = new Vector3(2, -2f, 0);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, horizontal_default * turnAmount * -1), timeCount * turnSpeed);

        timeCount = timeCount + Time.deltaTime;
    }

    //IEnumerator takeBreak(){
        // transition to end state if both left and right hand training are done
        // if (isTakingBreak) {
        //     instruction.text = "- - -";
        //     yield return new WaitForSecondsRealtime(breakTime);
        //     isTakingBreak = false;
        // }

        /*
        if (finishTestingLeft && !finishTestingRight)
            trial = TrialType.RightTrial;
        else if (finishTestingLeft && finishTestingRight){
            trial = TrialType.End;
        }
        else
            trial = TrialType.LeftTrial;
            */
    //}
    IEnumerator startLeft()
    {
        instruction.text = instructionBreak;
        yield return new WaitForSecondsRealtime(2f);
        instruction.text = instructionLeft;
        isLeft = true;
        isRight = false;

        outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.LeftHandTrialStart });
        yield return new WaitForSecondsRealtime(5f);
        outlet.push_sample(new float[] {- (float)Utils.EventMarker_BallGame.LeftHandTrialStart });

        isLeft = false;
        sessionCountLeft += 1;
        inTesting = false;

        // Take break
        // isTakingBreak = true;
        // StartCoroutine(takeBreak());
    }

    IEnumerator startRight()
    {
        instruction.text = instructionBreak;
        yield return new WaitForSecondsRealtime(2f);
        instruction.text = instructionRight;
        isLeft = false;
        isRight = true;
        
        outlet.push_sample(new float[] { (float)Utils.EventMarker_BallGame.RightHandTrialStart });
        yield return new WaitForSecondsRealtime(5f);
        outlet.push_sample(new float[] { -(float)Utils.EventMarker_BallGame.RightHandTrialStart });

        isRight = false;
        sessionCountRight += 1;
        inTesting = false;

        // Take break
        // isTakingBreak = true;
        // StartCoroutine(takeBreak());
    }

}
