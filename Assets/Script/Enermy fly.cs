using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermyfly : MonoBehaviour
{   
    public float speed;
    [SerializeField] private Transform target;
    [SerializeField] private float ShootingRange;
    [SerializeField] private float lineofsite;
    [SerializeField] private float khaccach;
    public GameObject bullet;
    public GameObject bulletParant;
    public float fireRage = 1f;
    public float nextFireTime;
    
    private bool _flip;
    public float rightBoundary;
    public float leftBoundary;
    public bool _isMovingRight;

    private float timeBtwShot;
    public float startTimeBtwShort;
    public float fly;

    

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Hitbox").GetComponent<Transform>();

    }
    void Update()
    {
        move();
    }
    public void move()
    {
        if (Vector2.Distance(transform.position, target.position) < khaccach)
        {

            Attack();
        }
        else
        {
            spawn();
        }
    }
    private void spawn()
    {
        
        var currenPosition = transform.position;
         if (currenPosition.x > rightBoundary)
        {
            _isMovingRight = false;
        }
        else if (currenPosition.x < leftBoundary)
        {           
            _isMovingRight = true;
        }      
        
        var direction = Vector3.right;
        if (_isMovingRight == false)
        {            
            direction = Vector3.left;
        }
        transform.Translate(direction * speed * Time.deltaTime);
        
        // Scale Hiện tại
        // Trái < 0 Phải >0
        var currentSacle = transform.localScale;

        if ((_isMovingRight && currentSacle.x < 0) || (_isMovingRight == false && currentSacle.x > 0))
        {
            flip();
        }
    }
    private void Attack()
    {
        float DistanceFromPlayer = Vector2.Distance(transform.position, target.position);
        if(DistanceFromPlayer > lineofsite&& DistanceFromPlayer>ShootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
        }else if( DistanceFromPlayer <= ShootingRange&& nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletParant.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRage;
            Destroy(bullet,3f);
        }
        isflip();
    }


    private void isflip()
    {
        if (transform.position.x < target.position.x && _flip)
        {
            flip();
        }
        else if (transform.position.x > target.position.x && !_flip)
        {
            flip();
        }
    }
    void flip()
    {
        _flip = !_flip;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineofsite); 
        Gizmos.DrawWireSphere(transform.position, khaccach);
        Gizmos.DrawWireSphere(transform.position, ShootingRange);

    }
}
        
