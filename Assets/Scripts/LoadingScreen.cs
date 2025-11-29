using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    [Header("Loading Bar")]
    public RectTransform bar; 
    public float speed = 50f;// how fast the mushrooms scroll
    public float startX = 510f;// Where the mushrooms scrolls from
    public float endX = -510f;// this is the point at which the mushrroma loop back to the start

    [Header("LoadingScene")]
    public string LoadingScene; //Name of the scene

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private void Update()
    {
        MushroomScroll();
    }

    private void MushroomScroll()
    {
        if (bar == null) return;

        Vector2 pos = bar.anchoredPosition;
        pos.x -= speed * Time.unscaledDeltaTime;

        // once the mushrooms reach the end point, reset to start point
        if (pos.x <= endX)
        {
            pos.x = startX;
        }

        bar.anchoredPosition = pos;
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(LoadingScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            // Check if the load has finished
            if (op.progress >= 0.9f)
            {
                // Activate the scene
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}

