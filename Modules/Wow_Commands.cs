using Discord;
using Discord.Addons.EmojiTools;
using Discord.Commands;
using Discord.WebSocket;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityBot.DTO;
using UtilityBot.Modules.Dto;

namespace Necessity.Modules
{
    public class Wow_Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ah")]
        [Alias("BloodToAH", "bosconversion", "bosprices", "bloodconversion", "bloodofsargeras")]
        public async Task ah([Optional]string realm)
        {
            string URL = "https://rodent.io/blood-money-eu/silvermoon";
            //Find all videos that have been posted within 1 hour from channel
            if (realm != null)
            {
                URL = "https://rodent.io/blood-money-eu/" + realm;
            }

            HtmlDocument doc = new HtmlDocument();
            Thread.Sleep(100);
            HtmlWeb hw = new HtmlWeb();
            Thread.Sleep(100);
            doc = hw.Load(URL);
            Thread.Sleep(100);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(@"//*[@id=""content""]/div/div[1]");
            HtmlNodeCollection Iconnodes = doc.DocumentNode.SelectNodes(@"//*[@id=""content""]/div/div[2]");
            HtmlNodeCollection UpdatedNode = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]");

            HtmlNodeCollection dsdsds = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/h2");

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
            if (realm != null)
            {
                prefix = "Current values for EU-" + realm;
            }
            string msg = "```" + Environment.NewLine;
            int p = 0;
            foreach (var item in AHitems)
            {
                if (p < 5)
                {
                    msg = msg + item.Text + " - " + item.Price + Environment.NewLine;
                    p++;
                }
            }
            AhitemLIstDTO list = new AhitemLIstDTO();
            list.UpdatedString = UpdatedNode[0].ChildNodes[4].InnerText.Replace("\n", "");
            var message = await Context.Channel.SendMessageAsync("", false, Command_Helper.SimpleEmbed(new Color(1f, 1f, 1f), prefix, "**" + best + "**" + Environment.NewLine + "*" + list.UpdatedString + "*" + Environment.NewLine + msg.Replace("  ", "").Replace("- ", " = ") + "```" + Environment.NewLine + "Click on the reaction to show more:", URL, ""));
            list.List = AHitems;
            list.MessageID = message.Id;
            list.best = best;
            list.Prefix = prefix;
            list.url = URL;
            list.msg = msg.Replace("  ", "").Replace("- ", " = ");
            CommandHandler.Itemlist.Add(list);
            await message.AddReactionAsync(EmojiExtensions.FromText(":arrow_double_down:"));
            Thread.Sleep(100);
            await message.AddReactionAsync(EmojiExtensions.FromText(":arrows_counterclockwise:"));
        }
        public async Task armory(string user, [Optional]string realm)
        {
            string Realm = "Silvermoon";
            if (realm == null || realm == "")
            {
            }
            else { Realm = realm; }

            string t = @"Armory for " + user + @" = http://eu.battle.net/wow/en/character/" + Realm + @"/" + user + @"/simple";

            await Context.Channel.SendMessageAsync(t);
        }
        [Command("affix")]
        public async Task GetAffix()
        {
            await Context.Channel.TriggerTypingAsync();
            var h = await Command_Helper.DownloadString("https://affixes.wisak.eu");
            var j = Command_Helper.StringFinder(h, "<h2>This Week</h2>" + Command_Helper.all + "</div>");
            var jd = Command_Helper.StringFinder(h, "<h2>Next Week</h2>" + Command_Helper.all + "</div>");

            List<int> NextAffixes = new List<int>();
            foreach (var item in Command_Helper.SplitOnString(jd, "affix="))
            {
                try
                {
                    NextAffixes.Add(int.Parse(item.Split('/')[0]));
                }
                catch (Exception)
                {
                }

            }

            List<int> Affixes = new List<int>();
            foreach (var item in Command_Helper.SplitOnString(j, "affix="))
            {
                try
                {
                    Affixes.Add(int.Parse(item.Split('/')[0]));
                }
                catch (Exception)
                {
                }

            }
            int p = 0;
            List<string> w = new List<string>();
            string Des = "";
            Des = Des + Environment.NewLine + "This Week:" + Environment.NewLine;
            foreach (var item in Affixes)
            {
                p++;

                var l = await Command_Helper.DownloadString("http://www.wowhead.com/affix=" + item);
                var u = Command_Helper.StringFinder(l, "tle>" + Command_Helper.all + " - My");
                string at = "";
                if (p == 1)
                {
                    at = "(4) ";
                }
                else if (p == 2)
                {
                    at = "(7) ";
                }
                else if (p == 3)
                {
                    at = "(10) ";
                }
                if (at.Contains("(4)") || at.Contains("(7)") || at.Contains("(10)"))
                {
                    Des = Des + "[" + at + u + "](http://www.wowhead.com/affix=" + item + ")" + Environment.NewLine;

                }


            }
            Des = Des + Environment.NewLine + "Next Week:" + Environment.NewLine;
            p = 0;
            foreach (var item in NextAffixes)
            {
                p++;

                var l = await Command_Helper.DownloadString("http://www.wowhead.com/affix=" + item);
                var u = Command_Helper.StringFinder(l, "tle>" + Command_Helper.all + " - My");
                string at = "";
                if (p == 1)
                {
                    at = "(4) ";
                }
                else if (p == 2)
                {
                    at = "(7) ";
                }
                else if (p == 3)
                {
                    at = "(10) ";
                }
                if (at.Contains("(4)") || at.Contains("(7)") || at.Contains("(10)"))
                {
                    Des = Des + "[" + at + u + "](http://www.wowhead.com/affix=" + item + ")" + Environment.NewLine;
                }
            }
            await Context.Channel.SendMessageAsync("", false, Command_Helper.SimpleEmbed(new Color(1f, 1f, 1f), "Affixes", Des));
        }
        [Command("wow")]
        public async Task wow(string Player, [Optional]string realm)
        {
            string Realm = "Silvermoon";
            if (realm == null || realm == "")
            {
            }
            else { Realm = realm; }
            await Context.Channel.TriggerTypingAsync();
            var b = Command_Helper.HTTPGET("https://eu.api.battle.net/wow/character/", Realm + "/" + Player + "?fields=progression&locale=en_GB&apikey=############################");
            string j = await Command_Helper.DownloadString("https://www.wowprogress.com/character/eu/" + Realm + "/" + Player);
            var s = Command_Helper.HTTPGET("https://eu.api.battle.net/wow/character/", Realm + "/" + Player + "?fields=items&locale=en_GB&apikey=############################");
            var g = Command_Helper.HTTPGET("https://eu.api.battle.net/wow/character/", Realm + "/" + Player + "?fields=guild&locale=en_GB&apikey=############################");
            var k = Command_Helper.HTTPGET("https://eu.api.battle.net/wow/character/", Realm + "/" + Player + "?fields=stats&locale=en_GB&apikey=############################");



            //Warcraftlogs(Player, Realm, Context);
            string t = @"http://eu.battle.net/wow/en/character/" + Realm + @"/" + Player + @"/simple";

            string HP = Command_Helper.StringFinder(k, ",\"stats\":{\"health\":" + Command_Helper.all + ",\"powerType\":\"");
            string MainStats = Command_Helper.StringFinder(k, "\"powerType\":\"" + Command_Helper.all + ",\"speedRating\":");
            var higheststat = 0;
            var nextstatname = "";
            var statname = "";
            foreach (var item in MainStats.Split(':'))
            {
                try
                {



                    var valuetoint = int.Parse(item.Split(',')[0]);
                    if (valuetoint > higheststat && !nextstatname.Contains("sta"))
                    {
                        higheststat = valuetoint;
                        statname = nextstatname;
                    }

                    nextstatname = Command_Helper.StringFinder(item, ",\"" + Command_Helper.all + "\"");
                }
                catch (Exception)
                {
                }
            }
            string MainStat = statname.Replace("agi", "Agility").Replace("int", "Intellect").Replace("str", "Strength");
            string MainStatValue = higheststat.ToString();
            string Mastery = Command_Helper.StringFinder(k, ",\"mastery\":" + Command_Helper.all + ",\"masteryRating\":");
            string Haste = Command_Helper.StringFinder(k, ",\"haste\":" + Command_Helper.all + ",\"hasteRating\":");
            string Versa = Command_Helper.StringFinder(k, ",\"versatilityDamageDoneBonus\":" + Command_Helper.all + ",\"versatilityHealingDoneBonus\":");
            string Crit = Command_Helper.StringFinder(k, ",\"crit\":" + Command_Helper.all + ",\"critRating\":");
            string ArtifactPower = Command_Helper.StringFinder(j, "class=\"char_rating_area\"><div class=\"gearscore\">Artifact Power: " + Command_Helper.all + "</div><table style=\"width:auto;border-spacing:0;margin:0\"><tr><td><a href=\"/arti");
            string ArtifactLevel = Command_Helper.StringFinder(j, "</div><h2>Artifact Traits" + Command_Helper.all + "</h2><table class=\"rating\"><tr><td><a href=").Replace(" (Level ", "").Replace(")", "");
            string SimDps = Command_Helper.StringFinder(j, "dps_score\" onclick=\"return false\" href=\"#\">" + Command_Helper.all + "</a><div id=\"data_simdps_score\" style=\"display:none\"");
            string Ilvl = Command_Helper.StringFinder(s, ":{\"averageItemLevel\":" + Command_Helper.all + ",\"averageItemLevelEquipped\"");
            string IlvlE = Command_Helper.StringFinder(s, "\"averageItemLevelEquipped\":" + Command_Helper.all + ",\"head\":{\"id\":");
            string Guild = Command_Helper.StringFinder(g, "\"guild\":{\"name\":\"" + Command_Helper.all + "\",\"realm\":");
            string AchivementPoints = Command_Helper.StringFinder(s, "\"achievementPoints\":" + Command_Helper.all + ",\"thumbnail\"");
            string Level = Command_Helper.StringFinder(s, "\"level\":" + Command_Helper.all + ",\"achievementPoints");

            while (Command_Helper.valueee == "")
            { Thread.Sleep(100); }
            string Des =
                "**Guild : **" + Guild + Environment.NewLine +
                "**Level :**" + Level + Environment.NewLine +
                "**HP : **" + HP + Environment.NewLine +
                "**Average item level : **" + Ilvl + Environment.NewLine +
                "**Average item level equipped : **" + IlvlE + Environment.NewLine +
                "**Artifact POWER : **" + ArtifactPower + Environment.NewLine +
                "**Artifact level : **" + ArtifactLevel + Environment.NewLine +
                "**AchivementPoints : **" + AchivementPoints + Environment.NewLine +
                "**WowProgress SimDps : **" + SimDps + Environment.NewLine +
                "**" + MainStat + ":**" + MainStatValue + "  **Mastery:**" + Mastery + "%  **Haste:**" + Haste + "%  **Crit:**" + Crit + "%  **Versatility:**" + Versa + "%" + Environment.NewLine +
                "[Wowprogress.com](https://www.wowprogress.com/character/eu/" + Realm + "/" + Player + ") " +
                "[Warcraftlogs.com]" + "(" + Command_Helper.valueee + ") " +
                "[Armory]" + "(" + t + ") " + Environment.NewLine + Environment.NewLine +
                "" + Command_Helper.GetProgressString(b)

                ;


            await Context.Channel.SendMessageAsync("", false, Command_Helper.SimpleEmbed(new Color(1f, 1f, 1f), Player, Des, "http://render-eu.worldofwarcraft.com/character/" + Command_Helper.StringFinder(s, ",\"thumbnail\":\"" + Command_Helper.all + "\",\"calcClass\":\"")));

        }
        [Command("fotm")]
        public async Task fotm()
        {
            await Context.Channel.SendMessageAsync(Command_Helper.FOTM());
        }
        [Command("dog")]
        public async Task dog()
        {
            await Context.Channel.TriggerTypingAsync();
            string file = Command_Helper.GetRandomDoggie();
            await Context.Channel.SendFileAsync(file);
            File.Delete(file);
        }
        [Command("cat")]
        [Alias("kitty")]
        public async Task cat()
        {
            await Context.Channel.TriggerTypingAsync();
            string file = Command_Helper.GetRandomCat();
            await Context.Channel.SendFileAsync(file);
            File.Delete(file);
        }
        [Command("roll")]
        public async Task Roll([Optional] string from, [Optional] string to)
        {
            int From = 0;
            int To = 100;
            Random s = new Random();
            if (from != null && to != null)
            {
                From = int.Parse(from);
                To = int.Parse(to);
            }

            await Context.Channel.SendMessageAsync(Context.User.Username + " Rolled " + s.Next(From, To));
        }
        [Command("google")]
        public async Task google([Remainder]string text)
        {
            await Context.Channel.SendMessageAsync(Command_Helper.Google(text));
        }
        [Command("imgur")]
        public async Task imgur([Remainder]string text)
        {
            await Context.Channel.SendMessageAsync(Command_Helper.ImgurS(text));
        }
        [Command("youtube")]
        public async Task youtube([Remainder]string text)
        {
            await Context.Channel.SendMessageAsync(Command_Helper.youtubeS(text));
        }
        [Command("tactic")]
        public async Task Wowtactic([Remainder]string text)
        {
            await Context.Channel.SendMessageAsync(Command_Helper.Fatboss(text));
        }
        [Command("invasion")]
        public async Task invation()
        {
            await Context.Channel.TriggerTypingAsync();

            InvasionDTO current = new InvasionDTO();
            current = Command_Helper.invasion();
            if (current.isActive)
            {
                await Context.Channel.SendMessageAsync(
                    "Invasion is up at the moment!" + Environment.NewLine +
                    current.Hour + " hours ," + current.Min + " minutes  and " + current.Sec + " seconds remaining."
                    );
            }
            else
            {
                await Context.Channel.SendMessageAsync(
               "Invasion is not up." + Environment.NewLine +
               current.Hour + " hours ," + current.Min + " minutes  and " + current.Sec + " seconds remaining till the next one!"
               );
            }
        }
        [Command("insult")]
        public async Task insult([Optional]string name)
        {
            int ls = 5;
            List<SocketGuildUser> UsersOnline = new List<SocketGuildUser>();

            if (name == null)
            {
                foreach (var item in Context.Guild.Users)
                {
                    if (item.Status != UserStatus.Offline)
                    {
                        UsersOnline.Add(item);
                    }
                }
                Random s = new Random();
                ls = s.Next(1, UsersOnline.Count());
                var Randomuser = UsersOnline.ToList()[ls];
                await Context.Channel.SendMessageAsync(Command_Helper.Randominsult(Randomuser.Username));
            }
            else
            {
                await Context.Channel.SendMessageAsync(Command_Helper.Randominsult(name));
            }
        }
        [Command("pun")]
        public async Task pun()
        {
            string text = Command_Helper.RandomPun();
            await Context.Channel.SendMessageAsync(text);
        }
       
    }
}
