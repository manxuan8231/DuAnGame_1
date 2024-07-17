using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFire3 : MonoBehaviour
{
    public float detectionRange = 7f;  // Phạm vi phát hiện người chơi
    private float StopRange = 2.5f;

    public Transform player;

    public float attackInterval = 2f;// Thời gian giữa các đợt tấn công
    private float nextAttackTime;

    private bool Player;
    private bool right;
    Vector2 moveDirection;
    Rigidbody2D rb;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       
    }

  
    void Update()
    {
        FollowPlayer();
        Attack();
        Flip();
    }
    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer < detectionRange)
        {

            if (distanceToPlayer > StopRange)
            {
                moveDirection = (player.position - transform.position).normalized;
                transform.Translate(moveDirection * 1f*Time.deltaTime); 
                animator.SetBool("isWalk",true);
                          
            }
            else
            {
                animator.SetBool("isWalk", false);
                
            }
        }else
        {
            animator.SetBool("isWalk", false);
        }
    }
    void Attack()
    {
        
        if (Player && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackInterval;   
            animator.SetTrigger("isAttack");  
        }

    }
    void Flip()
    {
        if (right && moveDirection.x < 0 ||!right && moveDirection.x > 0)
        {
            right = !right;
            Vector3 direction = transform.localScale;
            direction.x = direction.x * -1;
            transform.localScale = direction;

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = false;
        }
    }
}
