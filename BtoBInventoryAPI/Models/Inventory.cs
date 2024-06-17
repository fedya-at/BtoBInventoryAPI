using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtoBInventoryAPI.Models
{
    public class Inventory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

   
        [BsonElement("ProductId")]
        public int ProductId { get; set; }

        [BsonElement("Product")]
        public Product Product { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Category")]
        public Category Category { get; set; }

        [BsonElement("ScanNFC")]
        public string ScanNFC { get; set; }
    }
}
