using Discord;
using DiscordExampleBot;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityBot.Modules.Dto;

namespace Necessity.Modules
{
    public static class Command_Helper
    {
        public static Embed SimpleEmbed(Color c, string title, string description, string image)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.ThumbnailUrl = image;
            eb.WithColor(c);
            eb.Title = title;
            eb.WithDescription(description);
            return eb.Build();
        }
        public static IEnumerable<TSource> TopWithTies<TSource, TValue>(
    this IEnumerable<TSource> source,
    int count,
    Func<TSource, TValue> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            if (count == 0) yield break;
            using (var iter = source.OrderByDescending(selector).GetEnumerator())
            {
                if (iter.MoveNext())
                {
                    yield return iter.Current;
                    while (--count >= 0)
                    {
                        if (!iter.MoveNext()) yield break;
                        yield return iter.Current;
                    }
                    var lastVal = selector(iter.Current);
                    var eq = EqualityComparer<TValue>.Default;
                    while (iter.MoveNext() && eq.Equals(lastVal, selector(iter.Current)))
                    {
                        yield return iter.Current;
                    }
                }
            }
        }
        public static EmbedBuilder SEmbed(string title, string description)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(Color.Blue);
            eb.Title = title;
            eb.WithDescription(description);
            return eb;
        }
        public static Embed SimpleEmbed(Color c, string title, string description)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(c);
            eb.Title = title;
            eb.WithDescription(description);
            return eb.Build();
        }
        public static Embed SimpleEmbed(Color c, string title, string description, string URL, string image)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Url = URL;
            eb.WithColor(c);
            eb.Title = title;
            eb.WithDescription(description);
            return eb.Build();
        }
        static public string[] SplitOnString(string input, string Spliton)
        {
            return input.Split(new string[] { Spliton }, StringSplitOptions.None);
        }
        static public string StringFinder(string Input, string Split)
        {
            Match match = Regex.Match(Input, Split, RegexOptions.IgnoreCase);
            string value = "";
            if (match.Success)
            {
                value = match.Groups[1].Value; //result here
            }
            return value;
        }
        static public string valueee;
        static public string HTTPGET(string baseadress, string request)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(baseadress);
                HttpResponseMessage response = client.GetAsync(request).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            };


        }
        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
        public static async Task<string> DownloadString(string url)
        {
            string data = "";
            if (url.ToUpper().Contains("html".ToUpper()))
            {
                data = File.ReadAllText(url);
            }
            else
            {

                var client = new HttpClient();
                data = await client.GetStringAsync(url);


            }
            return data;
        }
        static string Sounds;
        static string Memes;
        static public string ImgurS(string text)
        {
            return Google(text + " site:imgur.com");

        }
        static public string youtubeS(string text)
        {
            return Google(text + " site:youtube.com");
        }
        static public string Fatboss(string text)
        {
            return Google(" wow fatboss " + text + " site:youtube.com");
        }
        static public string Google(string text)
        {
            try
            {
                string URL = "";
                string title = "";
                const string apiKey = "########################################";
                const string searchEngineId = "###############################:##########";
                string query = text;
                CustomsearchService customSearchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer() { ApiKey = apiKey });
                Google.Apis.Customsearch.v1.CseResource.ListRequest listRequest = customSearchService.Cse.List(query);
                listRequest.Cx = searchEngineId;
                Search search = listRequest.Execute();
                int count = 0;

                foreach (var item in search.Items)
                {
                    if (count < 1)
                    {
                        URL = item.Link;
                        title = item.Title.Replace(" - YouTube", "");

                        count = 3;
                    }
                }
                return URL;
            }
            catch (Exception)
            {
                return "Couldnt find any results for  '" + text + "'";
            }

        }
        static public string FOTM()
        {
            List<Classes> classes = new List<Classes>();

            ///////Demon Hunter///////////
            Classes Class = new Classes();
            Class.ClassName = "Demon Hunter";
            List<string> specs = new List<string>();
            specs.Add("Havoc");
            specs.Add("Vengeance");
            Class.Specs = specs;
            classes.Add(Class);
            ////////////////////////////

            ///////Death knite///////////
            List<string> specs1 = new List<string>();
            Classes Class1 = new Classes();
            Class1.ClassName = "DK";
            specs1.Add("Blood");
            specs1.Add("Frost");
            Class1.Specs = specs1;
            specs1.Add("Unholy");
            classes.Add(Class1);
            ////////////////////////////
            List<string> specs2 = new List<string>();
            ///////Hunter ///////////
            Classes Class2 = new Classes();
            Class2.ClassName = "Hunter";
            specs2.Add("Marksmand");
            specs2.Add("Survival");
            specs2.Add("Beast Mastery");
            Class2.Specs = specs2;
            classes.Add(Class2);
            ////////////////////////////
            List<string> specs3 = new List<string>();
            ///////Mage///////////
            Classes Class3 = new Classes();
            Class3.ClassName = "Mage";
            specs3.Add("Fire");
            specs3.Add("Frost");
            specs3.Add("Arcane");
            Class3.Specs = specs3;
            classes.Add(Class3);
            ////////////////////////////
            List<string> specs4 = new List<string>();
            ///////pally///////////
            Classes Class4 = new Classes();
            Class4.ClassName = "Paladin";
            specs4.Add("Holy");
            specs4.Add("Protection");
            specs4.Add("Retribution");
            Class4.Specs = specs4;
            classes.Add(Class4);
            ////////////////////////////
            List<string> specs5 = new List<string>();
            ///////warlock///////////
            Classes Class5 = new Classes();
            Class5.ClassName = "Warlock";
            specs5.Add("Affliction");
            specs5.Add("Demonology");
            specs5.Add("Destruction");
            Class5.Specs = specs5;
            classes.Add(Class5);
            ////////////////////////////
            List<string> specs7 = new List<string>();
            ///////priest///////////
            Classes Class7 = new Classes();
            Class7.ClassName = "Priest";
            specs7.Add("Holy");
            specs7.Add("Shadow");
            specs7.Add("Discipline");
            Class7.Specs = specs7;
            classes.Add(Class7);
            ////////////////////////////
            List<string> specs6 = new List<string>();
            ///////monk///////////
            Classes Class6 = new Classes();
            Class6.ClassName = "Monk";
            specs6.Add("Windwalker");
            specs6.Add("Mistweaver");
            specs6.Add("Brewmaster");
            Class6.Specs = specs6;
            classes.Add(Class6);
            ////////////////////////////
            List<string> specs8 = new List<string>();
            ///////druid///////////
            Classes Class8 = new Classes();
            Class8.ClassName = "Druid";
            specs8.Add("Feral");
            specs8.Add("Balance");
            specs8.Add("Guardian");
            specs8.Add("Restro");
            Class8.Specs = specs8;
            classes.Add(Class8);
            ////////////////////////////
            List<string> specs9 = new List<string>();
            ///////warrior//////////
            Classes Class9 = new Classes();
            Class9.ClassName = "Warrior";
            specs9.Add("Arms");
            specs9.Add("Fury");
            specs9.Add("Protection");
            Class9.Specs = specs9;
            classes.Add(Class9);
            ////////////////////////////
            List<string> specs10 = new List<string>();
            ///////Rouge///////////
            Classes Class10 = new Classes();
            Class10.ClassName = "Rouge";
            specs10.Add("Assassination");
            specs10.Add("Outlaw");
            specs10.Add("Subtlety");
            Class10.Specs = specs10;
            classes.Add(Class10);
            ////////////////////////////
            List<string> specs11 = new List<string>();
            ///////Shaman///////////
            Classes Class11 = new Classes();
            Class11.ClassName = "Shaman";
            specs11.Add("Elemental");
            specs11.Add("Enhancement");
            specs11.Add("Restoration");
            Class11.Specs = specs11;
            classes.Add(Class11);
            ////////////////////////////
            List<string> races = new List<string>();
            races.Add("Human");
            races.Add("Dwarf");
            races.Add("Night Elf");
            races.Add("Gnome");
            races.Add("Worgen");
            races.Add("Dranei");
            races.Add("Pandaren");
            /*races.Add("Orc");
            races.Add("Undead");
            races.Add("Tauren");
            races.Add("Troll");
            races.Add("Goblin");
            races.Add("Blood Elf");
            */
            List<string> Gender = new List<string>();
            Gender.Add("Male");
            Gender.Add("Female");

            Random RandomNummber = new Random();

            var gender = Gender[RandomNummber.Next(0, Gender.Count)];
            var race = races[RandomNummber.Next(0, races.Count)];
            var classs = classes[RandomNummber.Next(0, classes.Count)];
            var spec = classs.Specs[RandomNummber.Next(0, classs.Specs.Count)];

            string result = "Flavor of the month: " + gender + " " + race + " " + spec + " " + classs.ClassName;
            return result;

        }
        static public string Randominsult(string username)
        {
            string ReturnValue = "";
            Random s = new Random();
            //int nr = 0;

            string userName = username;

            Random insult = new Random();
            var insultnr = insult.Next(0, 39);
            if (insultnr == 0)
            {
                ReturnValue = ("I'm not saying I hate " + userName + ", but I would unplug his/her life support to charge my phone.");
            }
            else if (insultnr == 1)
            {
                ReturnValue = ("Is " + userName + "'s ass jealous of the amount of shit that just came out of his/her mouth?");
            }
            else if (insultnr == 2)
            {
                ReturnValue = (userName + "'s birth certificate is an apology letter from the condom factory.");
            }
            else if (insultnr == 3)
            {
                ReturnValue = ("I bet " + userName + "'s brain feels as good as new, seeing that he/she never use it.");
            }
            else if (insultnr == 4)
            {
                ReturnValue = (userName + " brings everyone a lot of joy, when he/she leaves the room.");
            }
            else if (insultnr == 5)
            {
                ReturnValue = (userName + " is the reason they invented double doors!");
            }
            else if (insultnr == 6)
            {
                ReturnValue = (userName + " must have been born on a highway because that's where most accidents happen.");
            }
            else if (insultnr == 7)
            {
                ReturnValue = ("What's the difference between " + userName + " and eggs? Eggs get laid and he/she don't.");
            }
            else if (insultnr == 8)
            {
                ReturnValue = ("I'm jealous of all the people that haven't met " + userName + "!");
            }
            else if (insultnr == 9)
            {
                ReturnValue = ("If I wanted to kill myself I'd climb " + userName + "'s ego and jump to his/her IQ.");
            }
            else if (insultnr == 10)
            {
                ReturnValue = (userName + " shouldn't play hide and seek, no one would look for him/her.");
            }
            else if (insultnr == 12)
            {
                ReturnValue = ("Shut up " + userName + ", you'll never be the man your mother is.");
            }
            else if (insultnr == 13)
            {
                ReturnValue = (userName + "'s family tree is a cactus, because everybody on it is a prick.");
            }
            else if (insultnr == 14)
            {
                ReturnValue = (userName + " is so ugly that when his/her mama dropped him/her off at school she got a fine for littering.");
            }
            else if (insultnr == 15)
            {
                ReturnValue = ("If " + userName + " were twice as smart, he/she still would be stupid.");
            }
            else if (insultnr == 16)
            {
                ReturnValue = ("Two wrongs don't make a right, take " + userName + "'s parents as an example.");
            }
            else if (insultnr == 17)
            {
                ReturnValue = ("I'd like to see things from " + userName + "'s point of view but I can't seem to get my head that far up my ass.");
            }
            else if (insultnr == 18)
            {
                ReturnValue = ("It's better to let someone think " + userName + " is an Idiot than for him/her to open his/her mouth and prove it.");
            }
            else if (insultnr == 19)
            {
                ReturnValue = ("If laughter is the best medicine, " + userName + "'s face must be curing the world.");
            }
            else if (insultnr == 20)
            {
                ReturnValue = ("If you really want to know about mistakes, you should ask " + userName + "'s parents.");
            }
            else if (insultnr == 21)
            {
                ReturnValue = ("I wasn't born with enough middle fingers to let you know how I feel about " + userName + ".");
            }
            else if (insultnr == 22)
            {
                ReturnValue = ("It looks like " + userName + "'s face caught on fire and someone tried to put it out with a hammer.");
            }
            else if (insultnr == 23)
            {
                ReturnValue = ("What language is " + userName + " speaking? Cause it sounds like bullshit.");
            }
            else if (insultnr == 24)
            {
                ReturnValue = (userName + " is so fake, Barbie is jealous.");
            }
            else if (insultnr == 25)
            {
                ReturnValue = (userName + " is the reason the gene pool needs a lifeguard.");
            }
            else if (insultnr == 26)
            {
                ReturnValue = ("So, a thought crossed " + userName + "'s mind? Must have been a long and lonely journey.");
            }
            else if (insultnr == 27)
            {
                ReturnValue = ("I don't think " + userName + " is stupid. he/she just have a bad luck when thinking.");
            }
            else if (insultnr == 28)
            {
                ReturnValue = (userName + " is proof that evolution CAN go in reverse.");
            }
            else if (insultnr == 29)
            {
                ReturnValue = ("Calling " + userName + " an idiot would be an insult to all stupid people.");
            }
            else if (insultnr == 30)
            {
                ReturnValue = ("Looking at " + userName + ", I understand why some animals eat their young.");
            }
            else if (insultnr == 31)
            {
                ReturnValue = (userName + " is so ugly when he / she were born the doctor threw him / her out the window and the window threw it back.");
            }
            else if (insultnr == 32)
            {
                ReturnValue = ("I may love to shop but I'm not buying " + userName + "'s bullshit.");
            }
            else if (insultnr == 33)
            {
                ReturnValue = (userName + " is like school in the summertime - no class.");
            }
            else if (insultnr == 34)
            {
                ReturnValue = ("How many times do I have to flush before " + userName + " go away?");
            }
            else if (insultnr == 35)
            {
                ReturnValue = ("Brains aren't everything. In " + userName + "'s case they're nothing.");
            }
            else if (insultnr == 36)
            {
                ReturnValue = (userName + " is not as bad as people say, he/she is much, much worse.");
            }
            else if (insultnr == 37)
            {
                ReturnValue = ("We all sprang from apes, but " + userName + " didn't spring far enough.");
            }
            else if (insultnr == 38)
            {
                ReturnValue = ("Ordinarily people live and learn. " + userName + " just live.");
            }
            else if (insultnr == 39)
            {
                ReturnValue = ("If I had a dollar for every time " + userName + " said something smart, I'd be broke.");
            }
            else if (insultnr == 11)
            {
                ReturnValue = ("It's scary to think that people like " + userName + " are graduating from college.");
            }
            return ReturnValue;

        }
        static public string RandomPun()
        {
            string ReturnValue = "";
            Random insult = new Random();
            var insultnr = insult.Next(0, 40);
            if (insultnr == 0)
            {
                ReturnValue = ("I'd tell you a chemistry joke but I know I wouldn't get a reaction.");
            }
            else if (insultnr == 1)
            {
                ReturnValue = ("Have you ever tried to eat a clock? It's very time consuming.");
            }
            else if (insultnr == 2)
            {
                ReturnValue = ("I would punch you in the face, but you know... shit splatters");
            }
            else if (insultnr == 3)
            {
                ReturnValue = ("I used to be a banker but I lost interest");
            }
            else if (insultnr == 4)
            {
                ReturnValue = ("When William joined the army he disliked the phrase 'fire at will'.");
            }
            else if (insultnr == 5)
            {
                ReturnValue = ("The roundest knight at king Arthur's round table was Sir Cumference.");
            }
            else if (insultnr == 6)
            {
                ReturnValue = ("A bicycle can't stand on its own because it is two-tired.");
            }
            else if (insultnr == 7)
            {
                ReturnValue = ("To write with a broken pencil is pointless.");
            }
            else if (insultnr == 8)
            {
                ReturnValue = ("When Peter Pan punches, they Neverland.");
            }
            else if (insultnr == 9)
            {
                ReturnValue = ("Why don't some couples go to the gym? Because some relationships don't work out.");
            }
            else if (insultnr == 10)
            {
                ReturnValue = ("Did you hear about the guy whose whole left side was cut off? He's all right now.");
            }
            else if (insultnr == 12)
            {
                ReturnValue = ("I wasn't originally going to get a brain transplant, but then I changed my mind.");
            }
            else if (insultnr == 13)
            {
                ReturnValue = ("I used to be addicted to soap, but I'm clean now.");
            }
            else if (insultnr == 14)
            {
                ReturnValue = ("I wondered why the baseball was getting bigger. Then it hit me.");
            }
            else if (insultnr == 15)
            {
                ReturnValue = ("Yesterday I accidentally swallowed some food coloring. The doctor says I'm OK, but I feel like I've dyed a little inside.");
            }
            else if (insultnr == 16)
            {
                ReturnValue = ("Why did the pig stop sunbathing? He was bacon in the heat.");
            }
            else if (insultnr == 17)
            {
                ReturnValue = ("I would tell you a leech joke, but it would suck anyway.");
            }
            else if (insultnr == 18)
            {
                ReturnValue = ("Why don't programmers like nature? It has too many bugs.");
            }
            else if (insultnr == 19)
            {
                ReturnValue = ("did you hear the joke about the german sausage? It was the wurst");
            }
            else if (insultnr == 20)
            {
                ReturnValue = ("I don't trust these stairs because they're always up to something.");
            }
            else if (insultnr == 21)
            {
                ReturnValue = ("Where do you imprison a skeleton? In a rib cage.");
            }
            else if (insultnr == 22)
            {
                ReturnValue = ("I wrote a book about birds. It flew off the shelf.");
            }
            else if (insultnr == 23)
            {
                ReturnValue = ("i once heard a joke about amnesia, but I forgot how it goes.");
            }
            else if (insultnr == 24)
            {
                ReturnValue = ("Police were called to a daycare where a three-year-old was resisting a rest.");
            }
            else if (insultnr == 25)
            {
                ReturnValue = ("I'm glad I know sign language, it's pretty handy.");
            }
            else if (insultnr == 26)
            {
                ReturnValue = ("Talking to her about computer hardware I make my mother board.");
            }
            else if (insultnr == 27)
            {
                ReturnValue = ("My friend's bakery burned down last night. Now his business is toast.");
            }
            else if (insultnr == 28)
            {
                ReturnValue = ("what do you do when chemists die? Barium.");
            }
            else if (insultnr == 29)
            {
                ReturnValue = ("The furniture store keeps calling me to come back. But all i wanted was one night stand");
            }
            else if (insultnr == 30)
            {
                ReturnValue = ("Yesterday I was on the computer, I couldn't find the Esc and I lost Ctrl.");
            }
            else if (insultnr == 31)
            {
                ReturnValue = ("My computer is so slow it hertz.");
            }
            else if (insultnr == 32)
            {
                ReturnValue = ("My rechargeable batteries are revolting.");
            }
            else if (insultnr == 33)
            {
                ReturnValue = ("The man put his name on the neck of his shirt so he would have collar ID.");
            }
            else if (insultnr == 34)
            {
                ReturnValue = ("Why dont cannibals eat clowns? They taste funny!");
            }
            else if (insultnr == 35)
            {
                ReturnValue = ("how can you spot a blind guy at a nudist colony? its not hard...");
            }
            else if (insultnr == 36)
            {
                ReturnValue = ("My battery had an alkaline problem, so it went to AA meetings.");
            }
            else if (insultnr == 37)
            {
                ReturnValue = ("I'm so poor i can't even pay attention");
            }
            else if (insultnr == 38)
            {
                ReturnValue = ("Atheism is a non-prophet organisation");
            }
            else if (insultnr == 39)
            {
                ReturnValue = ("I saw an ad for burial plots, and thought to myself this is the last thing i need.");
            }
            else if (insultnr == 11)
            {
                ReturnValue = ("Last time i got caught stealing a calendar i got 12 months.");
            }
            else if (insultnr == 40)
            {
                ReturnValue = ("Which day do chickens hate the most? Friday");
            }
            return ReturnValue;
        }
        public static async Task DownloadAsync(Uri requestUri, string filename)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (
                Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true))
            {
                await contentStream.CopyToAsync(stream);
            }
        }
        public static string all = @"(?<text>[^\]]*)";
        public static string all2 = @"\s*(.+?)\s*";
        public class Grabby
        {
            public string Grab(string url)
            {
                var sd = "index.js " + url;
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {


                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "phantomjs.exe",
                    Arguments = sd
                };

                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return output;
            }
        }
        static public InvasionDTO invasion()
        {
            var grabby = new Grabby();

            string output = grabby.Grab("https://wow.gameinfo.io/invasions");
            Console.WriteLine(output);
            //File.WriteAllText("c:\\test.html", output);
            InvasionDTO invasion = new InvasionDTO();
            MatchCollection m1 = Regex.Matches(output.ToLower(), "EU realms".ToLower() + all2 + "Invasion schedule".ToLower(), RegexOptions.Singleline);
            foreach (var m in m1)
            {
                MatchCollection m2 = Regex.Matches(m.ToString(), "<span>" + all2 + "</span>", RegexOptions.Singleline);
                int i = 0;

                foreach (var item in m2)
                {
                    int value = 0;
                    var gssd = StringFinder(m.ToString(), "<progress valu" + all + "\"></progress>");

                    string ge = item.ToString().Replace("<span>", "").Replace("</span>", "");
                    value = int.Parse(ge);


                    if (i == 0)
                    {
                        if (!gssd.ToString().Contains("-"))
                        {
                            invasion.Hour = value;
                            invasion.isActive = true;
                        }
                        else
                        {
                            invasion.Hour = value;
                            invasion.isActive = false;
                        }
                    }
                    if (i == 1)
                    {
                        if (!gssd.ToString().Contains("-"))
                        {
                            invasion.Min = value;
                            invasion.isActive = true;
                        }
                        else
                        {
                            invasion.Min = value;
                            invasion.isActive = false;
                        }
                    }
                    if (i == 2)
                    {
                        if (!gssd.ToString().Contains("-"))
                        {
                            invasion.Sec = value;
                            invasion.isActive = true;
                        }
                        else
                        {
                            invasion.Sec = value;
                            invasion.isActive = false;
                        }
                    }
                    if (i == 3)
                    {
                        if (!gssd.ToString().Contains("-"))
                        {
                            invasion.Hour = value;
                            invasion.isActive = false;
                        }
                    }
                    if (i == 4)
                    {
                        if (!gssd.ToString().Contains("-"))
                        {
                            invasion.Min = value;
                            invasion.isActive = false;
                        }
                    }
                    if (i == 5)
                    {
                        if (gssd.ToString().Contains("-"))
                        {
                            invasion.Sec = value;
                            invasion.isActive = false;
                        }
                    }

                    i++;
                }

            }

            return invasion;
        }
        static public string GetRandomCat()
        {
            return DownloadFile("http://www.randomkittengenerator.com/cats/rotator.php");
        }
        static public string GetRandomDoggie()
        {
            return DownloadFile("http://www.randomdoggiegenerator.com/randomdoggie.php");
        }
        static public string DownloadFile(string file)
        {
            Random s = new Random();
            string SavePath = "File" + s.Next(1000, 9999) + ".jpg";
            using (var cli = new HttpClient())
            {
                var rslt = cli.GetAsync(file).GetAwaiter().GetResult();

                if (rslt.IsSuccessStatusCode)
                {
                    var dat = rslt.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    File.WriteAllBytes(SavePath, dat);
                }
            }
            return SavePath;
        }
        static public string GetProgressString(string ProgressionString)
        {
            dynamic stuff = JsonConvert.DeserializeObject(ProgressionString);
            string name = stuff.name;
            var prog = stuff.progression;
            var raids = prog.raids;
            var NH = raids[37].bosses;
            var EN = raids[35].bosses;
            var ToV = raids[36].bosses;
            var ToS = raids[38].bosses;




            int ToS_HC = 0;
            int ToS_LFR = 0;
            int ToS_NM = 0;
            int ToS_M = 0;
            foreach (var item in ToS)
            {
                if (item.heroicKills > 0)
                {
                    ToS_HC++;
                }
                if (item.normalKills > 0)
                {
                    ToS_NM++;
                }
                if (item.mythicKills > 0)
                {
                    ToS_M++;
                }
                if (item.lfrKills > 0)
                {
                    ToS_LFR++;
                }
            }
            int NH_HC = 0;
            int NH_LFR = 0;
            int NH_NM = 0;
            int NH_M = 0;

            foreach (var item in NH)
            {
                if (item.heroicKills > 0)
                {
                    NH_HC++;
                }
                if (item.normalKills > 0)
                {
                    NH_NM++;
                }
                if (item.mythicKills > 0)
                {
                    NH_M++;
                }
                if (item.lfrKills > 0)
                {
                    NH_LFR++;
                }
            }
            int EN_HC = 0;
            int EN_LFR = 0;
            int EN_NM = 0;
            int EN_M = 0;

            foreach (var item in EN)
            {
                if (item.heroicKills > 0)
                {
                    EN_HC++;
                }
                if (item.normalKills > 0)
                {
                    EN_NM++;
                }
                if (item.mythicKills > 0)
                {
                    EN_M++;
                }
                if (item.lfrKills > 0)
                {
                    EN_LFR++;
                }
            }
            int ToV_HC = 0;
            int ToV_LFR = 0;
            int ToV_NM = 0;
            int ToV_M = 0;

            foreach (var item in ToV)
            {
                if (item.heroicKills > 0)
                {
                    ToV_HC++;
                }
                if (item.normalKills > 0)
                {
                    ToV_NM++;
                }
                if (item.mythicKills > 0)
                {
                    ToV_M++;
                }
                if (item.lfrKills > 0)
                {
                    ToV_LFR++;
                }
            }
            string ToVShow = "";
            string ENShow = "";
            string NHShow = "";
            string ToSShow = "";
            if (ToS_M != 0)
            {
                ToSShow = "***ToS*** :" + ToS_M + "/9M";
            }
            else if (ToS_HC != 0)
            {
                ToSShow = "***ToS*** :" + ToS_HC + "/9HC";
            }
            else if (ToS_NM != 0)
            {
                ToSShow = "***ToS*** :" + ToS_NM + "/9NM";
            }
            else if (ToS_LFR != 0)
            {
                ToSShow = "***ToS*** :" + ToS_LFR + "/9LFR";
            }
            if (NH_M != 0)
            {
                NHShow = "***NH*** :" + NH_M + "/10M";
            }
            else if (NH_HC != 0)
            {
                NHShow = "***NH*** :" + NH_HC + "/10HC";
            }
            else if (NH_NM != 0)
            {
                NHShow = "***NH*** :" + NH_NM + "/10NM";
            }
            else if (NH_LFR != 0)
            {
                NHShow = "***NH*** :" + NH_LFR + "/10LFR";
            }
            if (EN_M != 0)
            {
                ENShow = "***EN*** :" + EN_M + "/7M";
            }
            else if (EN_HC != 0)
            {
                ENShow = "***EN*** :" + EN_HC + "/7HC";
            }
            else if (EN_NM != 0)
            {
                ENShow = "***EN*** :" + EN_NM + "/7NM";
            }
            else if (EN_LFR != 0)
            {
                ENShow = "***EN*** :" + EN_LFR + "/7LFR";
            }
            if (ToV_M != 0)
            {
                ToVShow = "***ToV*** :" + ToV_M + "/3M";
            }
            else if (ToV_HC != 0)
            {
                ToVShow = "***ToV*** :" + ToV_HC + "/3HC";
            }
            else if (ToV_NM != 0)
            {
                ToVShow = "***ToV*** :" + ToV_NM + "/3NM";
            }
            else if (ToV_LFR != 0)
            {
                ToVShow = "***ToV*** :" + ToV_LFR + "/3LFR";
            }
            string done = ENShow + "   " + ToVShow + "   " + NHShow + "    " + ToSShow;
            /*   string done =
                   "***NH-LFR***:" + NH_LFR+ "/10   ***NH-NM***:"+ NH_NM+"/10   ***NH-HC***:"+ NH_HC + "/10   ***NH-M***:"+ NH_M + "/10" + Environment.NewLine +
                   "***ToV-LFR***:" + ToV_LFR + "/3   ***ToV-NM***:" + ToV_NM + "/3   ***ToV-HC***:" + ToV_HC + "/3   ***ToV-M***:" + ToV_M + "/3" + Environment.NewLine +
                   "***EN-LFR***:" + EN_LFR + "/7   ***EN-NM***:" + EN_NM + "/7   ***EN-HC***:" + EN_HC + "/7   ***EN-M***:" + EN_M + "/7"
                   ;
                   */
            return done;
        }
    }
}
