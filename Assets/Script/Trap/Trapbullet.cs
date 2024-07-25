using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Trapbullet : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    public float timer;
    private void Start()
    {
        
    }

    private void Update()
    {
        timer += Time.deltaTime; 
        if(timer > 1)
        {
            timer =0 ;
            shoot();
        }
    }
    void shoot()
    {
        Instantiate(bullet,bulletPos.position,Quaternion.identity);
    }

}
