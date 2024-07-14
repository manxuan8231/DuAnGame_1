using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
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
        if (other.gameObject.CompareTag("Player")|| other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Boss"))
        {
            animator.SetTrigger("isBreak");
            Destroy(gameObject, 0.5f);
        }
    }
   
}
