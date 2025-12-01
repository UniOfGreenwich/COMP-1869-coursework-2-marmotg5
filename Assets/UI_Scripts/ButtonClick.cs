using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Clickme()
    {
        SceneManager.LoadScene(2);
    }

}
