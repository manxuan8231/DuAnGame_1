using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BringerOfDeath : MonoBehaviour
{
    [SerializeField] private float _moveHurt = 10f;//vận tốc bị văng khi dính damage
    [SerializeField] private float _moveSpeed = 1f;//vận tốc di chuyển
    

    private bool Right;

    private bool player;
    public Transform Player;

    public Slider healthSlider;
    private int health;
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
            if (moveDirection.x != 0)
            {
                if (moveDirection.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                if (moveDirection.x > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
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
            //đang xoay mặt bên trái khi trúng shuriken thì văng qua phải 
            if (Right)
            {
                rb.AddForce(Vector2.up * _moveSpeed, ForceMode2D.Impulse);
                transform.Translate(Vector3.left * _moveHurt * Time.deltaTime);
            }
            if (!Right)
            {
                rb.AddForce(Vector2.up * _moveSpeed, ForceMode2D.Impulse);
                transform.Translate(Vector3.right*_moveHurt * Time.deltaTime);
            }

            animator.SetTrigger("isHurt");
            Destroy(other.gameObject);//shuriken biến mất
        }
        if (other.gameObject.CompareTag("Player"))
        {
            player = true;
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
