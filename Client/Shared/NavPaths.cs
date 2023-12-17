namespace OnlineStore.Client.Shared;

public static class NavPaths
{
    #region Users
    public const string UsersLogin = "/users/login";
    public const string UsersLogout = "/users/logout";
    public const string UsersRegister = "/accounts/register";
    public const string UsersManage = "/accounts/manage";
    public const string UsersSettings = "/clients/settings";
    public const string ClientsRegister = "/clients/register";

    #endregion

    #region Products

    public const string ProductsCreate = "/create";
    public const string ProductsManage = "/configuration";

    #endregion

    #region Orders
    
    public const string OrderManage = "/orders";
    public const string OrderCheckout = "/orders/checkout";
 
    #endregion
}
