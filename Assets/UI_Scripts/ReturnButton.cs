using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour

    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Clickme1()
        {
            SceneManager.LoadScene(1);
        }

    }


