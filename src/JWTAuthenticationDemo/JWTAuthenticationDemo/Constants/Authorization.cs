namespace JWTAuthenticationDemo.Constants;

public class Authorization
{
    public enum Roles
    {
        Administrator,
        Moderator,
        User
    }
    public const string DefaultFirstName = "userFirstName";
    public const string DefaultLastName = "userLastName";
    public const string DefaultUsername = "user";
    public const string DefaultEmail = "user@secureapi.com";
    public const string DefaultPassword = "Pa$$w0rd.";
    public const Roles DefaultRole = Roles.User;
}