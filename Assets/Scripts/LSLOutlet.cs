using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using static Utils;

public class LSLOutlet : MonoBehaviour
{
    [Header("LSL Objects")]
    public string streamName = "EventMarker_BallGame";
    public StreamOutlet Outlet;
    private float LSL_IRREGULAR_RATE = 0f;
    public void Awake() {
        DontDestroyOnLoad(this.gameObject);
        Outlet = InitLSLStreamOutlet(streamName, "EventMarker_BallGame", 1, LSL_IRREGULAR_RATE, channel_format_t.cf_float32);
    }
}
