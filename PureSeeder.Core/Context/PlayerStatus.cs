namespace PureSeeder.Core.Context
{
    public class PlayerStatus
    {
        private string _username;
        private string _currentServer;

        public PlayerStatus(string username, string currentServer)
        {
            _username = username;
            _currentServer = currentServer;
        }

        public string Username { get { return _username; }}
        public string CurrentServer { get { return _currentServer; } }
    }
}