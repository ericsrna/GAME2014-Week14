using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LifeCounterController : MonoBehaviour
{
    [Header("Life Properties")]
    public int lifeValue;

    private Image lifeImage;

    // Start is called before the first frame update
    void Start()
    {
        lifeValue = 3;
        lifeImage = GetComponent<Image>();
    }

    public void ResetLives()
    {
        lifeValue = 3;
        lifeImage.sprite = Resources.Load<Sprite>("Sprites/Life3");
    }

    public void LoseLife()
    {
        lifeValue -= 1;
        if (lifeValue < 0) lifeValue = 0;

        lifeImage.sprite = Resources.Load<Sprite>($"Sprites/Life{lifeValue}");
    }

    public void GainLife()
    {
        lifeValue += 1;
        if (lifeValue > 3) lifeValue = 3;

        lifeImage.sprite = Resources.Load<Sprite>($"Sprites/Life{lifeValue}");
    }
}
