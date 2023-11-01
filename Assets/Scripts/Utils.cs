using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public static class Utils
{
    public enum EventMarker_BallGame
    {
        TrainStart = 1,
        LeftHandTrialStart = 2,
        RightHandTrialStart = 3,
        //LeftBlockStart = 7,
        //RightBlockStart = 8,
        EvalStart = 6,
    }

    public enum GameState
    {
        Training,
        Fitting,
        Evaluation
    }

    public enum TrialType {
        Start,
        //LeftFinish,
        LeftTrial,
        //RightFinish,
        RightTrial,
        End
    }

    public static StreamOutlet InitLSLStreamOutlet(string streamName, string streamType, int channelNum, float nominalSamplingRate, LSL.channel_format_t channelFormat)
    {
        StreamInfo streamInfo = new StreamInfo(
            streamName,
            streamType,
            channelNum,
            nominalSamplingRate,
            channelFormat
            );

        return new StreamOutlet(streamInfo);
    }
}
