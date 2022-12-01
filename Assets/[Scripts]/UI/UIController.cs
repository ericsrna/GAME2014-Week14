using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject miniMap;

    private void Start()
    {
        miniMap = GameObject.Find("MiniMap");
    }

    public void OnRestartButton_Pressed()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnYButton_Pressed()
    {
        if (miniMap != null)
            miniMap.SetActive(!miniMap.activeInHierarchy);
    }
}
