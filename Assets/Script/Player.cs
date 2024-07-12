using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;//Vận tốc di chuyển 
    [SerializeField] private float _moveJump;//vận tốc nhảy
    [SerializeField] private float _moveJumpSkill;//vận tốc skill đặc biệt
    [SerializeField] private float _dashBoost = 5f;//vận tốc lướt

    [SerializeField] private Slider _healthSlider;//slider file
    private int maxHealth;//khai báo hp
    public int currentHealth; // Số máu hiện tại
    private float healRate = 1f; // thgian hồi hp
    private float healTimer;
    public TextMeshProUGUI _textHeal;
   

    [SerializeField] private Slider _manaSlider;//slider mana
    private int maxMana;//mana
    public int currentMana;//mana hien tai
    private float manaTimer;
    private float manaRate = 1f;
    
    public TextMeshProUGUI _textMana;
    
    public TextMeshProUGUI _textScore;//điểm số
    private float score=0;

    float speedX;//Horizontal(A,B)

    private bool Right;//mặc định mặt bên phải
    private bool okJump;//true false được phép nhảy
    

    //vị trí bắn
    public Transform Gun;
    public Transform Special;
    //bắn ra
    public GameObject ShurikenBullet;
    public GameObject Attack1bullet;
    public GameObject Attack2bullet;
    public GameObject Attack3bullet;
    public GameObject SpecialBullet;
    

    Rigidbody2D rb;
    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Hp người chs
        currentHealth = maxHealth = 100;
        _healthSlider.maxValue = maxHealth;
        healTimer = healRate;
        _textHeal.text = currentHealth.ToString();

        //mana nguoi chs
        currentMana= maxMana = 200;
        _manaSlider.maxValue = maxMana;
        manaTimer= manaRate;
        _textMana.text = currentMana.ToString();

        _textScore.text = score.ToString();
    }

    void Update()
    {
        Move();
        Flip();
        if (currentMana > 0)
        {
            PlayerAttack();
            Fire();
            Dash();
        }
        TimeHp();
        TimeMana();    
        CurrentHealAndMana();//trả hp và mana theo mặc định đã cho sẵn
       
    }
    
    private void TimeHp()
    {
        if(currentHealth < maxHealth)
        {
            healTimer -= Time.deltaTime;//làm giảm bộ đếm thời gian này dần dần cho đến khi đạt 0
            if (healTimer <= 0)
            {
                Heal(1);//cộng 1 hp
                _healthSlider.value = currentHealth;
                healTimer = healRate;// sau 1 giây sẽ hồi lại máu
                _textHeal.text = currentHealth.ToString();
            }

        }
    }
    private void Heal(int amount)
    {
        //hp mặc định cộng 1 hp
        currentHealth += amount;     
    }

    private void TimeMana()
    {
        if(currentMana < maxMana)
        {
            manaTimer -= Time.deltaTime;
            if(manaTimer <= 0)
            {
                Mana(10);
                manaTimer = manaRate;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();
            }
        }
    }
    private void Mana(int amount)
    {
        currentMana += amount;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
            
        }
    }

    private void CurrentHealAndMana()
    {
        //nếu hp hiện tại lớn hơn hp đã cho thì sẽ chuyển lại thành hp đã cho
        if (currentHealth > maxHealth && currentHealth<maxHealth)
        {
            currentHealth = maxHealth;
            _textHeal.text = currentHealth.ToString();
        }
        //nếu mana hiện tại lớn hơn hp đã cho thì sẽ chuyển lại thành mana đã cho
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
            _textMana.text = currentMana.ToString();
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
    private void Dash()
    {
        if (currentMana >= 10)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!Right)
                {
                    transform.Translate(Vector3.right * _dashBoost * Time.deltaTime);
                }
                if (Right)
                {
                    transform.Translate(Vector3.left * _dashBoost * Time.deltaTime);
                }
                animator.SetTrigger("isDash");
                currentMana -= 10;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();
            }
        }
    }

    private void PlayerAttack()
    {

        //tấn công
        if (currentMana >= 10)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //xử lý slide and text
                animator.SetTrigger("isAttack1");
                currentMana -= 10;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();

                //xử lý skill
                var oneAttackk1 = Instantiate(Attack1bullet, Gun.position, Quaternion.identity);
                Destroy(oneAttackk1, 0.1f);//hủy skill 
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("isAttack2");
                currentMana -= 10;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();

                //xử lý skill
                var oneAttackk1 = Instantiate(Attack2bullet, Gun.position, Quaternion.identity);
                Destroy(oneAttackk1, 0.1f);//hủy skill 
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                animator.SetTrigger("isAttack3");
                currentMana -= 10;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();

                //xử lý skill
                var oneAttackk1 = Instantiate(Attack3bullet, Gun.position, Quaternion.identity);
                Destroy(oneAttackk1, 0.1f);//hủy skill 
            }
        }
        if (currentMana >= 10)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetTrigger("isShuriken");
                currentMana -= 5;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();
            }
        }

        if (currentMana >= 30)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                animator.SetTrigger("isAttackSpecia");
                currentMana -= 30;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();

                //xử lý skill
                var oneAttackk1 = Instantiate(SpecialBullet, Special.position, Quaternion.identity);
                Destroy(oneAttackk1, 0.1f);//hủy skill 
            }
        }
    }

    private void Fire()//bắn shuriken
    {
        //nếu nhấn f thì bắn 
        if (Input.GetKeyDown(KeyCode.F))
        {
            //tạo ra viên đạn tại vị trí súng
            var oneBullet = Instantiate(ShurikenBullet, Gun.position, Quaternion.identity);

            //cho đạn bay theo huong nhân vật
            var velocity = new Vector2(50f, 0);
            if (Right == false)
            {
                velocity.x = 50;
            }
            oneBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Right ? -50 : 50, 0);

            Destroy(oneBullet, 1.5f);
        }
        
    }
    private void Flip()//Xoay mặt
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

    private void OnTriggerEnter2D(Collider2D other)//xử lý va chạm box
    {
        //chạm đất thì dc phép nhảy
        if (other.gameObject.CompareTag("Ground"))
        {
            okJump= true;
        }
            
        if (other.gameObject.CompareTag("Enemy")|| other.gameObject.CompareTag("AttackBoss"))
        {
            
            //nếu đụng enemy thì mất 10Hp
            currentHealth -= 10;
            _healthSlider.value = currentHealth;
            _textHeal.text=currentHealth.ToString();
            
            if (currentHealth >= 10)
            {
                //animation Hurt
                animator.SetTrigger("isHurt");
            }
           if(currentHealth <= 0)
            {
               
                //animation death
                animator.SetTrigger("isDeath");

                Destroy(gameObject, 1f);
            }


        }
        if (other.gameObject.CompareTag("Item")|| other.gameObject.CompareTag("Hp")|| other.gameObject.CompareTag("Mana"))
        {
            score += 20;
            _textScore.text = score.ToString();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Hp"))
        {
            //Đụng bình hp tăng 10Hp và slider tăng theo
            currentHealth += 10;
            _healthSlider.value = currentHealth;
            _textHeal.text=currentHealth.ToString();
            Destroy(other.gameObject);//bình hp biến mất
            
        }
        if (other.gameObject.CompareTag("Mana"))
        {
            //đụng bình mana tăng 50
            currentMana += 50;
            _manaSlider.value = currentMana;
            _textMana.text = currentMana.ToString();
            Destroy(other.gameObject);
        }    
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       
        if (other.gameObject.CompareTag("Ground"))
        {
            okJump= false;         
        }
        
    }
}
