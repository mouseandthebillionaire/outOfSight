using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TubeAnimation : MonoBehaviour
{
    public float[] scaleSizes;
    private int     currFrame;
    private int     numFrames = 2;
    
    private void Awake()
    {
        AudioManager.beatUpdated += Animate;
    }

    private void OnDestroy()
    {
        AudioManager.beatUpdated -= Animate;

    }

    // Update is called once per frame
    void Animate()
    {
        // Only animate on the 1s and 5s
        if (AudioManager.lastBeat % transform.parent.gameObject.GetComponent<BeltManager>().beltSpeed == 0)
        {
            this.transform.localScale = new Vector3(scaleSizes[currFrame], this.transform.localScale.y, 0);
            currFrame = (currFrame + 1) % numFrames;
        }
        // if (AudioManager.lastBeat == 3 || AudioManager.lastBeat == 7)
        // {
        //     this.transform.localScale = new Vector3(this.transform.localScale.x, scaleSizes[currFrame], 0);
        // }
    }
}
