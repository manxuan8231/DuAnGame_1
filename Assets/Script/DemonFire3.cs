using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonFire3 : MonoBehaviour
{
    public float detectionRange = 7f;  // Phạm vi phát hiện người chơi
    private float StopRange = 2.5f;
    public float deterctionAttack = 5f;
    public Transform player;

    public float attackCooldown = 5f; // Thời gian hồi chiêu sau mỗi lần tấn công
    public float attackDuration = 2.3f; // Thời gian thực hiện đòn tấn công (tính từ lúc bắt đầu đến khi kết thúc)
    private float nextAttackTime = 0f;
    private float attackEndTime;
    private bool isAttacking = false;
    private bool stopAttack = true;
    //xử lí hp
    public Slider healSlider;
    private float health;
    //skill 1
    public GameObject attackSkill;
    public Transform attack;
    //skill2
    public GameObject fireBall;
    public Transform fireTransform;
    public GameObject fireBall2;
    public Transform fireTransform2;
    public GameObject fireBall3;
    public Transform fireTransform3;
    public GameObject fireBall4;
    public Transform fireTransform4;

    
    private bool right;
    Vector2 moveDirection;
    Rigidbody2D rb;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = 800;
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
        if (stopAttack)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= deterctionAttack && Time.time >= nextAttackTime && !isAttacking)
            {
                nextAttackTime = Time.time + attackCooldown;
                int attackType = Random.Range(0, 3);  // Random số nguyên từ 0 tới 2 (0 hoặc 2)
                if (attackType == 0)
                {

                    attackEndTime = Time.time + attackDuration;
                    isAttacking = true;
                    animator.SetTrigger("isAttack");
                }
                if (attackType == 1)
                {
                    var oneBullet = Instantiate(fireBall, fireTransform.position, Quaternion.identity);
                    //cho đạn bay theo huong nhân vật
                    var velocity = new Vector2(50f, 0);
                    if (right == false)
                    {
                        velocity.x = 50;
                    }
                    oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(!right ? -4 : 4, 0);

                    var oneBullet2 = Instantiate(fireBall2, fireTransform2.position, Quaternion.identity);
                    //cho đạn bay theo huong nhân vật
                    var velocity2 = new Vector2(50f, 0);
                    if (right == false)
                    {
                        velocity2.x = 50;
                    }
                    oneBullet2.GetComponent<Rigidbody2D>().velocity = new Vector2(!right ? -4 : 4, 0);

                    var oneBullet3 = Instantiate(fireBall3, fireTransform3.position, Quaternion.identity);
                    //cho đạn bay theo huong nhân vật
                    var velocity3 = new Vector2(50f, 0);
                    if (right == false)
                    {
                        velocity3.x = 50;
                    }
                    oneBullet3.GetComponent<Rigidbody2D>().velocity = new Vector2(!right ? -4 : 4, 0);

                    var oneBullet4 = Instantiate(fireBall4, fireTransform4.position, Quaternion.identity);
                    //cho đạn bay theo huong nhân vật
                    var velocity4 = new Vector2(50f, 0);
                    if (right == false)
                    {
                        velocity4.x = 50;
                    }
                    oneBullet4.GetComponent<Rigidbody2D>().velocity = new Vector2(!right ? -4 : 4, 0);


                }
            }
            if (isAttacking && Time.time >= attackEndTime)
            {

                var oneSkill = Instantiate(attackSkill, attack.position, Quaternion.identity);
                Destroy(oneSkill, 0.5f);
                isAttacking = false;
            }
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
       
        //chạm shuriken
        if (other.gameObject.CompareTag("Shuriken"))
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
            stopAttack = false;

        }
        else
        {
            stopAttack = true;
        }
        if (other.gameObject.CompareTag("Attack1"))
        {
            health -= 10;
            healSlider.value = health;
            animator.SetTrigger("isHurt");
            stopAttack = false;

        }
        else
        {
            stopAttack = true;
        }

        if (other.gameObject.CompareTag("Attack2"))
        {
            health -= 20;
            healSlider.value = health;
            animator.SetTrigger("isHurt");
            stopAttack = false;

        }
        else
        {
            stopAttack = true;
        }

        if (other.gameObject.CompareTag("Attack3"))
        {
            health -= 35;
            healSlider.value = health;
            animator.SetTrigger("isHurt");
            stopAttack = false;

        }
        else
        {
            stopAttack = true;
        }

        if (other.gameObject.CompareTag("SpecialAttack"))
        {

            health -= 40;
            healSlider.value = health;
            animator.SetTrigger("isTakeHit");
            stopAttack = false;

        }
        else
        {
            stopAttack = true;
        }


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
