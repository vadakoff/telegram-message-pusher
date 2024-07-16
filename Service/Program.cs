using Domain;
using Service.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Service;


class MessagePusher
{
    private readonly Config? _config;
    private readonly PostRepository _postRepository;
    private readonly ITelegramBotClient _tbot;
    private readonly ChatId _chatId;
    
    public MessagePusher(Config? config)
    {
        _config = config;
        _postRepository = new PostRepository(_config.ConnectionStrings);
        _tbot = new TelegramBotClient(_config.AccessToken);
        _chatId = new ChatId(_config.ChatId);
    }

    public async Task<bool> PushAsync()
    {
        Post? post = await _postRepository.GetPostRandomAsync();
        Console.WriteLine("{0:T}, PostId: {1}", DateTime.Now, post.PostId);
        if (post is null)
        {
            throw new Exception("out of posts");
        }

        var message = await MessageBuilder.BuildAsync(post, _config);
        var messages = await _tbot.SendMediaGroupAsync(_chatId, message);

        await _postRepository.SetFlagTrueAsync(post);
        return true;
    }
}


class Program
{
    private static async Task<int> FooAsync()
    {
        await Task.Run(() => { Console.WriteLine("Run FooAsync() {0:T}", DateTime.Now); });
        return 1;
    }

    static async Task Main(string[] args)
    {
        var config = ConfigBuilder.Build("config.json");
        var pusher = new MessagePusher(config);
        var delayer = new Delayer(TimeSpan.FromSeconds(config.LoopIntervalSeconds));

        while (true)
        {
            await delayer.RunCycle(pusher.PushAsync);
        }
    }
}