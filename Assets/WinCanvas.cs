using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvas : MonoBehaviour {

    public void Attempts(int attempts)
    {
        transform.Find("Attempts").GetComponent<Text>().text = attempts.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
