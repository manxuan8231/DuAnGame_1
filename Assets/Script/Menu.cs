using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   public void Choi()
    {
        SceneManager.LoadScene(1);
    }
   public void thoatramenu()
    {
        SceneManager.LoadScene(0);
    }    
    public void Thoat()
    {
      Application.Quit();
    }

}
