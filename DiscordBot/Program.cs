using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Win32.SafeHandles;

class Program
{
    private DiscordSocketClient _client;
    public string sToken = "MTE5NDcwNTA3NjQzMDg0Mzk3NA.GDzUy-.OPmmWaOayr466f61_40-qSvNggulWRpnOhcVkg";

    static async Task Main(string[] args)
    {
        await new Program().MainAsync();
    }

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, sToken);
        await _client.StartAsync();

        _client.MessageReceived += MessageReceived;

        await Task.Delay(-1);
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    private async Task MessageReceived(SocketMessage arg)
    {
        if (arg.Author.Id == _client.CurrentUser.Id || arg.Author.IsBot)
            return;

        if (arg.Content.Contains("!hola", StringComparison.OrdinalIgnoreCase))
        {
            await arg.Channel.SendMessageAsync($"¡Hola, {arg.Author.Mention}!");
        }
    }
}

