using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonNav : MonoBehaviour
{
    public void GoToScene3() // loads to the upcoming scene
    {
        SceneManager.LoadScene("Upcoming");
    }
}

}
