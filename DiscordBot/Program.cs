using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Win32.SafeHandles;

class Program
{
    private readonly DiscordSocketClient _client;

    static void Main(string[] args)
        => new Program()
            .MainAsync()
            .GetAwaiter()
            .GetResult();

    public Program()
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        _client = new DiscordSocketClient(config);

        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
        _client.MessageReceived += MessageReceivedAsync;
        _client.InteractionCreated += InteractionCreatedAsync;
    }

    public async Task MainAsync()
    {
        await _client.LoginAsync(TokenType.Bot, "MTE5NDcwNTA3NjQzMDg0Mzk3NA.GMZ5Jg.eVsMWL2XwdTyzT6XepuPNw-3QeLfUOuIEs7JJ0");

        await _client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    private Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;
    }

    private Task ReadyAsync()
    {
        Console.WriteLine($"{_client.CurrentUser} is connected!");

        return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.Id == _client.CurrentUser.Id)
            return;


        if (message.Content.StartsWith("!metadata"))
        {
            string urlVideo = message.Content.Substring("!metadata".Length).Trim();

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:7269/api/downloader/video?_urlVideo={urlVideo}");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    await message.Channel.SendMessageAsync(responseBody);
                }
                else
                {
                    await message.Channel.SendMessageAsync($"Error al obtener metadata del video: {response.ReasonPhrase}");
                }
            }
        }
        else if (message.Content == "!ping")
        {
            var cb = new ComponentBuilder()
                .WithButton("Click me!", "unique-id", ButtonStyle.Primary);
            await message.Channel.SendMessageAsync("pong!", components: cb.Build());
        }
    }

    private async Task InteractionCreatedAsync(SocketInteraction interaction)
    {
        if (interaction is SocketMessageComponent component)
        {
            if (component.Data.CustomId == "unique-id")
                await interaction.RespondAsync("Thank you for clicking my button!");

            else
                Console.WriteLine("An ID has been received that has no handler!");
        }
    }
}

