using Newtonsoft.Json;

namespace WebClientAPI
{
    class User
    {
        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "completed")]
        public bool Complete { get; set; }
    }
}
