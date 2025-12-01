using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_nav: MonoBehaviour
{
    public string sceneName;
    // thid is a scene manager to load the bell into the uncoming scene and then back again
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
