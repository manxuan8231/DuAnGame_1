using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Trapground : MonoBehaviour
{
    public float x , y,z;
    public GameObject pipePrefab; 
    public List<GameObject> pipes;
    public float timeToCreatePipes;
    int numberOfPipes = 0;
    void Start()
    {
        timeToCreatePipes = 2f;
    }
    void Update()
    {
        timeToCreatePipes -= Time.deltaTime;
        if (timeToCreatePipes <= 0)
        {
            CreateRandomPipes();
            timeToCreatePipes = 2f;
        }
    }
    private void CreateRandomPipes()
    {

        
        var randomY = Random.Range(-3f, 3f);
        var pipe = Instantiate(pipePrefab,new Vector3(x, randomY , 0), Quaternion.identity);
        Destroy(pipe, 10);
        MovePipes(pipe);
    }
    private void MovePipes(GameObject pipe)
    {
        pipe.GetComponent<Rigidbody2D>().velocity = Vector2.left * 2;
    }
}
