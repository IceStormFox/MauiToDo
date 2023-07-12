using MongoDB.Bson;
using Realms;

namespace MauiToDo.Models
{
    public class Todo : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        [MapTo("owner")]
        public string Owner { get; set; }
        [MapTo("name")]
        [Required]
        public string Name { get; set; }
        [MapTo("completed")]
        public bool Completed { get; set; }
        [MapTo("_partition")]
        public string Partition { get; set; }


    }
}
