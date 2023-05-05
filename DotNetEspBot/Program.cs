using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace DotNetEspBot
{
    public class Program
    {

        private static TelegramBotClient botClient;

        static async Task Main(string[] args)
        {
            string telegramBotApiKey = "";

            // Load configuration from JSON file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Retrieve Telegram Bot API key from configuration
            telegramBotApiKey = configuration["TelegramBotApiKey"];

            botClient = new TelegramBotClient(telegramBotApiKey);

            botClient.OnApiResponseReceived += Bot_OnApiResponseReceived;

            //botClient.StartReceiving(new CancellationToken());

            int offset = 0;

            while (true)
            {
                var updates = await botClient.GetUpdatesAsync(offset);

                foreach (var update in updates)
                {
                    if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
                    {
                        var message = update.Message;
                        Console.WriteLine($"Received a message from {message.Chat.FirstName}: {message.Text}");

                        switch (message.Text)
                        {
                            case "activos":
                                Console.WriteLine("Usuarios activos:");
                                var chatId = message.Chat.Id;
                                var userId = message.From.Id;
                                var chatMembers = await botClient.GetChatAsync(chatId);


                                //foreach (var member in chatMembers)
                                //{
                                //    if (member.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Member)
                                //    {
                                //        var lastSeen = await botClient.GetChatMemberAsync(chatId, member.User.Id);

                                //        if (lastSeen.Status != null)
                                //        {
                                //            Console.WriteLine($"User {member.User.FirstName} is active in the group");
                                //        }
                                //        else
                                //        {
                                //            Console.WriteLine($"User {member.User.FirstName} is not active in the group");
                                //        }
                                //    }
                                //}
                                break;
                            default:
                                break;
                        }
                    }

                    offset = update.Id + 1;
                }

                await Task.Delay(1000);
            }

            Console.WriteLine("Bot started. Press any key to exit...");
            Console.ReadKey();

        }

        private static async ValueTask Bot_OnApiResponseReceived(ITelegramBotClient telegramBotClient, ApiResponseEventArgs args, CancellationToken cancellationToken)
        {
            Console.WriteLine(
                $"API response received. Method: {args.ResponseMessage.Content}, Response: {args.ResponseMessage.ReasonPhrase}");
            //if (e.Update.Type == UpdateType.Message)
            //{
            //    var message = e.Update.Message;

            //    if (message.Type == MessageType.Text)
            //    {
            //        if (message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
            //        {
            //            var chatId = message.Chat.Id;
            //            var userId = message.From.Id;

            //            var memberInfo = await botClient.GetChatMemberAsync(chatId, userId);

            //            if (memberInfo.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Administrator || memberInfo.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Creator)
            //            {
            //                // Check activity of users
            //                var chatMembers = await botClient.GetChatAdministratorsAsync(chatId);

            //                foreach (var member in chatMembers)
            //                {
            //                    if (member.Status == Telegram.Bot.Types.Enums.ChatMemberStatus.Member)
            //                    {
            //                        var lastSeen = await botClient.GetChatMemberAsync(chatId, member.User.Id);

            //                        if (lastSeen.Status != null)
            //                        {
            //                            Console.WriteLine($"User {member.User.FirstName} is active in the group");
            //                        }
            //                        else
            //                        {
            //                            Console.WriteLine($"User {member.User.FirstName} is not active in the group");
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}