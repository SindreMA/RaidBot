using System;
using Discord;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using UtilityBot.DTO;
using HtmlAgilityPack;
using System.Threading;
using Necessity.Modules;
using Discord.Addons.EmojiTools;
using Necessity.Dto;
using System.Linq;

namespace Necessity
{
    class CommandHandler
    {
        public System.Threading.Timer _timer;

        public static DiscordSocketClient _client;
        private CommandService _service;
        public static ulong OfficerChannel;
        public static List<AhitemLIstDTO> Itemlist = new List<AhitemLIstDTO>();
        static public List<PMRequest> Requests = new List<PMRequest>();
        static public List<UserPlayTime> PlayTimeList = new List<UserPlayTime>();
        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _timer = new System.Threading.Timer(Callback, true, 10000, System.Threading.Timeout.Infinite);

            _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += _client_MessageReceived;
            _client.ReactionAdded += _client_ReactionAdded;
            try
            {
                string json = File.ReadAllText("Requests.json");
                Requests = JsonConvert.DeserializeObject<List<PMRequest>>(json);
                foreach (var item in Requests)
                {
                    var Ttype = item.type;

                    var late = new LateRequest();
                    var attend = new NotAttendingRequest();
                    var message = new OMessageRequest();

                    if (item.type == late.GetType())
                    {
                        item.Request = JsonConvert.DeserializeObject<LateRequest>(item.Request.ToString());
                    }
                    else if (item.type == attend.GetType())
                    {
                        item.Request = JsonConvert.DeserializeObject<NotAttendingRequest>(item.Request.ToString());
                    }
                    else if (item.type == message.GetType())
                    {
                        item.Request = JsonConvert.DeserializeObject<OMessageRequest>(item.Request.ToString());
                    }

                }
            }
            catch (Exception) { }
            try
            {
                string json = File.ReadAllText("OfficerChannel.json");
                OfficerChannel = JsonConvert.DeserializeObject<ulong>(json);
            }
            catch (Exception) { }
        }
        private void Callback(Object state)
        {
            _timer.Change(2000, Timeout.Infinite);
            TimerEvent();
            
        }

        private void TimerEvent()
        {
            List<ulong> ReportedUsers = new List<ulong>();
            foreach (var guild in _client.Guilds)
            {
                foreach (var user in guild.Users)
                {
                    if (!ReportedUsers.Exists(x=> x == user.Id) && (user.Activity != null) && !user.IsBot)
                    {
                        if (PlayTimeList.Exists(x=> x.UserID == user.Id))
                        {
                            var USER = PlayTimeList.Find(x => x.UserID == user.Id);
                            if (!USER.GuildIDs.Exists(x => x == user.Guild.Id))
                            {
                                USER.GuildIDs.Add(user.Guild.Id);
                            }
                            if (USER.GameTime.Exists(x=> x.Game == user.Activity.Name))
                            {
                                var game = USER.GameTime.Find(x => x.Game == user.Activity.Name);
                                game.playedMin = game.playedMin + 1;
                                game.Updates.Add(DateTime.Now);
                            }
                            else
                            {
                                GameTime ne = new GameTime();
                                ne.Game = user.Activity.Name;
                                ne.playedMin = 1;
                                ne.Updates = new List<DateTime>();
                                ne.Updates.Add(DateTime.Now);
                                USER.GameTime.Add(ne);
                            }
                        }
                        else
                        {
                            var USER = new UserPlayTime();
                            USER.GameTime = new List<GameTime>();
                            USER.GuildIDs = new List<ulong>();
                            USER.GuildIDs.Add(user.Guild.Id);
                            USER.Username = user.Username;
                            USER.UserID = user.Id;
                            

                            GameTime ne = new GameTime();
                            ne.Game = user.Activity.Name;
                            ne.Updates = new List<DateTime>();
                            ne.Updates.Add(DateTime.Now);
                            ne.playedMin = 1;
                            USER.GameTime.Add(ne);

                            PlayTimeList.Add(USER);
                        }
                        ReportedUsers.Add(user.Id);
                    }
                }
            }
        }

        public void Save()
        {
            foreach (var item in Requests)
            {
                try
                {
                    item.type = item.Request.GetType();
                }
                catch (Exception){}
            }
            File.WriteAllText("Requests.json", JsonConvert.SerializeObject(CommandHandler.Requests,Formatting.Indented));
        }




