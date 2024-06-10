using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtoBInventoryAPI.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Stock")]
        public int Stock { get; set; }

        [BsonElement("TagId")]
        public string TagId { get; set; }
    }
}
