using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_nav: MonoBehaviour
{
    public string sceneName = Upcoming ;

    public void LoadScene() // this is what will make the button load to another scene when clicked.
    {
        SceneManager.LoadScene(sceneName);
    }
}
