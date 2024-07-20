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
    
    //health
    [SerializeField] private Slider _healthSlider;//slider file
    private int maxHealth;//khai báo hp
    public float currentHealth; // Số máu hiện tại
    private float healRate = 1f; // thgian hồi hp
    private float healTimer;
    public TextMeshProUGUI _textHeal;
    //dash
    public float dashSpeed = 5f;
    public float dashTime = 0.2f;
    private float dashCooldown = 1f;
    private float lastDash = -1f;
    private bool isDashing = false;
    private float dashDirection = 1f; // 1 for right, -1 for left
    //mana
    [SerializeField] private Slider _manaSlider;//slider mana
    private int maxMana;//mana
    public int currentMana;//mana hien tai
    private float manaTimer;
    private float manaRate = 1f;   
    public TextMeshProUGUI _textMana;
    //score
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
    //mất hp dần 
    public float damageAmount = 10f;  // Lượng sát thương mỗi 2 giây
    public float damageInterval = 2f; // Thời gian giữa mỗi lần giảm máu
    public float damageDuration = 7f; // Thời gian sát thương kéo dài
    private bool isTakingDamage = false;  // Cờ kiểm tra nhân vật có đang bị dính fire ball không
    private float damageTimer = 0f;       // Bộ đếm thời gian tổng cho sát thương
    private float intervalTimer = 0f;     // Bộ đếm thời gian cho khoảng cách giữa mỗi lần giảm máu


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
        TakingHeal();
       
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
        if (currentHealth > 0)
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
                animator.SetBool("isJump", true);
                rb.AddForce(Vector2.up * _moveJump, ForceMode2D.Impulse);
            }
        }
    }
    private void Dash()
    {
        if (currentHealth >= 0)
        {

            if (Input.GetKey(KeyCode.LeftShift) && Time.time >= lastDash + dashCooldown)
            {
                StartDash();
                animator.SetTrigger("isDash");
                currentMana -= 20;
                _manaSlider.value = currentMana;
                _textMana.text = currentMana.ToString();
            }

            if (isDashing)
            {
                rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);
            }
        }     
           
    }
    private void StartDash()
    {
        lastDash = Time.time;
        isDashing = true;
        dashDirection = transform.localScale.x > 0 ? 1f : -1f; // Kiểm tra hướng của nhân vật

        Invoke("StopDash", dashTime);
    }

    private void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;
    }
    private void PlayerAttack()
    {
        if (currentHealth >= 0)
        {
            //tấn công
            if (currentMana >= 10)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //xử lý slide and text
                    animator.SetTrigger("isAttack1");

                    //xử lý skill
                    var oneAttackk1 = Instantiate(Attack1bullet, Gun.position, Quaternion.identity);
                    Destroy(oneAttackk1, 0.1f);//hủy skill 
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    animator.SetTrigger("isAttack2");

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
    }

    private void Fire()//bắn shuriken
    {
        if (currentHealth >= 0)
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
    }
    private void Death()
    {
        if (currentHealth >= 10)
        {
            //animation Hurt
            animator.SetTrigger("isHurt");
           
        }
        if (currentHealth <= 0)
        {

            //animation death
            animator.SetTrigger("isDeath");

            Destroy(gameObject, 1f);
        }
    }
    private void Flip()//Xoay mặt
    {
        if (currentHealth >= 0)
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
    }

    private void TakingHeal()
    {
        if (currentHealth >= 0) {


            if (isTakingDamage)
            {
                damageTimer += Time.deltaTime;
                intervalTimer += Time.deltaTime;
                if (intervalTimer >= damageInterval)
                {
                    currentHealth -= damageAmount;
                    _healthSlider.value = currentHealth;
                    _textHeal.text = currentHealth.ToString();
                    if (currentHealth > 0)
                    {
                        animator.SetTrigger("isFireHurt");
                    }
                    intervalTimer = 0f;
                    Death();
                }
                if (damageTimer >= damageDuration)
                {
                    isTakingDamage = false;  // Dừng việc mất máu sau khi hết thời gian
                    damageTimer = 0f;        // Reset bộ đếm thời gian
                    intervalTimer = 0f;      // Reset bộ đếm thời gian cho khoảng cách giữa mỗi lần giảm máu
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)//xử lý va chạm box
    {
        if (currentHealth >= 0)
        {
            //chạm đất thì dc phép nhảy
            if (other.gameObject.CompareTag("Ground"))
            {
                okJump = true;
                animator.SetBool("isJump", false);
            }
            //chạm skill mất hp
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("AttackBoss"))
            {

                //nếu đụng enemy thì mất 10Hp
                currentHealth -= 10;
                _healthSlider.value = currentHealth;
                _textHeal.text = currentHealth.ToString();

            }
            //chạm skill 1 của boss 2
            if (other.gameObject.CompareTag("Skill1(Boss2)"))
            {
                //nếu đụng enemy thì mất 10Hp
                currentHealth -= 5;
                _healthSlider.value = currentHealth;
                _textHeal.text = currentHealth.ToString();

                Death();
            }
            //chạm skill 2 của boss 2
            if (other.gameObject.CompareTag("Skill2(Boss2)"))
            {
                //nếu đụng enemy thì mất 5Hp
                currentHealth -= 5;
                _healthSlider.value = currentHealth;
                _textHeal.text = currentHealth.ToString();
                Death();
            }
            if (other.gameObject.CompareTag("FireBall1"))
            {
                currentHealth -= 10;
                _healthSlider.value = currentHealth;
                _textHeal.text = currentHealth.ToString();

                isTakingDamage = true;
                damageTimer = 0f;  // Reset bộ đếm thời gian
                intervalTimer = 0f; // Reset bộ đếm thời gian cho khoảng cách giữa mỗi lần giảm máu
                Death();
            }
            if (other.gameObject.CompareTag("Skill1(Boss3)"))
            {
                //nếu đụng enemy thì mất Hp
                currentHealth -= 10;
                _healthSlider.value = currentHealth;
                _textHeal.text = currentHealth.ToString();
                Death();
            }
            if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("Hp") || other.gameObject.CompareTag("Mana"))
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
                _textHeal.text = currentHealth.ToString();
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
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       
        if (other.gameObject.CompareTag("Ground"))
        {
            okJump= false;         
        }
        
    }
    
}
