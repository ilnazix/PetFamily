using AutoFixture;
using PetFamily.SharedKernel.ValueObjects.Ids;

namespace PetFamily.Discussions.Domain.UnitTests.Extensions;

internal static class FixtureExtensions
{
    public static Discussion CreateDiscussion(
        this IFixture fixture)
    {
        var id = DiscussionId.NewDiscussionId();
        
        var relationId = Guid.NewGuid();

        var users = Enumerable.Range(0, 2).Select(_ => fixture.CreateUser());

        var result = Discussion.Create(id, relationId, users).Value;

        return result;
    }

    public static Message CreateMessageWithUserId(
        this IFixture fixture,
        Guid userId)
    {
        var id = MessageId.NewMessageId();
        var text = fixture.Create<string>();

        var result = Message.Create(id, userId, text).Value;

        return result;
    }

    public static User CreateUser(
        this IFixture fixture)
    {
        var id = Guid.NewGuid();
        var firstName = fixture.Create<string>();
        var lastName = fixture.Create<string>();

        var result = User.Create(id, firstName, lastName).Value;

        return result;
    }

    public static Discussion CreateDiscussionWithMessage(
        this IFixture fixture)
    {
        var discussion = fixture.CreateDiscussion();
        var userId = discussion.Users.First().Id;
        var message = fixture.CreateMessageWithUserId(userId);

        discussion.AddMessage(message);

        return discussion;
    }
}
