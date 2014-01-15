using System.Text.RegularExpressions;

namespace PureSeeder.Core.Configuration
{
    public class GameInfo
    {
        public string GameName { get; set; }
        public string ProcessName { get; set; }
        public string WindowTitle { get; set; }
        public string FaultWindowTitle { get; set; }
        public Regex UrlMatch { get; set; }
    }
}