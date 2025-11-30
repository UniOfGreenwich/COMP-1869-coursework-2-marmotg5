using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Prefixes")]
    [SerializeField] string cashPrefix = "Cash: $";
    [SerializeField] string levelPrefix = "Level: ";

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI cashText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image levelForegroundImage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();    
    }

    void UpdateUI()
    {
        if (GameManager.player == null) return;
        Player player = GameManager.player;
        Level playerLevel = player.GetLevel();

        cashText.text = cashPrefix + player.GetCash();
        levelText.text = levelPrefix + playerLevel.levelAmount;
        experienceText.text = playerLevel.experience + "/" + playerLevel.maxExperience;
    }
}
