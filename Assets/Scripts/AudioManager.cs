using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;
using System.Runtime.InteropServices;

public class AudioManager : MonoBehaviour
{
    public FMODUnity.EventReference music;

    public TimelineInfo timelineInfo = null;
    private GCHandle     timelineHandle;
    
    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.Studio.EVENT_CALLBACK beatCallback;

    public static int lastBeat = 0;

    public delegate void BeatEventDelegate();
    public static event BeatEventDelegate beatUpdated;
    
    public delegate void MarkerListenerDelegate();
    public static event MarkerListenerDelegate markerUpdated;
    
    public static AudioManager S;

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int                currentBeat = 0;
        public FMOD.StringWrapper lastMarker  = new FMOD.StringWrapper();

    }
    
    // Start is called before the first frame update
    private void Awake()
    {
        S = this;
        
        // I'm going to comment this out because it keeps throwing an error, but it should probably be fixed
        //if (music.Path != null)
        //{
            musicInstance = RuntimeManager.CreateInstance(music);
            musicInstance.start();
        //}
    }

    private void Start()
    {
        // Same here
        //if (music.Path != null)
        //{
            timelineInfo = new TimelineInfo();
            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
            musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
            musicInstance.setCallback(beatCallback,
                FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        //}
    }

    private void OnDestory()
    {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
        timelineHandle.Free();
    }

    private void Update()
    {
        if (lastBeat != timelineInfo.currentBeat)
        {
            lastBeat = timelineInfo.currentBeat;
            if (beatUpdated != null)
            {
                beatUpdated();
            }
        }
    }
    
    #if UNITY_EDITOR// || DEVELOPMENT_BUILD
    void OnGUI()
    {
        GUILayout.Box($"Current Beat = {timelineInfo.currentBeat}, last marker = {(string)timelineInfo.lastMarker}");
    }
    #endif

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        } 
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                {
                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr,
                        typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                    timelineInfo.currentBeat = parameter.beat;
                }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                {
                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr,
                        typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    timelineInfo.lastMarker = parameter.name;
                }
                    break;
            }
        }

        return FMOD.RESULT.OK;
    }
}
