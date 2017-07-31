using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject BatteryUI;
    public GameObject tutorial;

    private void Start()
    {
        World.instance.UIOpen = true;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && World.instance.UIOpen)
        {
            BatteryUI.SetActive(true);
            World.instance.UIOpen = false;
            tutorial.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            BatteryUI.SetActive(false);
            World.instance.UIOpen = true;
            tutorial.SetActive(true);
        }
	}

    public void Quitgame()
    {
        Application.Quit();
    }
}
