using OnlineStore.Shared.Clients;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Models;

[DataContract]
[XmlRoot("LoginResult")]
public class AuthResponse
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Email { get; set; }

    [DataMember]
    public string Token { get; set; }
    
    // [DataMember]
    // public UserPreferencesDto? Preferences { get; set; }
}