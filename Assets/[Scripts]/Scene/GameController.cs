using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject onScreenControls;

    void Awake()
    {
        onScreenControls = GameObject.Find("OnScreenControls");

        onScreenControls.SetActive(Application.platform != RuntimePlatform.WindowsPlayer && 
                                   Application.platform != RuntimePlatform.WindowsEditor);

        //FindObjectOfType<SoundManager>().PlayMusic(Sound.MAIN_MUSIC);
    }
}
