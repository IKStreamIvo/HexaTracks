using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimator : MonoBehaviour {

    public float animSpeed = 1f;

    public Animation Track_L;
    public Animation Track_R;

	public void StartMove(bool forward)
    {
        if (forward)
        {
            Track_L["TrackMove"].speed = animSpeed;
            Track_L.Play("TrackMove");
            Track_R["TrackMove"].speed = animSpeed;
            Track_R.Play("TrackMove");
        }
        else
        {
            Track_L["TrackMove"].speed = -animSpeed;
            Track_L.Play("TrackMove");
            Track_R["TrackMove"].speed = -animSpeed;
            Track_R.Play("TrackMove");
        }
    }

    public void StartRotate(bool right)
    {
        if (right)
        {
            Track_L["TrackMove"].speed = animSpeed;
            Track_L.Play("TrackMove");
            Track_R["TrackMove"].speed = -animSpeed;
            Track_R.Play("TrackMove");
        }
        else
        {
            Track_L["TrackMove"].speed = -animSpeed;
            Track_L.Play("TrackMove");
            Track_R["TrackMove"].speed = animSpeed;
            Track_R.Play("TrackMove");
        }
    }

    public void Stop()
    {
        Track_L.Stop();
        Track_R.Stop();
    }
}
