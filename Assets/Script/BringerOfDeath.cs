using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BringerOfDeath : MonoBehaviour
{
      
    private bool player;
    public Transform Player;//follow player

    public Slider healthSlider;//slider hp boss
    private int health;

    public GameObject attackSkill;//skill
    public Transform attack;//vị trí tấn công

    private bool right;

    Animator animator;
    Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health=1000;
        healthSlider.maxValue = health;
    }

    
    void Update()
    {                                
            followPlayer();             
    }
   
    private void followPlayer()//thấy player thì chạy theo
    {
        if (player)
        {
            Vector2 moveDirection = (Player.position - transform.position).normalized;
            rb.velocity = moveDirection * 1.5f;// Tốc độ di chuyển
            animator.SetFloat("isRun",Mathf.Abs(moveDirection.x));
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
        else
        {
            rb.velocity = Vector2.zero;//dừng khi không nhìn thấy player          
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shuriken"))
        {
            
            health -= 5;
            healthSlider.value = health;
            
            animator.SetTrigger("isHurt");
            Destroy(other.gameObject,0.1f);//shuriken biến mất
        }
        if (other.gameObject.CompareTag("Player"))
        {
            player = true;
            //animation tấn công
            animator.SetTrigger("isAttack");
            var oneSkill = Instantiate(attackSkill, attack.position, Quaternion.identity);
            Destroy(oneSkill,1f);
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
