using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using NavigationBot.Properties;

namespace NavigationBot.Dialogs
{
    [Serializable]
    public class Topic1Dialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await this.ShowMenuAsync(context);
        }

        private async Task ShowMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            
            reply.Attachments.Add(CreateHeroCardAttachment(Resources.Topic1_1_Option));
            reply.Attachments.Add(CreateHeroCardAttachment(Resources.Topic1_2_Option));
            reply.Attachments.Add(CreateHeroCardAttachment(Resources.Topic1_3_Option));

            await context.PostAsync(reply);

            context.Wait(this.ShowMenuResumeAfterAsync);
        }

        private async Task ShowMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // This is a navigation dialog, every choice should be a global command, so if we get here the user responded with something other than
            //  one of the navigation options.
            await this.StartOverAsync(context, $"I'm sorry, I don't understand '{ message.Text }'.");
        }

        private Attachment CreateHeroCardAttachment(string title)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Text = $"To learn more about { title }...",

                Images = new List<CardImage>() {
                        new CardImage($"https://placeholdit.imgix.net/~text?txtsize=28&txt={ HttpContext.Current.Server.UrlEncode(title) }&w=320&h=160") },

                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, title, value: title)
                }
            };

            return heroCard.ToAttachment();
        }


        private async Task StartOverAsync(IDialogContext context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            await this.StartOverAsync(context, message);
        }

        private async Task StartOverAsync(IDialogContext context, IMessageActivity message)
        {
            await context.PostAsync(message);
            await this.ShowMenuAsync(context);
        }
    }
}