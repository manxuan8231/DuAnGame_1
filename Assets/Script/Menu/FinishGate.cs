using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishGate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            UnlockNewlevels();
            SceneController.instance.NextLevel1();
        }   

    }
    void UnlockNewlevels()
    {
        if(SceneManager.GetActiveScene().buildIndex>=PlayerPrefs.GetInt("RechedIndex"))
        {
            PlayerPrefs.SetInt("RechedIndex",SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }    
    }    
}
