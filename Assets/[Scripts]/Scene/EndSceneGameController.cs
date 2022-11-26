using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndSceneGameController : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<SoundManager>().PlayMusic(Sound.END_MUSIC);
    }
}
