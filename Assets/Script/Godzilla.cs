using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godzilla : MonoBehaviour
{
    public float detectionRange = 10f;  // Phạm vi phát hiện người chơi
    public float stopRange = 2f;
    public float AppearRange = 15f;
    public float detectionAttack = 2f;
    public float attack3Range = 30f;
    public float stopAttack3Range = 5f;

    private float timeCoolDown;
    private float attackEndTime;
    public float attackDuration;

    private bool isAppear;
    private bool isAttack;
    
    private bool isAttacking;
    private bool isAttacking3;

    private bool right;

    public Transform TransformAttack;
    public GameObject Attack1;
    public Transform TransformAttack3;
    public GameObject attack3;

    public Transform player;
    Animator animator;
    Vector2 moveDiretion;
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    
    void Update()
    {
        FollowPlayer();
        Appear();
        Attack();
        Attack3();
        Flip();
    }
    private void Appear()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance < AppearRange)
        {
            
            animator.SetTrigger("isAppear");
            isAppear = true;
        }
    }
   
    private void FollowPlayer()
    {
        if (isAppear)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < detectionRange)
            {
                if (distance > stopRange)
                {
                     moveDiretion = (player.position - transform.position).normalized;
                    transform.Translate(moveDiretion * 1 * Time.deltaTime);
                    animator.SetBool("isRun", true);

                }
                else
                {
                    animator.SetBool("isRun", false);
                }

            }
            else
            {
                animator.SetBool("isRun", false);
            }
        }
    }

    private void Attack()
    {
        
        if(isAttack && Time.time >= timeCoolDown +2f && !isAttacking)
        {
            timeCoolDown = Time.time;
            int typeAttack = Random.Range(0, 2);
            if(typeAttack == 0)
            {
                attackEndTime = Time.time + attackDuration;
                animator.SetTrigger("isAttack1");
                isAttacking = true;
                
            }
            if (typeAttack == 1)
            {
                attackEndTime = Time.time + attackDuration;
                animator.SetTrigger("isAttack2");
                isAttacking = true;
            }

        }
        if (isAttacking && Time.time >= attackEndTime)
        {
            var oneSkill = Instantiate(Attack1, TransformAttack.position, Quaternion.identity);
            Destroy(oneSkill, 0.1f);
            isAttacking = false;
        }
    }
    private void Attack3()
    {
        if (isAppear)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < attack3Range && Time.time >= timeCoolDown + 4 && !isAttacking3)
            {
                timeCoolDown = Time.time;
                if (distance > stopAttack3Range)
                {
                    attackEndTime = Time.time + attackDuration;
                    animator.SetTrigger("isAttack3");
                    isAttacking3 = true;
                }
            }
            if (isAttacking3 && Time.time >= attackEndTime)
            {
                var oneSkill = Instantiate(attack3, TransformAttack3.position, Quaternion.identity);
                Destroy(oneSkill, 1f);
                var velocity = new Vector2(-20f, 0);
                if (right == false)
                {
                    velocity.x = -20;
                }
                oneSkill.GetComponent<Rigidbody2D>().velocity = new Vector2(!right ? 20 : -20, 0);
                isAttacking3 = false;
                // Xoay viên đạn theo hướng di chuyển của boss
                oneSkill.transform.localScale = new Vector3(right ? 1 : -1, 1, 1);

            }
        }
    }
    void Flip()
    {
        if (right && moveDiretion.x > 0 || !right && moveDiretion.x < 0)
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
            isAttack = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttack = false;
           
        }
    }
}
