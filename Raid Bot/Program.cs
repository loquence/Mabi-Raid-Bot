using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;





namespace Raid_Bot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private System.Threading.Timer timer;
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            string token = "NDExOTI4OTY5NjkzMTAyMDgx.DWEVRg.DOCw_fH4O-SZxjP60wkccdLjss4"; // Remember to keep this private!
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();


            List<RaidTimer> raidList = new List<RaidTimer>();
            raidList = SaveRaid.DeSerializeObj();
            AddRaids(raidList);
            RaidTimer bd = new RaidTimer();
            bd.time.Add(new TimeSpan(1, 1, 1));

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }
            
        }
        private void SetUpTimer(RaidTimer raid)
        {
            List<XmlTimeSpan> l = raid.time;
            foreach (var time in l) {
                DateTime current = DateTime.Now;
                TimeSpan timeToGo = time - current.TimeOfDay;
                if (timeToGo < TimeSpan.Zero)
                {
                    return;
                }
                Console.WriteLine("The raid time is at: " + time);
                this.timer = new System.Threading.Timer(x =>
                {
                    this.RaidAlert("It totally works bro!", _client, "default");
                }, null, timeToGo, System.Threading.Timeout.InfiniteTimeSpan);
            }
        }

        private void AddRaids(List<RaidTimer> raids)
        {
            foreach (var raid in raids)
                SetUpTimer(raid);
        }

        private async Task RaidAlert(String message, DiscordSocketClient c, String name)
        {
            var channelName = (ISocketMessageChannel)c.GetChannel(411928609624686594);
            await channelName.SendMessageAsync(name + " is in 15 minutes!");
        }



        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

    }
}
