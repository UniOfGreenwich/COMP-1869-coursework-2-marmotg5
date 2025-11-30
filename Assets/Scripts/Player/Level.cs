using UnityEngine;

[System.Serializable]
public class Level
{
    // Starting level consts
    const int STARTING_LEVEL = 1;
    const int STARTING_EXPERIENCE = 0;

    public int levelAmount { get; private set; }
    public int experience { get; private set; }
    public int maxExperience { get; private set; }

    public Level()
    {
        levelAmount = STARTING_LEVEL;
        experience = STARTING_EXPERIENCE;
        maxExperience = CalculateRequiredExperience(levelAmount);
    }

    // Called to create theg level for the player when loading the player data from the save file
    public Level(int levelAmount, int experience)
    {
        this.levelAmount = levelAmount;
        this.experience = experience;
        this.maxExperience = CalculateRequiredExperience(levelAmount);
    }

    void LevelUp()
    {
        levelAmount++;
        experience = 0;
        maxExperience = CalculateRequiredExperience(levelAmount);
    }

    public void AddExperience(int amount)
    {
        experience += amount;

        if (experience >= maxExperience)
        {
            int leftOverXp = experience - maxExperience; // Check if we get/add more experience than the max and store it
            LevelUp();
            experience += leftOverXp; // Add the left over xp after leveling up
        }
    }

    int CalculateRequiredExperience(int level)
    {
        return (100 * (level * level)) + (50 * level);
    }
}
