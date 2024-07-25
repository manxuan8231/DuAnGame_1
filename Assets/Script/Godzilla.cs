using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godzilla : MonoBehaviour
{
    public Transform player;
    //xử lý chạy theo player
    public float detectionWalk = 3f; //tính khoản cách player không trong phạm vi thì walk
    public float detectionRun = 7f; //tính khoản cách player không trong phạm vi thì run
    private bool isWalk;
    //xử lý xuất hiện 
    public float detectionAppear = 7f; //phạm vi nhìn thấy người chơi 
    private float appearTimer;
    private bool isAppearing = false; // để theo dõi trạng thái "Appear"

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
 
    void Update()
    {
        Appear();
        Walk();
    }
    
    private void Appear()//thấy người chơi thì xuất hiện
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionAppear && !isAppearing)
        {
            isAppearing = true;
            animator.SetTrigger("isAppear");
            appearTimer = 1.4f; // đặt lại thời gian
        }
        if (isAppearing)
        {
            appearTimer -= Time.deltaTime;
            if (appearTimer <= 0)
            {
                animator.SetTrigger("isIdle");
                isAppearing = false;
                isWalk = true;//chạy xong idle mới dc true
            }else
            {
                isWalk = false;
            }
        }
    }
    private void Walk()
    {
        if(isWalk)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance > detectionWalk)
            {
                Vector2 moveWalk = (player.position - transform.position).normalized;
                transform.Translate(moveWalk * 1f * Time.deltaTime);
                animator.SetBool("isWalk", true);
            }
            else
            {
                animator.SetBool("isWalk", false);
            }
        }
    }
}
