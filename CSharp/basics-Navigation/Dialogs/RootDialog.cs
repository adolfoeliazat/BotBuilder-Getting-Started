#pragma warning disable 1998

namespace NavigationBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Properties;

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // If the message matches a navigation command, take the correct action (post something to the conversation, call a dialog to change the conversation flow, etc.
            if (message.Text.ToLowerInvariant() == Resources.Main_Nav_Cmd.ToLowerInvariant())
            {
                await this.ShowNavMenuAsync(context);
            }
            else if (message.Text.ToLowerInvariant() == Resources.Topic1_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic1Dialog(), this.TopicX_X_DialogResumeAfter);
            }
            else if (message.Text.ToLowerInvariant() == Resources.Topic1_1_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic1_1_Dialog(), this.TopicX_X_DialogResumeAfter);
            }
            else if (message.Text.ToLowerInvariant() == Resources.Topic1_2_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic1_2_Dialog(), this.TopicX_X_DialogResumeAfter);
            }
            else if (message.Text.ToLowerInvariant() == Resources.Topic1_3_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic1_3_Dialog(), this.TopicX_X_DialogResumeAfter);
            }

            else if (message.Text.ToLowerInvariant() == Resources.Topic2_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic2Dialog(), this.TopicX_X_DialogResumeAfter);
            }
            else if (message.Text.ToLowerInvariant() == Resources.Topic2_1_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic2_1_Dialog(), this.TopicX_X_DialogResumeAfter);
            }
            else if (message.Text.ToLowerInvariant() == Resources.Topic2_2_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic2_2_Dialog(), this.TopicX_X_DialogResumeAfter);
            }
            else if (message.Text.ToLowerInvariant() == Resources.Topic2_3_Nav_Cmd.ToLowerInvariant())
            {
                context.Call(new Topic2_3_Dialog(), this.TopicX_X_DialogResumeAfter);
            }


            else
            {
                // Else something other than a navigation command was sent, and this dialog only supports navigation commands, so explain the bot doesn't understand the command.
                await this.StartOverAsync(context, string.Format(Resources.Do_Not_Understand, message.Text));
            }
        }

        private async Task TopicX_X_DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var diagResults = await result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await this.ShowNavMenuAsync(context);
            }
        }

        private async Task ShowNavMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var menuHeroCard = new HeroCard
            {
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.Topic1_Nav_Cmd, value: Resources.Topic1_Nav_Cmd),
                }
            };

            reply.Attachments.Add(menuHeroCard.ToAttachment());

            await context.PostAsync(reply);

            context.Wait(this.ShowNavMenuResumeAfterAsync);
        }

        private async Task ShowNavMenuResumeAfterAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // If we got here, it's because something other than a navigation command was sent to the bot (navigation commands are handled in NavigationScorable middleware), 
            //  and this dialog only supports navigation commands, so explain bot doesn't understand the message.
            await this.StartOverAsync(context, string.Format(Resources.Do_Not_Understand, message.Text));
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
            await this.ShowNavMenuAsync(context);
        }
    }
}