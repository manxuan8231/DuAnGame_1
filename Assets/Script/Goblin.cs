using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Goblin : MonoBehaviour
{
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;
    [SerializeField] private float _moveSpeed;

    public Transform player;
    public float detectionRange = 5f; //tính khoản cách thấy player r chạy theo  
    public float detectionAttack1 = 1f;
    public float detectionIdle = 1f;

    Animator animator;
    bool isFollow;   
    bool isRight = true;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
  
    void Update()
    {
        FollowPlayer();
        Curent();
        moveRun();      
    }

    private void moveRun()
    {
        if (isFollow == true)
        {
            var direction = Vector3.right;
            if (isRight == false)
            {
                direction = Vector3.left;
            }
            transform.Translate(direction * _moveSpeed * Time.deltaTime);      
            if(direction.x != 0)
            {
                if(direction.x > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                if (direction.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            animator.SetBool("isRun", true);
        }
    }
    private void Curent()
    {
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightBoundary)
        {
            isRight = false;
        }
        else if (currentPosition.x < leftBoundary)
        {
            isRight = true;    
        }        
    }

    private void FollowPlayer()
    {
        float direction = Vector3.Distance(transform.position, player.position);
        if(direction < detectionRange)
        {           
            if (direction > detectionIdle)
            {
                animator.SetBool("isRun", true);
                isFollow = false;
                Vector2 moveDirection = (player.position - transform.position).normalized;
                transform.Translate(moveDirection * _moveSpeed * Time.deltaTime);
                //flip
                if (isRight && moveDirection.x < 0 || !isRight && moveDirection.x > 0)
                {
                    isRight = !isRight;
                    Vector3 distance = transform.localScale;
                    distance.x = distance.x * -1;
                    transform.localScale = distance;
                }
            }
            else
            {
                animator.SetBool("isRun", false);
            }       
        }
        else
        {
            isFollow = true;           
        }

    }
}
