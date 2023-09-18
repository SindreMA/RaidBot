using System;
using System.Collections.Generic;
using System.Text;

namespace Necessity.Dto
{

    public class UserPlayTime
    {
        public ulong UserID { get; set; }
        public List<ulong> GuildIDs { get; set; }
        public string Username { get; set; }
        public List<GameTime> GameTime { get; set; }
    }
    public class GameTime
    {
        public string Game { get; set; }
        public Int64 playedMin { get; set; }
        public List<DateTime> Updates { get; set; }
    }
}
 

