using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godzilla : MonoBehaviour
{
    public Transform player;
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
            }
        }
    }
    
}
