using System.Globalization;
using System.Text;
using Domain;
using Service.Settings;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Utilites;

namespace Service;


class MessageBuilder(Post post, Config config)
{
    private readonly Post _post = post;
    private readonly Config _config = config;

    private async Task<string> TemplateInterpolateAsync(string templateFilename)
    {
        var fullPathTemplateFilename = Path.Combine(Directory.GetCurrentDirectory(), templateFilename);
        var tpl = await System.IO.File.ReadAllTextAsync(fullPathTemplateFilename, Encoding.UTF8);
        var (size, capacity) = HumanBytes.Calculate(post.Size);
        
        return String.Format(
            new CultureInfo("en-US"),
            tpl,
            post.Title, // 0
            post.Year, // 1
            post.Developer, // 2
            post.Version, // 3
            post.Language, // 4
            post.Tags, // 5
            post.Slug, // 6
            post.Slug, // 7
            size, // 8
            capacity // 9
        );
    }

    public static async Task<IAlbumInputMedia[]> BuildAsync(Post post, Config config)
    {
        var self = new MessageBuilder(post, config);

        return
        [
            new InputMediaPhoto($"https://what2play.xyz/content/{post.Slug}/images/screenshot_0")
            {
                Caption = await self.TemplateInterpolateAsync("template.html"),
                ParseMode = ParseMode.Html,
            },
            new InputMediaPhoto($"https://what2play.xyz/content/{post.Slug}/images/screenshot_1"),
            new InputMediaPhoto($"https://what2play.xyz/content/{post.Slug}/images/screenshot_2"),
            new InputMediaPhoto($"https://what2play.xyz/content/{post.Slug}/images/screenshot_3")
        ];
    }
}
