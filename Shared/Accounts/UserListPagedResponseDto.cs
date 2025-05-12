using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OnlineStore.Shared.Accounts;

[DataContract]
[XmlRoot("GetUsersResult")]
public class UserListPagedResponseDto
{
    [DataMember]
    public List<UserDto> Items { get; set; }

    [DataMember]
    public int PageNumber { get; set; }

    [DataMember]
    public int PageSize { get; set; }

    [DataMember]
    public int TotalPages { get; set; }

    [DataMember]
    public int TotalItemsCount { get; set; }

    public UserListPagedResponseDto()
    {
        
    }
    
    public UserListPagedResponseDto(
        List<UserDto> items, 
        int pageNumber, 
        int pageSize, 
        int totalPages, 
        int totalItemsCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalItemsCount = totalItemsCount;
    }
}