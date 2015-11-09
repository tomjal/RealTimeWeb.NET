using Soloco.ReactiveStarterKit.Common.Infrastructure.Messages;

namespace Soloco.ReactiveStarterKit.Membership.Messages.Queries
{
    public class ValidUserLoginQuery : IMessage<bool>
    {
        public string UserName { get; }
        public string Password { get; }

        public ValidUserLoginQuery(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}