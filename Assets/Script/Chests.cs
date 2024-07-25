using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chests : MonoBehaviour
{
    Animator animator;
    public GameObject itemPrefab; // Prefab của vật phẩm
    public Transform itemSpawnPoint; // Điểm xuất hiện của vật phẩm   
    private bool isOpen = true;
    private bool isMovingItem = false;
    private GameObject spawnedItem;
    private Vector3 itemStartPos;
    private Vector3 itemEndPos;
    private float itemMoveDuration = 1f; // Thời gian vật phẩm bay lên
    private float itemMoveElapsed = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingItem && spawnedItem != null) // Kiểm tra nếu item còn tồn tại
        {
            itemMoveElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(itemMoveElapsed / itemMoveDuration);
            spawnedItem.transform.position = Vector3.Lerp(itemStartPos, itemEndPos, t);

            if (t >= 1.0f)
            {
                isMovingItem = false;
                
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isOpen)
        {
            animator.SetTrigger("isOpen");
            isOpen = false;
            spawnedItem = Instantiate(itemPrefab, itemSpawnPoint.position, Quaternion.identity);
            itemStartPos = spawnedItem.transform.position;
            itemEndPos = itemStartPos + new Vector3(0, 1f, 0); //vi tri bay len
            isMovingItem = true;
            itemMoveElapsed = 0; // Đặt lại thời gian đã trôi qua cho chuyển động

        }
    }
}
