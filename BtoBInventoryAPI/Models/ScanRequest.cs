namespace BtoBInventoryAPI.Models
{
    public class ScanRequest
    {
        public required string TagId { get; set; }
        public required string TagType { get; set; }
    }
}
