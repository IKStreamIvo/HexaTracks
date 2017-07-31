using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(StartScene());
            
        }
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(GetComponent<ScreenFade>().BeginFade(1));
        SceneManager.LoadScene(1);
    }
}
