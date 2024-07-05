using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;//Vận tốc di chuyển 

    [SerializeField] private float _moveJump;//vận tốc nhảy
    [SerializeField] private float _moveJumpSkill;//vận tốc skill đặc biệt
    [SerializeField] private float _dashBoost = 5f;//vận tốc lướt

    [SerializeField] private Slider _healthSlider;//slider file
    private int health;//khai báo hp
    public int currentHealth; // Số máu hiện tại
    public float healRate = 1.0f; // Tốc độ hồi máu, ở đây là mỗi 1 giây hồi 1 máu
    private float healTimer;


    [SerializeField] private Slider _manaSlider;//slider mana
    private int mana;//mana

    float speedX;//Horizontal(A,B)

    private bool Right;//mặc định mặt bên phải
    private bool okJump;//true false được phép nhảy


    public GameObject bullet;//khai báo viên đạn
    public Transform gun;//viên đạn tại vị trí súng

    Rigidbody2D rb;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Hp người chs
        currentHealth = health = 100;
        _healthSlider.maxValue = health;
        healTimer = healRate;

        //mana nguoi chs
        mana = 100;
        _manaSlider.maxValue = mana;
    }

    void Update()
    {
        Move();
        Flip();
        if (mana > 0)//nếu mana lớn hơn 0 thì mới cho 
        {
            AnimatorAttack();
            Fire();
        }
        TimeHp();
    }
    private void TimeHp()
    {
        if(currentHealth < health)
        {
            healTimer -= Time.deltaTime;//làm giảm bộ đếm thời gian này dần dần cho đến khi đạt 0
            if (healTimer <= 0)
            {
                Heal(1);//cộng 1 hp
                _healthSlider.value = currentHealth;
                healTimer = healRate;// sau 1 giây sẽ hồi lại máu
            }

        }
    }
    private void Heal(int amount)
    {
        //hp mặc định cộng 1 hp
        currentHealth += amount;
        //nếu hp hiện tại lớn hơn hp đã cho thì sẽ chuyển lại thành hp đã cho
        if (currentHealth > health)
        {
            currentHealth = health;           
        }
    }
    private void Move()
    {
        //di chuyển
        speedX = Input.GetAxis("Horizontal");
        Vector3 x = new Vector3(speedX, 0, 0);
        transform.Translate(x * _moveSpeed * Time.deltaTime);
        //Hiệu ứng di chuyển       
        animator.SetFloat("isRun", Mathf.Abs(speedX));

        //nhảy
        if (Input.GetKeyDown(KeyCode.Space) && okJump)
        {
            //hiệu ứng nhảy
            animator.SetTrigger("isJump");
            rb.AddForce(Vector2.up * _moveJump, ForceMode2D.Impulse);
        }
        
    }
    private void AnimatorAttack()
    {

        //hiệu ứng tấn công
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("isAttack1");
            mana -= 10;
            _manaSlider.value = mana;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("isAttack2");
            mana -= 10;
            _manaSlider.value = mana;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger("isAttack3");
            mana -= 10;
            _manaSlider.value = mana;
        }
        if (Input.GetKeyDown(KeyCode.R) && okJump)
        {
            animator.SetTrigger("isAttackSpecia");
            mana -= 30;
            _manaSlider.value = mana;
            rb.AddForce(Vector2.up * _moveJumpSkill, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("isShuriken");
            mana -= 1;
            _manaSlider.value = mana;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            animator.SetTrigger("isDash");
            mana -= 5;
            _manaSlider.value = mana;
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
            
        if (other.gameObject.CompareTag("Enemy"))
        {
            
            //nếu đụng enemy thì mất 10Hp
            currentHealth -= 10;
            _healthSlider.value = currentHealth;
            if (currentHealth >= 10)
            {
                //animation Hurt
                animator.SetTrigger("isHurt");
            }
            if (currentHealth <= 0)
            {
                //het mau thi chet
                Destroy(gameObject,0.8f);
               
                //animation death
                animator.SetTrigger("isDeath");
            }
        
        }
        if (other.gameObject.CompareTag("Hp"))
        {
            //Đụng bình hp tăng 10Hp và slider tăng theo
            currentHealth += 10;
            _healthSlider.value = currentHealth;
            Destroy(other.gameObject);//bình hp biến mất
            //nếu hp hiện tại lớn hơn hp đã cho thì sẽ chuyển lại thành hp đã cho
            if(currentHealth > health)
            {
                currentHealth = health;
            }
        }
        if (other.gameObject.CompareTag("Mana"))
        {
            //đụng bình mana tăng 50
            mana += 50;
            _manaSlider.value = mana;
            Destroy(other.gameObject);
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
