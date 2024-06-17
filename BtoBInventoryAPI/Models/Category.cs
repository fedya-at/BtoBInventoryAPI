using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BtoBInventoryAPI.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("NameCategory")]
        public string NameCategory { get; set; }

        [BsonElement("DescriptionCategory")]
        public string DescriptionCategory { get; set; }

        [BsonElement("ImageUrlCategory")]
        public string ImageUrlCategory { get; set; }

    }
}
