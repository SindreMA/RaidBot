using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityBot.DTO
{
    public class GuildAndLog
    {
        public SocketGuild Guild { get; set; }
        public SocketTextChannel LogChannel { get; set; }
    }
}
