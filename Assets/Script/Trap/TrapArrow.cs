using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    public GameObject projectile;
    public Transform SpawnLocation;
    public Quaternion SpawnRotation;
    public float spawnTime = 0.5f;
    private float timeSinceSpawned = 0f; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawned += Time.deltaTime;
        if (timeSinceSpawned >= spawnTime)
        {
            Instantiate(projectile, SpawnLocation.position, SpawnRotation);
        }
    }
    
}
