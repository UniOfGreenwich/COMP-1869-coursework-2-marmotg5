using UnityEngine;

[System.Serializable]
public class Item   // Defining what an 'Item' is and giving it an ID
{
    public string itemName;
    public int itemID;

    public Item(string name, int id)
    {
        itemName = name;
        itemID = id;
    }
}
