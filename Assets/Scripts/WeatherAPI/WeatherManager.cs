using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;


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
    public string city = "London";
    public string apiKey = "caf8a5594d72e756e08740053486f79b";

    [Header("Game Environment")]
    // add whatever you want to use the weather api for, im guessing rain/sun idk

    // (SOMETHING MESSED UP HERE IM NOT SURE) private string apiUrl => $"https://api.openweathermap.org/data/2.5/weather?q={{city}}&appid={{apiKey}}&units=metric\";
}
