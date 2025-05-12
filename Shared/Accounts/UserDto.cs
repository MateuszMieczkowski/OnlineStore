using OnlineStore.Shared.Enums;
using System.Runtime.Serialization;

namespace OnlineStore.Shared.Accounts;

[DataContract]
public class UserDto
{
    public UserDto(int id, string email, UserRoleDto role)
    {
        Id = id;
        Email = email;
        Role = role;
    }

    public UserDto() { }
    
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Email { get; set; }

    [DataMember]
    public UserRoleDto Role { get; set; }
}