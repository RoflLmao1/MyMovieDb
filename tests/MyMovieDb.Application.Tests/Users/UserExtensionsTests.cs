using System.Security.Claims;

namespace MyMovieDb.Users;

public class UserExtensionsTests : MyMovieDbApplicationTestBase<MyMovieDbApplicationTestModule>
{
    [Fact]
    public void GetClientId_ShouldReturnValue()
    {
        // Arrange
        string claimClientId = Guid.NewGuid().ToString();
        var claims = new Claim[]
        {
            new Claim(UserExtensions.CLAIM_TYPE_CLIENT_ID, claimClientId)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "Bearer"));

        // Act
        string? clientId = claimsPrincipal.GetClientId();

        // Assert
        Assert.Equal(claimClientId, clientId);
    }

    [Fact]
    public void GetClientId_NoClientIdClaim_ShouldReturnNull()
    {
        // Arrange
        string claimClientId = Guid.NewGuid().ToString();
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity("Bearer"));

        // Act
        string? clientId = claimsPrincipal.GetClientId();

        // Assert
        Assert.Null(clientId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GetClientId_ReturnNull(string claimClientId)
    {
        // Arrange
        var claims = new Claim[]
        {
            new Claim(UserExtensions.CLAIM_TYPE_CLIENT_ID, claimClientId)
        };
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "Bearer"));

        // Act
        string? clientId = claimsPrincipal.GetClientId();

        // Assert
        Assert.Null(clientId);
    }
}
