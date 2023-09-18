using Discord;
using Discord.Addons.EmojiTools;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Necessity
{
    public class Util
    {
        static public EmbedBuilder Embed(Color color, string title, string des = "")
        {
            var Cooler = Color.Red;

            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(color);
            eb.Title = title;
            eb.Description = des;
            return eb;
        }
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
        public static EmbedBuilder add2WeeksOfRaidsButton(EmbedBuilder eb)
        {
            var Monday = GetNextWeekday(DateTime.Now, DayOfWeek.Monday);
            var Tuesday = GetNextWeekday(DateTime.Now, DayOfWeek.Tuesday);
            var Thursday = GetNextWeekday(DateTime.Now, DayOfWeek.Thursday);

            eb.AddField("Monday(" + Monday.Date.ToString().Replace(" 00.00.00","") + ")", ":one:",true);
            eb.AddField("Tuesday(" + Tuesday.Date.ToString().Replace(" 00.00.00", "") + ")", ":two:",true);
            eb.AddField("Thursday(" + Thursday.Date.ToString().Replace(" 00.00.00", "") + ")", ":three:",true);
            eb.AddField("Monday(" + GetNextWeekday(Monday.AddDays(1), DayOfWeek.Monday).Date.ToString().Replace(" 00.00.00", "") + ")", ":four:",true);
            eb.AddField("Tuesday(" + GetNextWeekday(Tuesday.AddDays(1), DayOfWeek.Tuesday).Date.ToString().Replace(" 00.00.00", "") + ")", ":five:",true);
            eb.AddField("Thursday(" + GetNextWeekday(Thursday.AddDays(1), DayOfWeek.Thursday).Date.ToString().Replace(" 00.00.00", "") + ")", ":six:",true);

            return eb;
        }
        public static async Task AddReactionsSixdaysReactions(RestUserMessage msg)
        {
            await msg.AddReactionAsync(EmojiExtensions.FromText(":one:"));
            Thread.Sleep(100);
            await msg.AddReactionAsync(EmojiExtensions.FromText(":two:"));
            Thread.Sleep(100);
            await msg.AddReactionAsync(EmojiExtensions.FromText(":three:"));
            Thread.Sleep(100);
            await msg.AddReactionAsync(EmojiExtensions.FromText(":four:"));
            Thread.Sleep(100);
            await msg.AddReactionAsync(EmojiExtensions.FromText(":five:"));
            Thread.Sleep(100);
            await msg.AddReactionAsync(EmojiExtensions.FromText(":six:"));

        }
        static public DateTime getDate(string emote)
        {

            var Monday = GetNextWeekday(DateTime.Now, DayOfWeek.Monday);
            var Tuesday = GetNextWeekday(DateTime.Now, DayOfWeek.Tuesday);
            var Thursday = GetNextWeekday(DateTime.Now, DayOfWeek.Thursday);

            if (emote == "1⃣")
            {
                return Monday;
            }
            else if (emote == "2⃣")
            {
                return Tuesday;
            }
            else if (emote == "3⃣")
            {
                return Thursday;
            }
            else if (emote == "4⃣")
            {
                return GetNextWeekday(Monday.AddDays(1), DayOfWeek.Monday);

            }
            else if (emote == "5⃣")
            {
                return GetNextWeekday(Tuesday.AddDays(1), DayOfWeek.Tuesday);

            }
            else if (emote == "6⃣")
            {
                return GetNextWeekday(Thursday.AddDays(1), DayOfWeek.Thursday);

            }
            else
            {
                return GetNextWeekday(DateTime.Now, DayOfWeek.Saturday);
            }

        }
        public static async Task SendOfficerMessage(DiscordSocketClient _client, PMRequest request)
        {
            try
            {
                var channel = _client.GetChannel(CommandHandler.OfficerChannel) as SocketTextChannel;

                var eb = Embed(Color.Blue, "New report from " + request.Username, "** **");
                if (request.Request is LateRequest)
                {
                    var req = request.Request as LateRequest;
                    eb.Description = "User will be late for a raid!";
                    eb.AddField("Will be late by",req.HowMuchLate.Text,true);
                    eb.AddField("Raid Day", req.RaidDate.Time.ToString().Split(' ')[0],true);
                    eb.AddField("Reason", req.Reason.Text);
                }
                else if (request.Request is NotAttendingRequest)
                {
                    var req = request.Request as NotAttendingRequest;
                    eb.Description = "User cant come to raid!";
                    eb.AddField("Raid Day", req.RaidDate.Time.ToString().Split(' ')[0]);
                    eb.AddField("Reason", req.Reason.Text);
                }
                else if (request.Request is OMessageRequest)
                {
                    var req = request.Request as OMessageRequest;
                    eb.Description = "User left a message for officers!";
                    eb.Title = "New message from " + request.Username;
                    eb.AddField("Message : ", req.Text);
                }
                await channel.SendMessageAsync("",false,eb.Build());
            }
            catch (Exception)
            {
                await Program.Log("Cant find channel", ConsoleColor.Red);
            }
            

        }
    }
}
