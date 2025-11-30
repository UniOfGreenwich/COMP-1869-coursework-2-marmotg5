using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    [Header("Loading Bar")]
    public RectTransform bar;
    public float speed = 50f;
    public float startX = 400f;
    public float endX = -400f;

    [Header("Loading Scene")]
    public string LoadingScene;

    [Header("Load Time")]
    public float minLoadTime = 2f;//how long the loading screen will load for before switiching to main

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

        float timer = 0f;

        while (!op.isDone)
        {
            timer += Time.unscaledDeltaTime;

           // this will check if the load screen has stayed long enough so that it can switch to the sample scene
            if (op.progress >= 0.9f && timer >= minLoadTime)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
