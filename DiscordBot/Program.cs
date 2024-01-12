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
        await _client.LoginAsync(TokenType.Bot, "TOKEN");

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
        try
        {
            if (message.Author.Id == _client.CurrentUser.Id)
                return;


            if (message.Content.StartsWith("!download"))
            {
                string urlVideo = message.Content.Substring("!download".Length).Trim();

                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"http://localhost:5215/api/downloader/video?_urlVideo={urlVideo}");

                    if (response.IsSuccessStatusCode)
                    {
                        var videoContent = await response.Content.ReadAsStreamAsync();
                        await message.Channel.SendFileAsync(videoContent, "video.mp4", "Aquí está tu video:");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync($"Error al obtener el video: {response.ReasonPhrase}");
                    }
                }
            }
            //else if (message.Content == "!ping")
            //{
            //    var cb = new ComponentBuilder()
            //        .WithButton("Click me!", "unique-id", ButtonStyle.Primary);
            //    await message.Channel.SendMessageAsync("pong!", components: cb.Build());
            //}

            if (message.Content.StartsWith("!music"))
            {
                string urlVideo = message.Content.Substring("!music".Length).Trim();

                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"http://localhost:5215/api/downloader/audio?_urlVideo={urlVideo}");

                    if (response.IsSuccessStatusCode)
                    {
                        var videoContent = await response.Content.ReadAsStreamAsync();
                        await message.Channel.SendFileAsync(videoContent, "audio.mp3", "`Aquí está tu audio: `");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync($"Error al obtener el video: {response.ReasonPhrase}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await message.Channel.SendMessageAsync($"Error con el bot: {ex.Message}");
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

