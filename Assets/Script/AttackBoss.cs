using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoss : MonoBehaviour
{
    

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            
           
            
        }
    }
}
