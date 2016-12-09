using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClientAPI
{
    public class Post
    {
        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
    }
}
