using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour
{

    public const int maxHealth = 100;
    public bool destroyOnDeath;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    [SyncVar(hook = "OnDeath")]
    public int deathCount = 0;

    public RectTransform healthBar;

    private NetworkStartPosition[] spawnPoints;
    private Text deathCountText;

    public override void OnStartLocalPlayer()
    {
        deathCountText = GameObject.FindGameObjectWithTag("deathCount").GetComponent<Text>();
        deathCountText.text = "Death : " + deathCount;
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                deathCount++;
                // called on the Server, but invoked on the Clients
                RpcRespawn();
            }
        }
    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }


    void OnDeath(int deathCount)
    {
        if (deathCountText != null)
        {
            deathCountText.text = "Death : " + deathCount;
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;

        }
    }
}