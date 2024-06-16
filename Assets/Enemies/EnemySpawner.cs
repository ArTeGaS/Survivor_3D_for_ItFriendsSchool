using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float len;
    float width;
    public float spawnRate;

    GameObject player;
    public GameObject enemy;
    private void Start()
    {
        player = GameObject.Find("Player");
        InvokeRepeating("SpawnEnemy", 2, spawnRate);
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPosition = player.transform.position;

        float h_or_w = Random.Range(0, 2);
        float p_or_m = Random.Range(0, 2);
        switch (h_or_w)
        {
            case 0:
                len = Random.Range(0f, 14.1f);
                width = 20f;
                break;
            case 1:
                len = 14f;
                width = Random.Range(0f, 20.1f);
                break;
        }
        switch (p_or_m)
        {
            case 0:
                spawnPosition.x = spawnPosition.x + width;
                spawnPosition.z = spawnPosition.z + len;
                break; 
            case 1:
                spawnPosition.x = spawnPosition.x - width;
                spawnPosition.z = spawnPosition.z - len;
                break;
        }
        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }
}
