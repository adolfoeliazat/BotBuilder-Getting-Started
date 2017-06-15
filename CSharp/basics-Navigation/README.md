# Navigation Sample Bot

A sample that shows how to add conversation navigation to a bot that allow users to easily discover and access (or navigate to) the features and capabilities of your bot.

### Prerequisites

To run this sample, install the prerequisites by following the steps in the [Bot Builder SDK for .NET Quickstart](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-quickstart) section of the documentation.

This sample assumes you're familiar with:
* [Bot Builder for .NET SDK](https://dev.botframework.com/)
* [Dialog API](https://docs.botframework.com/en-us/csharp/builder/sdkreference/dialogs.html)
* [Global Message Handlers](https://docs.microsoft.com/en-us/bot-framework/dotnet/bot-builder-dotnet-global-handlers)

### Overview

In mobile or web app navigation, the UI canvas includes navigation UI (a navigation header, breadcrumb, links, a back button, buttons, content, etc.) that not only provides navigation to the featuress of the app, but also provides discoverability and promotes wayfinding of the app. 

The navigation UI in apps provides discoverability of the features and content of the app just by being visible on the UI canvas. Users are accustomed to scrolling around and clicking on UI controls and content to access features and content of the app. 

The navigation UI in apps promotes wayfinding for users by communicating where they can go (navigation headers or menus, links, buttons, content), where they are (breadcrumbs, page content), and how they can get back (back button). These wayfinding tools helps users feel comfortable exploring the app. Users know where they are in the app, how they got there, and how to get back so exploring the app becomes low consequence and comfortable, so users feel free to explore.

Bots are just apps. But, what is navigation in bots? How do you make the capabilities and features of your bot discoverable? How to do promote wayfinding in a bot so users feel free, comfortable, and motivated to explore the capabilities and features of the bot?

#### Bot Navigation

Navigation = changing the topic of covnersation. 

vs. Replies = carrying on with teh current conversation.

I want to talk aobut this vs. that.

vs. "What is your name?", "My name is Chris."

In this sample, navigation is accomplished via middleware. Examine every message, look for navigation commands that would change the topic of convesation. In this example, we pattern match commands, but could be natural language in teh future.

If the message matches a command, reset stack, start a new dialog to start a new conversation flow.

The conversation always moves forward. Rather than having back buttons, cancel buttons, the user just tells you what they want to talk about next. 

Commands are available from anywhere, whether the user lcicks on the menu button or just types menue, they can always get back to the menu, promotes wayfinding.

Buttons and menus are used to promote discoverablitity.  Can add natural langeuage later, after the user is comfortable with flow of the bot, what the bot does, more natural to talk to it at that point.

Code Walkthrough

End Results

More information