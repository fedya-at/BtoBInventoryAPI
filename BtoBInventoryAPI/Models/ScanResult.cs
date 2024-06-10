namespace BtoBInventoryAPI.Models
{
    public class ScanResult
    {
        public bool Success { get; set; }
        public  string Message { get; set; }
        public  Inventory Inventory { get; set; }
    }
}
