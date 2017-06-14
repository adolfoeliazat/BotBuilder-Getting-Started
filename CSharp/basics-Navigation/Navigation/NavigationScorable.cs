﻿#pragma warning disable 1998

namespace NavigationBot.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dialogs;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using Microsoft.Bot.Builder.Scorables.Internals;
    using Microsoft.Bot.Connector;
    using Properties;

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

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

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

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}