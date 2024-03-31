using UnityEngine;
using UnityEngine.SceneManagement;

public class ImputManager : MonoBehaviour
{

    void Update()
    {
    
        if(Input.GetKeyUp(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            Application.Quit();
        }
    }
}
