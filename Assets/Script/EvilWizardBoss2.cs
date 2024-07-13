using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardBoss2 : MonoBehaviour
{
    public float detectionRangeAttack = 2.5f;  // Phạm vi phát hiện người chơi
    public float detectionRange = 7f;  // Phạm vi phát hiện người chơi
    public Transform Player;//follow player

    private float TimeAttackRate = 2f;
    private float TimeAttackRate2 = 5f;
    private float timeAttack;

    public GameObject attackSkill;//skill 1
    public Transform attack;//vị trí tấn công
    public GameObject attackSkill2;//skill 2

    private bool right;

    Animator animator;
    Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

  
    void Update()
    {
        followPlayer();
        TimeAttack();
    }
    void TimeAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        if (distanceToPlayer < detectionRangeAttack)
        {
            timeAttack -= Time.deltaTime;
            if (timeAttack <= 0)
            {
                //animation tấn công 1
                animator.SetTrigger("isAttack");
                var oneSkill = Instantiate(attackSkill, attack.position, Quaternion.identity);//skill 1
                Destroy(oneSkill, 0.1f);//mất skill
                timeAttack = TimeAttackRate;//lặp lại

                animator.SetTrigger("isAttack2");
                var twoSkill = Instantiate(attackSkill2, attack.position, Quaternion.identity);//skill 2
                Destroy(twoSkill, 0.1f);
                timeAttack = TimeAttackRate2;
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
            transform.Translate( moveDirection * 3f*Time.deltaTime);// Tốc độ di chuyển
            animator.SetFloat("isRun", Mathf.Abs(moveDirection.x));
            
            //xoay mặt
            if (right && moveDirection.x < 0 || !right && moveDirection.x > 0)
            {
                right = !right;
                Vector3 kichThuoc = transform.localScale;
                kichThuoc.x = kichThuoc.x * -1;
                transform.localScale = kichThuoc;
                
            }
        }
    }
}
