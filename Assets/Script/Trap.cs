using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float tocdoxoay;
    public float tocdodiChuyen;
    public Transform diemA; 
    public Transform diemB;
    private Vector3 diemMuctieu;
    
    private void Start()
    {
        diemMuctieu = diemA.position;    
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,diemMuctieu,tocdodiChuyen * Time.deltaTime);
        if(Vector3.Distance(transform.position, diemMuctieu) < 0.1f)
        {
            if(transform.position== diemA.position) 
            {
                diemMuctieu = diemB.position;
            }else
            {
                diemMuctieu = diemA.position;
            }
        }
    }
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, tocdoxoay);
    }
}
