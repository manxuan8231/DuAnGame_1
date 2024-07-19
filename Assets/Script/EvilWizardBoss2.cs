using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvilWizardBoss2 : MonoBehaviour
{
    public float detectionRangeAttack = 3f;  // Phạm vi phát hiện người chơi attack
    public float detectionRange = 7f;  // Phạm vi phát hiện người chơi follow

    public float stopRange;
    public Transform Player;//follow player

    public float fireInterval = 6f;// Thời gian giữa các đợt tấn công
    public float attackInterval = 2f;// Thời gian giữa các đợt tấn công
    private float nextAttackTime;

    //vị trí tấn công
    public Transform fire;
    public Transform fire2;
    public Transform fire3;
    public Transform fire4;
    public Transform fire5;
    //fire ball
    public GameObject fireBall;
    public GameObject fireBall2;
    public GameObject fireBall3;
    public GameObject fireBall4;
    public GameObject fireBall5;


    public Transform attack;//vị trí tấn công
    public GameObject attackSkill;//skill 1  
    public GameObject attackSkill2;//skill 2

    public Slider healthSlider;//slider hp
    private float health = 700;

    private bool right;
    Vector2 moveDirection;
    Animator animator;
    Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        healthSlider.maxValue = health;
        
    }

  
    void Update()
    {
        followPlayer();
        Attack();
        
    }
    void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= detectionRangeAttack && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackInterval;
            int attackType = Random.Range(0, 3);  // Random số nguyên từ 0 tới 2 (0 hoặc 2)

            if (attackType == 0)
            {
                animator.SetTrigger("isAttack");
                var oneSkill = Instantiate(attackSkill, attack.position, Quaternion.identity);
                Destroy(oneSkill, 0.1f);
            }          
            if (attackType == 1) 
            {
                animator.SetTrigger("isAttack2");
                var oneSkill = Instantiate(attackSkill2, attack.position, Quaternion.identity);
                Destroy(oneSkill, 0.1f);
            }
            if (attackType == 2)
            {              
                var fireTmp = Instantiate(fireBall, fire.position, Quaternion.identity);               
                Destroy(fireTmp, 5f);
                var fireTmp2 = Instantiate(fireBall2, fire2.position, Quaternion.identity);
                Destroy(fireTmp2, 5f);
                var fireTmp3 = Instantiate(fireBall3, fire3.position, Quaternion.identity);
                Destroy(fireTmp3, 5f);
                var fireTmp4 = Instantiate(fireBall4, fire4.position, Quaternion.identity);
                Destroy(fireTmp4, 5f);
                var fireTmp5 = Instantiate(fireBall5, fire5.position, Quaternion.identity);
                Destroy(fireTmp5, 5f);
            }
        }
    }
   void death()
    {
        if (health <= 0)
        {
            animator.SetTrigger("isDeath");
            Destroy(gameObject, 2f);
            
        }
    }
    private void followPlayer()//thấy player thì chạy theo
    {
        // Tính khoảng cách giữa quái vật và người chơi
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        // Nếu khoảng cách nhỏ hơn phạm vi phát hiện, quái vật sẽ đuổi theo người chơi
        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > stopRange)
            {
                moveDirection = (Player.position - transform.position).normalized;
                transform.Translate(moveDirection * 3f * Time.deltaTime);// Tốc độ di chuyển
                animator.SetBool("isRun", true);

                //xoay mặt
                Flip();
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
    private void Flip()//xoay mat
    {
        if (right && moveDirection.x < 0 || !right && moveDirection.x > 0)
        {
            right = !right;
            Vector3 kichThuoc = transform.localScale;
            kichThuoc.x = kichThuoc.x * -1;
            transform.localScale = kichThuoc;

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
                healthSlider.value = health;
                animator.SetTrigger("isTakeHit");
            }
            
            death();
        }
        if (other.gameObject.CompareTag("Attack1"))
        {
            if (health > 0)
            {
                health -= 10;
                healthSlider.value = health;
                animator.SetTrigger("isHurt");
            }
            death();
        }
        if (other.gameObject.CompareTag("Attack2"))
        {
            if (health > 0)
            {
                health -= 20;
                healthSlider.value = health;
                animator.SetTrigger("isHurt");
            }
            death();
        }
        if (other.gameObject.CompareTag("Attack3"))
        {
            if (health > 0)
            {
                health -= 30;
                healthSlider.value = health;
                animator.SetTrigger("isHurt");
            }
            death();
        }
        if (other.gameObject.CompareTag("SpecialAttack"))
        {
            if (health > 0)
            {
                health -= 40;
                healthSlider.value = health;
                animator.SetTrigger("isTakeHit");
            }
            death();
        }
        
    }
}
