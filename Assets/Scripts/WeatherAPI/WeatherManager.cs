using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

// Classes
[Serializable]
public class WeatherInfo
{
    public WeatherMain main;
    public WeatherDescription[] weather;
}

[Serializable]
public class WeatherMain
{
    public float temp;
    public float humidity;
}

[Serializable]
public class WeatherDescription
{
    public string main;
    public string description;
}

public class WeatherManager : MonoBehaviour
{
    [Header("API Settings")]
    public string city = "London";  // City to request data for
    public string apiKey = "caf8a5594d72e756e08740053486f79b";  // API key from OpenWeatherMap

    // Potential changes we'd use the API for, can change to whatever, just examples
    [Header("Game Environment")]
    public Light sunLight;
    public ParticleSystem rainEffect;

    private string apiUrl => $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

    public string weatherCondition { get; private set; } = "";

    private void Start()
    {
        StartCoroutine(GetWeatherData());
    }

    private IEnumerator GetWeatherData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();
            // Parser for JSON
            if (request.result == UnityWebRequest.Result.Success)
            {
                WeatherInfo weatherInfo = JsonUtility.FromJson<WeatherInfo>(request.downloadHandler.text);
                UpdateEnvironment(weatherInfo);
            }
            else
            {
                Debug.LogError("Weather API Error: " + request.error);
            }
        }

        // Refresh every 5 minutes (we don't have to do this but I thought why not)
        yield return new WaitForSeconds(300f);
        StartCoroutine(GetWeatherData());
    }

    private void UpdateEnvironment(WeatherInfo info)
    {
        //string condition = info.weather[0].main.ToLower();
        weatherCondition = info.weather[0].main.ToLower();
        float temp = info.main.temp;

        Debug.Log($"Weather: {weatherCondition}, Temp: {temp} celcius");

        // Adjust sunlight intensity depending on the weather
        if (weatherCondition.Contains("cloud"))
            sunLight.intensity = 0.9f;
        else if (weatherCondition.Contains("rain"))
            sunLight.intensity = 0.7f;
        else
            sunLight.intensity = 1.0f;

        // Toggle the rain effect on/off depending on the weather
        if (weatherCondition.Contains("rain"))
        {
            if (!rainEffect.isPlaying)
                rainEffect.Play();
        }
        else
        {
            if (rainEffect.isPlaying)
                rainEffect.Stop();
        }
    }
}
    