        private async Task _client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {

            if (arg3.Emote.Name == "⏫" && !arg3.User.Value.IsBot)
            {
                if (Itemlist.Exists(x => x.MessageID == arg3.MessageId))
                {
                    var list = Itemlist.Find(x => x.MessageID == arg3.MessageId);

                    string URL = list.url;
                    //Find all videos that have been posted within 1 hour from channel
                    HtmlDocument doc = new HtmlDocument();
                    Thread.Sleep(100);
                    HtmlWeb hw = new HtmlWeb();
                    Thread.Sleep(100);
                    doc = hw.Load(URL);
                    string msg = "```" + Environment.NewLine;
                    int p = 0;
                    foreach (var item in list.List)
                    {
                        if (p < 5)

                        {
                            msg = msg + item.Text + " - " + item.Price + Environment.NewLine;
                            p++;
                        }

                    }
                    HtmlNodeCollection UpdatedNode = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]");
                    list.UpdatedString = UpdatedNode[0].ChildNodes[4].InnerText.Replace("\n", "");

                    var message = arg1.DownloadAsync().Result;
                    message.ModifyAsync(x =>
                    {
                        x.Embed = Command_Helper.SimpleEmbed(new Color(1f, 1f, 1f), list.Prefix, "**" + list.best + "**" + Environment.NewLine + "*" + list.UpdatedString + "*" + Environment.NewLine + msg.Replace("  ", "").Replace("- ", " = ") + "```" + Environment.NewLine + "Click the reaction to show all: ", list.url, "");
                    });
                    message.RemoveAllReactionsAsync();
                    Thread.Sleep(100);
                    message.AddReactionAsync(EmojiExtensions.FromText(":arrow_double_down:"));
                    Thread.Sleep(100);
                    message.AddReactionAsync(EmojiExtensions.FromText(":arrows_counterclockwise:"));

                }
            }
            else if (arg3.Emote.Name == "⏬" && !arg3.User.Value.IsBot)
            {
                if (Itemlist.Exists(x => x.MessageID == arg3.MessageId))
                {
                    var list = Itemlist.Find(x => x.MessageID == arg3.MessageId);

                    string URL = list.url;
                    //Find all videos that have been posted within 1 hour from channel
                    HtmlDocument doc = new HtmlDocument();
                    Thread.Sleep(100);
                    HtmlWeb hw = new HtmlWeb();
                    Thread.Sleep(100);
                    doc = hw.Load(URL);
                    string msg = "```" + Environment.NewLine;
                    int p = 0;
                    foreach (var item in list.List)
                    {
                        msg = msg + item.Text + " - " + item.Price + Environment.NewLine;
                        p++;
                    }
                    HtmlNodeCollection UpdatedNode = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]");
                    list.UpdatedString = UpdatedNode[0].ChildNodes[4].InnerText.Replace("\n", "");

                    var message = arg1.DownloadAsync().Result;
                    message.ModifyAsync(x =>
                    {
                        x.Embed = Command_Helper.SimpleEmbed(new Color(1f, 1f, 1f), list.Prefix, "**" + list.best + "**" + Environment.NewLine + "*" + list.UpdatedString + "*" + Environment.NewLine + msg.Replace("  ", "").Replace("- ", " = ") + "```" + Environment.NewLine + "Click the reaction to hide again: ", list.url, "");
                    });
                    message.RemoveAllReactionsAsync();
                    Thread.Sleep(100);
                    message.AddReactionAsync(EmojiExtensions.FromText(":arrow_double_up:"));
                    Thread.Sleep(100);
                    message.AddReactionAsync(EmojiExtensions.FromText(":arrows_counterclockwise:"));

                }
            }
            else if (arg3.Emote.Name == "🔄" && !arg3.User.Value.IsBot)
            {

                if (Itemlist.Exists(x => x.MessageID == arg3.MessageId))
                {
                    var list = Itemlist.Find(x => x.MessageID == arg3.MessageId);
                    string URL = list.url;
                    //Find all videos that have been posted within 1 hour from channel
                    HtmlDocument doc = new HtmlDocument();
                    Thread.Sleep(100);
                    HtmlWeb hw = new HtmlWeb();
                    Thread.Sleep(100);
                    doc = hw.Load(URL);
                    Thread.Sleep(100);
                    AhitemLIstDTO list2 = new AhitemLIstDTO();
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(@"//*[@id=""content""]/div/div[1]");
                    HtmlNodeCollection Iconnodes = doc.DocumentNode.SelectNodes(@"//*[@id=""content""]/div/div[2]");
                    HtmlNodeCollection dsdsds = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/h2");
                    HtmlNodeCollection UpdatedNode = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]");


                    Thread.Sleep(100);
                    List<AHItemDTO> AHitems = new List<AHItemDTO>();
                    string best = "";
                    HtmlNodeCollection ds = null;
                    try
                    {
                        int o = 0;
                        foreach (var item2 in ds = nodes[0].ChildNodes)
                        {
                            if (item2.Name == "div")
                            {
                                AHItemDTO AHitem = new AHItemDTO();
                                AHitem.Text = item2.InnerText.Replace("\n      ", "").Replace("\n ", "");
                                AHitem.Id = o;
                                int i = 0;
                                foreach (var item in Iconnodes[0].ChildNodes)
                                {
                                    if (item.Name == "div")
                                    {
                                        if (i == o)
                                        {
                                            AHitem.Price = item.InnerText.Replace("\n      ", "").Replace("\n ", "");
                                        }
                                        i++;
                                    }
                                }
                                o++;
                                AHitems.Add(AHitem);
                            }
                        }
                        foreach (var item in dsdsds[0].ChildNodes)
                        {
                            best = item.InnerText;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    string prefix = "Current values for EU-Silvermoon";
                    list.UpdatedString = UpdatedNode[0].ChildNodes[4].InnerText.Replace("\n", "");
                    string msg = "```" + Environment.NewLine;
                    int p = 0;
                    var message = arg1.DownloadAsync().Result;

                    if (message.Content.Length > 500)
                    {
                        foreach (var item in AHitems)
                        {

                            msg = msg + item.Text + " - " + item.Price + Environment.NewLine;
                            p++;


                        }
                    }
                    else
                    {
                        foreach (var item in AHitems)
                        {
                            if (p < 5)

                            {
                                msg = msg + item.Text + " - " + item.Price + Environment.NewLine;
                                p++;
                            }

                        }
                    }
                    message.ModifyAsync(x =>
                    {
                        x.Embed = Command_Helper.SimpleEmbed(new Color(1f, 1f, 1f), prefix, "**Refreshing!**", list.url, "");
                    });
                    Thread.Sleep(500);
                    message.ModifyAsync(x =>
                    {
                        x.Embed = Command_Helper.SimpleEmbed(new Color(1f, 1f, 1f), prefix, "**" + best + "**" + Environment.NewLine + "*" + list.UpdatedString + "*" + Environment.NewLine + msg.Replace("  ", "").Replace("- ", " = ") + "```" + Environment.NewLine + "Click the reaction to show more: ", list.url, "");
                    });

                    Itemlist.Remove(list);
                    Thread.Sleep(100);

                    list2.List = AHitems;
                    list2.MessageID = message.Id;
                    list2.best = best;
                    list2.Prefix = prefix;
                    list2.url = URL;
                    list2.msg = msg.Replace("  ", "").Replace("- ", " = ");
                    CommandHandler.Itemlist.Add(list2);
                }
            }
            else if (arg3.Channel is IPrivateChannel && !arg3.User.Value.IsBot)
            {
                var msg = arg2.GetCachedMessage(arg1.Id);
                PMProcess ds = new PMProcess();
                ds.Main(msg as SocketUserMessage, arg3);
            }

        }
        private async Task _client_MessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);
            int argPost = 0;
            if (msg.Channel is IPrivateChannel)
            {
                PMProcess ds = new PMProcess();
                ds.Main(msg, null);
                Save();
            }
            if (msg.HasCharPrefix('.', ref argPost))
            {
                var result = _service.ExecuteAsync(context, argPost);
                if (!result.Result.IsSuccess && result.Result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.Result.ErrorReason);
                }
                await Program.Log("Invoked " + msg + " in " + context.Channel + " with " + result.Result, ConsoleColor.Magenta);
            }
            else
            {
                await Program.Log(context.Channel + "-" + context.User.Username + " : " + msg, ConsoleColor.White);
            }

        }
    }
}
