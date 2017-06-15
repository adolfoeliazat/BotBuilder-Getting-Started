# Navigation Sample Bot

A sample that shows how to add conversation navigation to a bot that allow users to easily discover and access (or navigate to) the features and capabilities of your bot.

**TBD**

### Prerequisites

To run this sample, install the prerequisites by following the steps in the [Bot Builder SDK for .NET Quickstart](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-quickstart) section of the documentation.

This sample assumes you're familiar with:
* [Bot Builder for .NET SDK](https://dev.botframework.com/)
* [Dialog API](https://docs.botframework.com/en-us/csharp/builder/sdkreference/dialogs.html)
* [Global Message Handlers](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-global-handlers)

### Overview

#### App Navigation

In mobile or web app navigation, the UI canvas includes navigation UI (a navigation header, breadcrumb, links, a back button, buttons, content, etc.) that not only provides navigation to the featuress of the app, but also provides discoverability and promotes wayfinding of the app. 

The navigation UI in apps provides discoverability of the features and content of the app just by being visible on the UI canvas. Users are accustomed to scrolling around and clicking on UI controls and content to access features and content of the app. 

The navigation UI in apps promotes wayfinding for users by communicating where they can go (navigation headers or menus, links, buttons, content), where they are (breadcrumbs, page content), and how they can get back (back button). These wayfinding tools helps users feel comfortable exploring the app. Users know where they are in the app, how they got there, and how to get back so exploring the app becomes low consequence and comfortable, so users feel free to explore.

Bots are just apps. But, what is navigation in bots? How do you make the capabilities and features of your bot discoverable? How to do promote wayfinding in a bot so users feel free, comfortable, and motivated to explore the capabilities and features of the bot?

#### Bot Navigation

In a bot, navigation is the ability for the user to change the topic of conversation. Navigation allows the user to say "I want to talk about this vs. that", changing the topic of conversation to capability of the bot.

Bots need to differentiate between the user wanting to navigate, or change the topic of conversation, from the user just replying to the current prompt in the current conversation.

Bots support navigation commands to support changes in the topic of conversation. Navigation commands are key words or phrases that the bot listens for in the conversation. If the bot receives a navigation command, the bot changes the topic of conversation that corresponds to the navigation command. For example, a bot for a chain of retail stores could listen for the word "Locations" to change the conversation to helping the user find and learn about the location near them.

Navigation commands are different from replies. Replies are responses to the current prompt in the current dialog. Replies move the current conversation flow forward. For example, if the current prompt in the conversation flow is "What is your name?", the user responding with "My name is Chris." is a reply to that question.

Bots can make their conversation navigation discoverable by introducing those navigation commands to the conversation via conversation UI (text, buttons, etc.) so the use will know where they can take the conversation and how they can take it there.

Bots can promote wayfinding in the conversation by introducing navigation commands that make it easy to revisit conversation topics or by making it easy to return to top level navigation.

#### Implementing Navigation Commands

In this sample, navigation commands are implemented via [Global Message Handlers](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-global-handlers). Review the Global Message Handler sample to understand how `Autofac`, `IScorable` and `Scorablebase` ared used in the Bot Builder SDK to implement global message middelware. In this sample, Global Message Handler middleware examines every message in the conversation looking for navigation commands. When the text of a message to the bot matches a navigation command, the bot resets the dialog stack and starts the conversation flow that corresponds to the command.

Simple text matching is used in this sample to identify navigation commands in the conversaiton, but natural language processing (for examle via [LUIS](https://www.luis.ai/) could also be used.

Navigation is made discoverable by introducing navigation UI into the conversation. This is done via menus and buttons throughout the conversation. Uesrs can can easily see how they can navigate the bot's capabilities and don't have to remember specific commands.

Wayfinding is promoted by providing conversational UI that makes it easy for the user to return to a prior topic of conversatoin or to easily be reminded of all the possible topics of conversation. These commands are available from anywhere, whether the user clicks on a button or just types the command, promotes wayfinding.

**Note:** In this sample, navigation commands like "Back" and "Cancel" aren't used, since they feel unnatural within a conversation. Rather than providing navigation commands that move the conversation back ("Back", "Cancel"), navigation UI is used to move the conversation forward. This is done by providing navigation commands that allow the user to tell you what they want to talk about next, rather than providing commands to move back in the conersation. 

### Sample Walkthrough

3 topics, each topic has 3 subtopics, could be anything...

Hierarchy

Use the Bot-to-Bot sample.




Let's look at how this is done in code.

### Code Walkthrough

#### Creating the Navigation Global Message Handler

````C#
    public class NavigationScorable : ScorableBase<IActivity, string, double>
    {
        private IDialogStack stack;
        private IDialogTask task;

        // TODO: Move navigation commands to the Root dialog so these commands are only defined in a single location.
        // List of navigation commands that will, if matched to the text of the incoming message, trigger navigation to another dialog/conversation flow.
        private List<string> navigationCommands;
        
        public NavigationScorable(IDialogStack stack, IDialogTask task)
        {
            SetField.NotNull(out this.stack, nameof(stack), stack);
            SetField.NotNull(out this.task, nameof(task), task);

            this.navigationCommands = new List<string>();

            this.navigationCommands.Add(Resources.Main_Nav_Cmd);

            // Topic 1
            this.navigationCommands.Add(Resources.Topic1_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic1_1_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic1_2_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic1_3_Nav_Cmd);

            // Topic 2
            this.navigationCommands.Add(Resources.Topic2_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic2_1_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic2_2_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic2_3_Nav_Cmd);

            // Topic 3
            this.navigationCommands.Add(Resources.Topic3_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic3_1_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic3_2_Nav_Cmd);
            this.navigationCommands.Add(Resources.Topic3_3_Nav_Cmd);
        }
````

````C#
        protected override async Task<string> PrepareAsync(IActivity activity, CancellationToken token)
        {
            var message = activity as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var command = (from cmd in this.navigationCommands
                               where message.Text.Equals(cmd, StringComparison.InvariantCultureIgnoreCase)
                               select cmd).FirstOrDefault();

                if (command != null)
                {
                    return message.Text;
                }
            }

            return null;
        }
````

````C#
        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null)
            {
                this.stack.Reset();

                var rootDialog = new RootDialog();

                await this.stack.Forward(rootDialog, null, message, CancellationToken.None);
                await this.task.PollAsync(CancellationToken.None);
            }
        }
````

#### Fielding the Navigation Command in RootDialog

````C#
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
            // Topic 1
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

            // ...

            else
            {
                // Else something other than a navigation command was sent, and this dialog only supports navigation commands, so explain the bot doesn't understand the command.
                await this.StartOverAsync(context, string.Format(Resources.Do_Not_Understand, message.Text));
            }
        }
````

````C#
        private async Task ShowNavMenuAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();

            var menuHeroCard = new HeroCard
            {
                Text = "Please choose one of the topics below. You can see these options again at anytime by saying 'Menu'".,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.Topic1_Nav_Cmd, value: Resources.Topic1_Nav_Cmd),
                    new CardAction(ActionTypes.ImBack, Resources.Topic2_Nav_Cmd, value: Resources.Topic2_Nav_Cmd),
                    new CardAction(ActionTypes.ImBack, Resources.Topic3_Nav_Cmd, value: Resources.Topic3_Nav_Cmd),
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
````

````C#
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
````

#### Topic1Dialog



#### Topic1_1_1Dialog


````C#
````


### More information

* [Bot Builder for .NET SDK](https://dev.botframework.com/)
* [Dialog API](https://docs.botframework.com/en-us/csharp/builder/sdkreference/dialogs.html)
* [Global Message Handlers](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-global-handlers)