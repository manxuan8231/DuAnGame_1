using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BringerOfDeath : MonoBehaviour
{


    public float detectionRange = 7f;  // Phạm vi phát hiện người chơi
    public Transform Player;//follow player
    private bool player;

    public Slider healthSlider;//slider hp boss
    private int health;

    public GameObject attackSkill;//skill
    public Transform attack;//vị trí tấn công

    private float TimeAttackRate = 2f;
    private float timeAttack;

    private bool right;


    Animator animator;
    Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = 1000;
        healthSlider.maxValue = health;
        timeAttack = TimeAttackRate;

    }

    void Update()
    {
        followPlayer();
        TimeAttack();//độ trể khi thấy player sau 3f tấn công


    }
    void TimeAttack()
    {
        if (player)
        {
            timeAttack -= Time.deltaTime;
            if (timeAttack <= 0)
            {
                //animation tấn công
                animator.SetTrigger("isAttack");
                var oneSkill = Instantiate(attackSkill, attack.position, Quaternion.identity);
                Destroy(oneSkill, 0.1f);
                timeAttack = TimeAttackRate;
            }
        }
    }


    private void followPlayer()//thấy player thì chạy theo
    {
        // Tính khoảng cách giữa quái vật và người chơi
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        // Nếu khoảng cách nhỏ hơn phạm vi phát hiện, quái vật sẽ đuổi theo người chơi
        if (distanceToPlayer < detectionRange)
        {
            Vector2 moveDirection = (Player.position - transform.position).normalized;
            rb.velocity = moveDirection * 1.5f;// Tốc độ di chuyển

            animator.SetFloat("isRun", Mathf.Abs(moveDirection.x));


            //xoay mặt
            if (right && moveDirection.x < 0 || !right && moveDirection.x > 0)
            {
                right = !right;
                Vector3 kichThuoc = transform.localScale;
                kichThuoc.x = kichThuoc.x * -1;
                transform.localScale = kichThuoc;
                animator.SetTrigger("isTele");
            }
        }


    } 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shuriken"))//nếu chạm shuriken thì mất máu
        {
            
            health -= 5;
            healthSlider.value = health;
            
            animator.SetTrigger("isHurt");
            Destroy(other.gameObject);//shuriken biến mất
        }
        if (other.gameObject.CompareTag("Player"))
        {
            player = true;           
        }
        if (other.gameObject.CompareTag("Attack1"))
        {
            health -= 10;
            healthSlider.value = health;

            animator.SetTrigger("isHurt");
        }
        if (other.gameObject.CompareTag("Attack2"))
        {
            health -= 20;
            healthSlider.value = health;

            animator.SetTrigger("isHurt");
        }
        if (other.gameObject.CompareTag("Attack3"))
        {
            health -= 30;
            healthSlider.value = health;

            animator.SetTrigger("isHurt");
        }
        if (other.gameObject.CompareTag("SpecialAttack"))
        {

            health -= 40;
            healthSlider.value = health;
            animator.SetTrigger("isHurt");

        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = false;
        }
       
    }
}
