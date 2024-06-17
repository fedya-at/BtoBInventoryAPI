using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BtoBInventoryAPI.Models
{
    public class Export
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("ExportDate")]
        public DateTime ExportDate { get; set; }
    }
}
