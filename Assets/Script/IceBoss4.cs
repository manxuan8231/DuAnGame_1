using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoss4 : MonoBehaviour
{
    public float detectionRange = 10f;  // Phạm vi phát hiện người chơi
    private float StopRange = 2.5f;
    public float detectionAttack = 5f;
    float time;

    private bool Player;
    public Transform player;
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        FollowPlayer();
        Attack();
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance < detectionRange)
        {
            if (distance > StopRange)
            {
                Vector2 moveSpeed = (player.position - transform.position).normalized;
                transform.Translate(moveSpeed * 1f * Time.deltaTime);
                animator.SetBool("isWalk", true);
               
            }
            else
            {
                animator.SetBool("isWalk", false);
            }
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }
    void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        time -= Time.deltaTime;
        if (distanceToPlayer < detectionAttack && time < 0)
        {
                  
                //animation tấn công
                animator.SetTrigger("isAttack");
           time = 2;
        }
    }
}
