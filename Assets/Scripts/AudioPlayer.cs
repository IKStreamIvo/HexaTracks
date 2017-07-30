using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    public static AudioPlayer instance;
    public AudioSource source;

    public AudioClip robotTurn;
    public AudioClip robotMove;
    public AudioClip forceField;
    public AudioClip batteryDeath;
    public AudioClip finish;

    void Awake () {
        instance = this;
        source.clip = null;
        source.Stop();
	}
	
	public void RobotTurn()
    {
        if (robotTurn != null)
        {
            source.clip = robotTurn;
            source.Play();
        }
    }
    public void RobotMove()
    {
        if (robotMove != null)
        {
            source.clip = robotMove;
            source.Play();
        }
    }
    public void Forcefield()
    {
        if (forceField != null)
        {
            source.clip = forceField;
            source.Play();
        }
    }
    public void BatteryDeath()
    {
        if (batteryDeath != null)
        {
            source.clip = batteryDeath;
            source.Play();
        }
    }
    public void Finish()
    {
        if (finish != null)
        {
            source.clip = finish;
            source.Play();
        }
    }
}
