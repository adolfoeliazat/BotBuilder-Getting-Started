# Navigation Sample Bot

Sample to help you add conversation navigation support into your bots.

Mobile/web navigation, navigation UI (header, breadcrumbs, links, buttons, images, back button) not only provides navigation, but promotes discoverability (you know some of the places you can navigate too), but also promotes wayfinding (users know where they came from, where they are, and most importantly how to get back, so they feel free to explore).

But, what is navigation in bots? How do you make it discoverable? How to do promote wayfinding so users feel free/safe/motivated to take the conversation where they want?

Bot Navigation

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