using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;//Vận tốc di chuyển 
    [SerializeField] private float _moveJump;//vận tốc nhảy
    [SerializeField] private float _moveJumpSkill;//vận tốc skill đặc biệt
    [SerializeField] private float _dashBoost = 5f;//vận tốc lướt

    float speedX;//Horizontal(A,B)

    private bool Right;//mặc định mặt bên phải
    private bool okJump;//true false được phép nhảy

    public float dashTime;//thgian lướt
    private float _dashTime;
    bool isDashing = false;

    public GameObject bullet;//khai báo viên đạn
    public Transform gun;//viên đạn tại vị trí súng

    public LayerMask enemyLayer;
    Rigidbody2D rb;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {
        Move();
        Flip();
        Animator();
        Fire();
    }

   private void Move()
   {
        //di chuyển
        speedX = Input.GetAxis("Horizontal");
        Vector3 x = new Vector3(speedX, 0, 0);
        transform.Translate(x*_moveSpeed*Time.deltaTime);
        //Hiệu ứng di chuyển       
        animator.SetFloat("isRun", Mathf.Abs(speedX));

        //nhảy
        if (Input.GetKeyDown(KeyCode.Space)&&okJump)
        {
            //hiệu ứng nhảy
            animator.SetTrigger("isJump");
            rb.AddForce(Vector2.up*_moveJump,ForceMode2D.Impulse);
        }
        //lướt
        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTime <= 0 && isDashing == false)
        {
            animator.SetTrigger("isDash");
            _moveSpeed += _dashBoost;
            _dashTime = dashTime;
            isDashing = true;
        }
        if (_dashTime <= 0 && isDashing == true)
        {
            _moveSpeed -= _dashBoost;
            isDashing = false;
        }
        else
        {
            _dashTime -= Time.deltaTime;
        }
    }
    private void Animator()
    {
        //hiệu ứng tấn công
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("isAttack1");
           
          
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("isAttack2");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger("isAttack3");
        }
        if (Input.GetKeyDown(KeyCode.R)&&okJump)
        {
            animator.SetTrigger("isAttackSpecia");
            rb.AddForce(Vector2.up * _moveJumpSkill, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("isShuriken");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("isDash");
        }
    }

    private void Fire()
    {
        //nếu nhấn f thì bắn 
        if (Input.GetKeyDown(KeyCode.F))
        {
            //tạo ra viên đạn tại vị trí súng
            var oneBullet = Instantiate(bullet, gun.position, Quaternion.identity);

            //cho đạn bay theo huong nhân vật
            var velocity = new Vector2(50f, 0);
            if (Right == false)
            {
                velocity.x = 50;
            }
            oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Right ? -50 : 50, 0);

            Destroy(oneBullet, 2f);
        }
        
    }
    private void Flip()
    {
        //xoay mặt
        if (Right && speedX > 0 || !Right && speedX < 0)
        {
            Right = !Right;
            Vector3 kichThuoc = transform.localScale;
            kichThuoc.x = kichThuoc.x * -1;
            transform.localScale = kichThuoc;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //chạm đất thì dc phép nhảy
        if (other.gameObject.CompareTag("Ground"))
        {
            okJump= true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //không chạm đất thì không được nhảy
        if (other.gameObject.CompareTag("Ground"))
        {
            okJump= false;
        }
    }
}
