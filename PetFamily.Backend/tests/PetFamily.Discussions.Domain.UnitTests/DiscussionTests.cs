using AutoFixture;
using FluentAssertions;
using PetFamily.Discussions.Domain.UnitTests.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Domain.UnitTests;

public class DiscussionTests
{
    private readonly IFixture _fixture;

    public DiscussionTests()
    {
        _fixture = new Fixture(); 
    }

    [Fact]
    public void AddMessage_ValidUser_ReturnsSuccessResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussion();
        var userId = sut.Users.First().Id;
        var message = _fixture.CreateMessageWithUserId(userId);

        //Act
        var result = sut.AddMessage(message);

        //Assert
        result.IsSuccess.Should().BeTrue();
        sut.Messages.Should().HaveCount(1);
    }

    [Fact]
    public void AddMessage_InvalidUser_ReturnsFailureResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussion();
        var userId = Guid.NewGuid();
        var message = _fixture.CreateMessageWithUserId(userId);

        //Act
        var result = sut.AddMessage(message);

        //Assert
        result.IsSuccess.Should().BeFalse();
        sut.Messages.Should().HaveCount(0);
    }

    [Fact]
    public void EditMessage_ValidData_ReturnsSuccessResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussionWithMessage();
        var message = sut.Messages.First();
        var messageId = message.Id;
        var userId = message.UserId;
        var newText = "Edited message text";
        var oldText = message.Text;

        //Act
        var result = sut.EditMessage(userId, messageId, newText);

        //Assert
        result.IsSuccess.Should().BeTrue();
        message.Text.Should().Be(newText);
        message.Text.Should().NotBe(oldText);
        message.IsEdited.Should().BeTrue();
    }

    [Fact]
    public void EditMessage_InvalidUserId_ReturnsFailureResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussionWithMessage();
        var message = sut.Messages.First();
        var messageId = message.Id;
        var userId = Guid.NewGuid();
        var newText = "Edited message text";
        var oldText = message.Text;

        //Act
        var result = sut.EditMessage(userId, messageId, newText);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Message.EditNotAllowed());
        message.Text.Should().Be(oldText);
        message.Text.Should().NotBe(newText);
    }

    [Fact]
    public void EditMessage_InvalidMessageId_ReturnsFailureResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussionWithMessage();
        var message = sut.Messages.First();
        var messageId = MessageId.NewMessageId();
        var userId = message.UserId;
        var newText = "Edited message text";

        //Act
        var result = sut.EditMessage(userId, messageId, newText);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.NotFound());
    }

    [Fact]
    public void EditMessage_EmptyMessage_ReturnsFailureResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussionWithMessage();
        var message = sut.Messages.First();
        var messageId = message.Id;
        var userId = message.UserId;
        var newText = string.Empty;

        //Act
        var result = sut.EditMessage(userId, messageId, newText);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.Message.MessageTextRequired());
    }

    [Fact]
    public void DeleteMessage_ValidData_DeletesMessage()
    {
        //Arrange
        var sut = _fixture.CreateDiscussionWithMessage();
        var message = sut.Messages.First();
        var messageId = message.Id;
        var userId = message.UserId;

        //Act
        var result = sut.DeleteMessage(userId, messageId);

        //Assert
        result.IsSuccess.Should().BeTrue();
        sut.Messages.Should().NotContain(message);
    }

    [Fact]
    public void DeleteMessage_InvalidUserId_ReturnsFailureResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussionWithMessage();
        var message = sut.Messages.First();
        var messageId = message.Id;
        var userId = Guid.NewGuid();

        //Act
        var result = sut.DeleteMessage(userId, messageId);

        //Assert
        result.IsFailure.Should().BeTrue();
        sut.Messages.Should().Contain(message);
    }

    [Fact]
    public void DeleteMessage_InvalidMessageId_ReturnsFailureResult()
    {
        //Arrange
        var sut = _fixture.CreateDiscussionWithMessage();
        var message = sut.Messages.First();
        var messageId = MessageId.NewMessageId();
        var userId = message.UserId;

        //Act
        var result = sut.DeleteMessage(userId, messageId);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Errors.General.NotFound());
        sut.Messages.Should().Contain(message);
    }
}