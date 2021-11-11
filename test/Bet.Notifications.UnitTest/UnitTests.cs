using Bet.Notifications.Abstractions.Smtp;

namespace Bet.Notifications.UnitTest;

public class UnitTests
{
    [Fact]
    public void Address_Are_The_Same()
    {
        var email1 = new Address("email@gmail.com");
        var email2 = new Address("email@gmail.com");

        Assert.Equal(email1, email2);
    }

    [Fact]
    public void Address_Are_Not_The_Same()
    {
        var email1 = new Address("email1@gmail.com");
        var email2 = new Address("email2@gmail.com");

        Assert.NotEqual(email1, email2);
    }

    [Fact]
    public void Split_Address_Test()
    {
        var email = EmailConfigurator
        .From("test@test.com")
        .To("james@test.com;john@test.com", "James 1;John 2");

        Assert.Equal(2, email.Message.To.Count);
        Assert.Equal("james@test.com", email.Message.To[0].Email);
        Assert.Equal("john@test.com", email.Message.To[1].Email);
        Assert.Equal("James 1", email.Message.To[0].Name);
        Assert.Equal("John 2", email.Message.To[1].Name);
    }

    [Fact]
    public void Replace_Render_Test()
    {
        var template = "Shalom ##Name##";

        var email = EmailConfigurator
            .From("from@email.com")
            .To("to@email.com")
            .Subject("Test message")
            .UsingTemplate(template, new { Name = "John the Immerser" });

        Assert.Equal("Shalom John the Immerser", email.Message.Body);
    }
}
