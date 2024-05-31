namespace BtoBInventoryAPI.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public DateTime LastChecked { get; set; }
        public bool IsNfcTag { get; set; }
    }
}
