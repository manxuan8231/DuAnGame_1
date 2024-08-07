using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Memory : MonoBehaviour
{
    Player _player;
    public GameObject _showCanvas;
    [SerializeField] TextMeshProUGUI _highestScore;
    [SerializeField] TextMeshProUGUI _highestTime;
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
        var time = _player.GetTime();
        var maxScore = Mathf.Max(score);
        var maxtime = Mathf.Max(time);
        _highestScore.text = $"Best: {maxScore}";
        _highestTime.text = $"Time: {maxtime:0.00}";
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
