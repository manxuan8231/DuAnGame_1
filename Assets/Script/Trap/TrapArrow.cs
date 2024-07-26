using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    public GameObject projectile;
    public Transform SpawnLocation;
    public Quaternion SpawnRotation; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(projectile, SpawnLocation.position,SpawnRotation);
    }
    
}
