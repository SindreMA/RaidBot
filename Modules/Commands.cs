using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Necessity.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

        [Command("count")]
        public async Task Count()
        {
            if ((Context.User as SocketGuildUser).VoiceChannel != null)
            {


                var h = Context.Guild.GetUser(Context.User.Id);
                var users = (h.VoiceChannel as SocketVoiceChannel).Users;
                int i = users.Count;

                await Context.Channel.SendMessageAsync("There are " + i + " people in " + h.VoiceChannel.Name);
            }
            else
            {
                await Context.Channel.SendMessageAsync("You must be in a voice channel to use this!");
            }
        }
        [Command("Ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
        }
        [Command("gametime")]
        public async Task gametime([Optional] int days)
        {
            if (CommandHandler.PlayTimeList.Exists(x => x.UserID == Context.User.Id))
            {
                if (days == 0)
                {


                    var user = (CommandHandler.PlayTimeList.Find(x => x.UserID == Context.User.Id));
                    var eb = Command_Helper.SEmbed("GameTime", "Here are your 5 most played games!");


                    var top = user.GameTime.TopWithTies(5, x => x.playedMin).ToList();
                    foreach (var item in top)
                    {
                        var playtime = item.playedMin + " min";
                        if (item.playedMin > 60)
                        {
                            playtime = item.playedMin / 60 + " hours";
                        }
                        eb.AddField(item.Game, playtime);
                    }
                    Int64 allTime = 0;
                    foreach (var item in user.GameTime)
                    {
                        allTime = allTime + item.playedMin;
                    }
                    var allplaytime = allTime + " min";
                    if (allTime > 60)
                    {
                        allplaytime = allTime / 60 + " hours";
                    }
                    eb.Footer = new EmbedFooterBuilder();
                    eb.Footer.Text = allplaytime + " in total";


                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
                else
                {
                    var user = (CommandHandler.PlayTimeList.Find(x => x.UserID == Context.User.Id));
                    var eb = Command_Helper.SEmbed("GameTime", "Here are your 5 most played games from the last " + days +" days");
                    TimeSpan ts;

                    foreach (var item in user.GameTime)
                    {
                        foreach (var item2 in item.Updates.FindAll(z => DateTime.Now.Subtract(z).TotalDays < days))
                        {

                        }
                    }
                    var top = user.GameTime.TopWithTies(5, x => x.playedMin).ToList();
                    foreach (var item in top)
                    {
                        var playtime = item.playedMin + " min";
                        if (item.playedMin > 60)
                        {
                            playtime = item.playedMin / 60 + " hours";
                        }
                        eb.AddField(item.Game, playtime);
                    }
                    Int64 allTime = 0;
                    foreach (var item in user.GameTime)
                    {
                        allTime = allTime + item.playedMin;
                    }
                    var allplaytime = allTime + " min";
                    if (allTime > 60)
                    {
                        allplaytime = allTime / 60 + " hours";
                    }
                    eb.Footer = new EmbedFooterBuilder();
                    eb.Footer.Text = allplaytime + " in total";


                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Got not records saved of you!");
                
            }
        }
        [Command("alerthere")]
        public async Task alerthere()
        {
            CommandHandler.OfficerChannel = Context.Channel.Id;
            await Context.Channel.SendMessageAsync("", false, Util.Embed(Color.Blue, "Reports are now being sent here!", "").Build());
            File.WriteAllText("OfficerChannel.json", JsonConvert.SerializeObject(CommandHandler.OfficerChannel,Formatting.Indented));
        }
        [Command("ShowAllReports")]
        public async Task ShowAllReports()
        {
            var eb = Util.Embed(Color.Blue, "Reports");

            foreach (var report in CommandHandler.Requests)
            {
                if (report.isActive == false)
                {
                
                  

                    if (report.Request is LateRequest)
                    {
                        var re = report.Request as LateRequest;
                        eb.Description = eb.Description +
                            report.Username + " will be late by " + re.HowMuchLate.Text + " on raid-day " + re.RaidDate.Time.ToString().Split(' ')[0] +
                            Environment.NewLine;
                    }
                    else if (report.Request is NotAttendingRequest)
                    {
                        var re = report.Request as NotAttendingRequest;
                        eb.Description = eb.Description +
                            report.Username + " cant come to raid on raid-day " + re.RaidDate.Time.ToString().Split(' ')[0] +
                            Environment.NewLine;
                    }
                }
            }
            await Context.Channel.SendMessageAsync("", false, eb.Build());
           

        }


    }
}
