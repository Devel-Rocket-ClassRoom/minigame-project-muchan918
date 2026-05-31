public class SaveSlotData
{
    public string ItemID { get; set; }
    public int Amount { get; set; }

    public SaveSlotData() { }

    public SaveSlotData(string itemID, int amount)
    {
        ItemID = itemID;
        Amount = amount;
    }
}
