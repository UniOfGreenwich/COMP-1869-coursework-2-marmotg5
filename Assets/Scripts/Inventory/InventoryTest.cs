using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    private InventorySystem playerInventory;    // testing to see if it works, ignore

    private void Start()
    {
        playerInventory = GetComponent<InventorySystem>();

        Item mushroom = new Item("Mushroom", 1);
        Item spore = new Item("Spore", 2);

        playerInventory.AddItem(mushroom);
        playerInventory.AddItem(spore);
        playerInventory.ShowInventory();
        playerInventory.RemoveItem(mushroom);
        playerInventory.ShowInventory();
        playerInventory.RemoveItem(spore);
        playerInventory.ShowInventory();
    }
}
