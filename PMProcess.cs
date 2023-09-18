using Discord;
using Discord.Addons.EmojiTools;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Necessity
{

    public class PMProcess
    {
        public async Task Main(SocketUserMessage msg, SocketReaction reaction)
        {
            if (msg == null || !msg.Author.IsBot)
           {

                IPrivateChannel channel = null;
                if (msg != null)
                {
                    channel = msg.Channel as IPrivateChannel;
                }
                
                
                PMRequest request = null;
                if ( msg != null &&CommandHandler.Requests.Exists(x => x.isActive && x.UserID == msg.Author.Id))
                {
                    request = CommandHandler.Requests.Find(x => x.isActive && x.UserID == msg.Author.Id);
                }
                else if (msg == null && CommandHandler.Requests.Exists(x => x.isActive && x.UserID == reaction.User.Value.Id))
                {
                    request = CommandHandler.Requests.Find(x => x.isActive && x.UserID == reaction.User.Value.Id);
                }
                else
                {
                    request = new PMRequest();
                    request.UserID = msg.Author.Id;
                    request.isActive = true;
                    request.Username = msg.Author.Username;
                    CommandHandler.Requests.Add(request);
                    var eb = Util.Embed(Color.DarkRed, "Hi there!", "What would you like to report?");
                    eb.AddField("Late for raid", ":one:",true);
                    eb.AddField("Cant come to raid", ":two:",true);
                    eb.AddField("Send a message to officers", ":three:",true);
                    var message = msg.Channel.SendMessageAsync("", false, eb.Build());
                    List<string> ls = new List<string>();
                    ls.Add("one");
                    ls.Add("two");
                    ls.Add("three");
                    await addemoji(ls,message.Result);
          
                    //await message.Result.AddReactionAsync(EmojiExtensions.FromText(":one:"));
                    //await message.Result.AddReactionAsync(EmojiExtensions.FromText(":two:"));
                    //await message.Result.AddReactionAsync(EmojiExtensions.FromText(":three:"));

                }
                string emote = "";
                if (reaction != null)
                {
                    emote = reaction.Emote.Name;
                }
                
                await CheckRequest(channel, msg, request, emote,reaction);
            }
        }
        private async Task addemoji(List<string> list, RestUserMessage message)
        {
            foreach (var item in list)
            {

                await message.AddReactionAsync(EmojiExtensions.FromText(":" + item + ":"));

            };
        }
        public async Task CheckRequest(IPrivateChannel channel, SocketUserMessage msg, PMRequest request, string emote,SocketReaction reaction)
        {
            if (request.Request == null)
            {
                if (emote == "1⃣")
                {
                    request.Request = new LateRequest();
                    var eb = Util.Embed(Color.DarkRed, "What raid day is this regarding?");
                    eb = Util.add2WeeksOfRaidsButton(eb);
                    var message = reaction.Channel.SendMessageAsync("", false, eb.Build());
                    await Util.AddReactionsSixdaysReactions(message.Result);
                }
                else if (emote == "2⃣")
                {
                    request.Request = new NotAttendingRequest();
                    var eb = Util.Embed(Color.DarkRed, "What raid day is this regarding?");
                    eb = Util.add2WeeksOfRaidsButton(eb);
                    var message = reaction.Channel.SendMessageAsync("", false, eb.Build());
                    await Util.AddReactionsSixdaysReactions(message.Result);
                }
                else if (emote == "3⃣")
                {
                    request.Request = new OMessageRequest();
                    var eb = Util.Embed(Color.DarkRed, "Whats your message?");
                    var message = reaction.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
            else if (request.Request is LateRequest)
            {
               await SendLateRequestDate(channel, msg, request, emote,reaction);
            }
            else if (request.Request is NotAttendingRequest)
            {
                await SendNotAttendingRequestDate(channel, msg, request, emote, reaction);

            }
            else if (request.Request is OMessageRequest)
            {
                await SendOMessageText(channel, msg, request, emote, reaction);

            }
        }

        private async Task SendOMessageText(IPrivateChannel channel, SocketUserMessage msg, PMRequest request, string emote, SocketReaction reaction)
        {
            var Request = (request.Request as OMessageRequest);
            if (Request.Text == null || !Request.Complete)
            {
                Request.Text = msg.Content;
                Request.Complete = true;
                request.isActive = false;
                var eb = Util.Embed(Color.DarkRed, "Thanks for your message!", "The message have been saved and sent to the officers!");
                var message = msg.Channel.SendMessageAsync("", false, eb.Build());
                await Util.SendOfficerMessage(CommandHandler._client, request);
            }
        }

        public async Task SendNotAttendingRequestText(IPrivateChannel channel, SocketUserMessage msg, PMRequest request, string emote, SocketReaction reaction)
        {
            var LateRequest = (request.Request as NotAttendingRequest);
            if (LateRequest.Reason == null || !LateRequest.Reason.Complete)
            {
                RequestReasonText te = new RequestReasonText();
                te.Complete = true;
                te.Text = msg.Content;
                LateRequest.Reason = te;

                var eb = Util.Embed(Color.DarkRed, "Thanks for your report!", "The message have been noted and sent to the officers!");
                var message = msg.Channel.SendMessageAsync("", false, eb.Build());
                request.isActive = false;
                await Util.SendOfficerMessage(CommandHandler._client, request);
            }
            else if (LateRequest.Reason.Complete)
            {

            }
        }
        public async Task SendNotAttendingRequestDate(IPrivateChannel channel, SocketUserMessage msg, PMRequest request, string emote, SocketReaction reaction)
        {
            var LateRequest = (request.Request as NotAttendingRequest);
            if (LateRequest.RaidDate == null || !LateRequest.RaidDate.Complete)
            {
                RequestRaidDate te = new RequestRaidDate();
                te.Time = Util.getDate(emote);
                te.Complete = true;
                LateRequest.RaidDate = te;


                var eb = Util.Embed(Color.DarkRed, "What is the reason for this?");
                var message = reaction.Channel.SendMessageAsync("", false, eb.Build());
            }
            else if (LateRequest.RaidDate.Complete)
            {
                await SendNotAttendingRequestText(channel, msg, request, emote, reaction);
            }
        }
        public async Task SendLateRequestDate(IPrivateChannel channel, SocketUserMessage msg, PMRequest request,string emote, SocketReaction reaction)
        {
            var LateRequest = (request.Request as LateRequest);
            if (LateRequest.RaidDate == null || !LateRequest.RaidDate.Complete)
            {
                if (!string.IsNullOrEmpty(emote))
                {


                    RequestRaidDate te = new RequestRaidDate();
                    te.Time = Util.getDate(emote);
                    te.Complete = true;
                    LateRequest.RaidDate = te;


                    var eb = Util.Embed(Color.DarkRed, "By how much will you be late?");
                    var message = reaction.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
            else if (LateRequest.RaidDate.Complete)
            {
               await SendRequestLateTime(channel, msg, request, emote,reaction);
            }
        }

        public async Task SendRequestLateTime(IPrivateChannel channel, SocketUserMessage msg, PMRequest request, string emote, SocketReaction reaction)
        {
            var LateRequest = (request.Request as LateRequest);
            if (LateRequest.HowMuchLate == null || !LateRequest.HowMuchLate.Complete)
            {
                RequestLateTime te = new RequestLateTime();
                te.Complete = true;
                te.Text = msg.Content;
                LateRequest.HowMuchLate = te;

                var eb = Util.Embed(Color.DarkRed, "What is the reason for this?");
                var message = msg.Channel.SendMessageAsync("", false, eb.Build());
            }
            else if (LateRequest.HowMuchLate.Complete)
            {
                await SendLateRequestText(channel, msg, request, emote, reaction);

            }
        }

        public async Task SendLateRequestText(IPrivateChannel channel, SocketUserMessage msg, PMRequest request,string emote, SocketReaction reaction)
        {
            var LateRequest = (request.Request as LateRequest);
            if (LateRequest.Reason == null || !LateRequest.Reason.Complete)
            {
                RequestReasonText te = new RequestReasonText();
                te.Complete = true;
                te.Text = msg.Content;
                LateRequest.Reason = te;

                var eb = Util.Embed(Color.DarkRed, "Thanks for your report!", "The message have been noted and sent to the officers!");
                var message = msg.Channel.SendMessageAsync("", false, eb.Build());
                request.isActive = false;
                await Util.SendOfficerMessage(CommandHandler._client, request);
            }
            else if (LateRequest.Reason.Complete)
            {

            }
        }
    }

}
