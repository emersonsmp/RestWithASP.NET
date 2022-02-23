using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestWithASPNET.Data.VO
{
    public class PersonVO
    {  
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("nome")]
        public string FirstName { get; set; }

        [JsonPropertyName("sobrenome")]
        public string LastName { get; set; }

        [JsonPropertyName("endereco")]
        public string Address { get; set; }

        [JsonPropertyName("sexo")]
        public string Gender { get; set; }
    }
}
