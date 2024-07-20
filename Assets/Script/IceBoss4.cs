using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceBoss4 : MonoBehaviour
{
    public float detectionRange = 10f;  // Phạm vi phát hiện người chơi
    private float StopRange = 2.5f;
    public float detectionAttack = 5f;
    float time;
    public Slider healSlider;
    public float health;
    private bool right;

    private bool stopAttack=true;
    private bool Player;
    public Transform player;
    Animator animator;
    Vector2 moveSpeed;
    void Start()
    {
        animator = GetComponent<Animator>();
        health = 700;
        healSlider.maxValue = health;
       
    }


    void Update()
    {
       
            FollowPlayer();
            Attack();
            Flip();
        
    }
    
    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < detectionRange)
        {
            if (distance > StopRange)
            {
               moveSpeed = (player.position - transform.position).normalized;
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
        if (stopAttack)
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
    void Flip()
    {
        if (right && moveSpeed.x < 0|| !right && moveSpeed.x>0)
        {
            right = !right;
            Vector3 direction = transform.localScale;
            direction.x = direction.x * -1;
            transform.localScale = direction;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {       
        //chạm shuriken
        if (other.gameObject.CompareTag("Shuriken"))
        {
            if (health > 0)
            {
                health -= 5;
                healSlider.value = health;
                animator.SetTrigger("isTakeHit");
                if (right)
                {
                    transform.Translate(Vector2.left * 6f * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.right * 6f * Time.deltaTime);
                }
                Destroy(other.gameObject);//shuriken biến mất
                
            }
            
            stopAttack = false;
        }
        else
        {
            stopAttack = true;
        }
        if (other.gameObject.CompareTag("Attack1"))
        {
            if (health > 0)
            {
                health -= 10;
                healSlider.value = health;
                animator.SetTrigger("isHurt");
                
            }
            
            stopAttack = false;
        }
        else
        {
            stopAttack = true;
        }
        if (other.gameObject.CompareTag("Attack2"))
        {
            if (health > 0)
            {
                health -= 20;
                healSlider.value = health;
                animator.SetTrigger("isHurt");
                
            }
            
            stopAttack = false;
        }
        else
        {
            stopAttack = true;
        }
        if (other.gameObject.CompareTag("Attack3"))
        {
            if (health > 0)
            {
                health -= 35;
                healSlider.value = health;
                animator.SetTrigger("isHurt");
                
            }
           
            stopAttack = false;
        }
        else
        {
            stopAttack = true;
        }
        if (other.gameObject.CompareTag("SpecialAttack"))
        {
            if (health > 0)
            {
                health -= 40;
                healSlider.value = health;
                animator.SetTrigger("isTakeHit");
                
            }           
            stopAttack = false;
        }
        else
        {
            stopAttack = true;
        }

    }
}