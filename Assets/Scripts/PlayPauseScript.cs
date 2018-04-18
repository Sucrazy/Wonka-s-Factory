using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPauseScript : MonoBehaviour {

    private bool isPlaying;
	public void PlayTrigger()
    {
        if (isPlaying)
        {
            GetComponent<Text>().text = "Play";
            isPlaying = false;
        }
        else
        {
            GetComponent<Text>().text = "Pause";
            isPlaying = true;
        }
    }
}
