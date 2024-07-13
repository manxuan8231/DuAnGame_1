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

    public float attackInterval = 2f;// Thời gian giữa các đợt tấn công
    private float nextAttackTime;

    public Transform attack;//vị trí tấn công
    public GameObject attackSkill;//skill 1  
    public GameObject attackSkill2;//skill 2

    public Slider healthSlider;//slider hp
    private float health = 1000;

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
        TimeAttack();
    }
    void TimeAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= detectionRangeAttack && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackInterval;
            int attackType = Random.Range(0, 2);  // Random số nguyên từ 0 tới 1 (0 hoặc 1)

            if (attackType == 0)
            {
                animator.SetTrigger("isAttack");
                var oneSkill = Instantiate(attackSkill, attack.position, Quaternion.identity);
                Destroy(oneSkill, 0.1f);
            }
            else
            {
                animator.SetTrigger("isAttack2");
                var oneSkill = Instantiate(attackSkill2, attack.position, Quaternion.identity);
                Destroy(oneSkill, 0.1f);
            }
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
            health -= 5;
            healthSlider.value = health;
            animator.SetTrigger("isTakeHit");
            if (right)
            {
                transform.Translate(Vector2.left * 20f * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * 20f * Time.deltaTime);
            }
            Destroy(other.gameObject);//shuriken biến mất
        }
        if (other.gameObject.CompareTag("Attack1"))
        {
            health -= 10;
            healthSlider.value = health;

           
        }
        if (other.gameObject.CompareTag("Attack2"))
        {
            health -= 20;
            healthSlider.value = health;

           
        }
        if (other.gameObject.CompareTag("Attack3"))
        {
            health -= 30;
            healthSlider.value = health;

           
        }
        if (other.gameObject.CompareTag("SpecialAttack"))
        {

            health -= 40;
            healthSlider.value = health;
            animator.SetTrigger("isTakeHit");
            if (right)
            {
                transform.Translate(Vector2.left * 20f * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * 20f * Time.deltaTime);
            }
        }
        if (health <= 0)
        {
            Destroy(gameObject, 1f);
            animator.SetTrigger("isDeath");
        }
    }
}
