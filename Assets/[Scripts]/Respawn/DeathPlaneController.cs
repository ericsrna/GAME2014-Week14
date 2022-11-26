using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeathPlaneController : MonoBehaviour
{
    public Transform playerSpawnPoint;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
            player.life.LoseLife();
            player.health.ResetHealth();

            if (player.life.lifeValue > 0)
            {
                ReSpawn(collision.gameObject);

                FindObjectOfType<SoundManager>().PlaySoundFX(Sound.DEATH, Channel.PLAYER_DEATH_FX);
            }
        }
    }

    public void ReSpawn(GameObject gameObject)
    {
        gameObject.transform.position = playerSpawnPoint.position;
    }
}
