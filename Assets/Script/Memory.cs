using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Memory : MonoBehaviour
{
    Player _player;
    public GameObject _showCanvas;
    [SerializeField] TextMeshProUGUI _highestScore;
    void Start()
    {
        //tìm đối tượng player(lấy điểm)
        _player = FindObjectOfType<Player>();
    }

    
    void Update()
    {
        
    }
    private void ShowData()
    {
        var score = _player.GetScore();
        var maxScore = Mathf.Max(score);
        _highestScore.text = $"Best: {maxScore}";
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowData();
            _showCanvas.SetActive(true);
            Time.timeScale =0f;
        }
    }
}
