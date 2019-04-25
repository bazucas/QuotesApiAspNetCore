using Newtonsoft.Json;
using System;

namespace QuotesApi.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
