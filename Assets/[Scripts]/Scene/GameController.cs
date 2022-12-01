using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject onScreenControls;
    public GameObject miniMap;

    private void Start()
    {
        
    }

    void Awake()
    {
        miniMap = GameObject.Find("MiniMap");
        onScreenControls = GameObject.Find("OnScreenControls");

        onScreenControls.SetActive(Application.platform != RuntimePlatform.WindowsPlayer && 
                                   Application.platform != RuntimePlatform.WindowsEditor);

        FindObjectOfType<SoundManager>().PlayMusic(Sound.MAIN_MUSIC);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(!miniMap.activeInHierarchy);
        }
    }
}
