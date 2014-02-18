MyGNotifier
===========

A simple Gmail Imap Notifier (only in Italian)
This simple application runs in the tray and programmatically scans your imap folder to check the unread messages.
When it finds any new email it shows a nice pop up, summing up its date/from/subject info.

It is only in Italian, but I already started developing the "resx" to localize it in "en-US".

Take a look at the "Program.cs":
.At first it checks if a main password is already set (string setting MAIN_PASSWORD) :
    .If yes, it checks if the user wants to be prompted with his password at any starts (bool setting MAIN_PASS_SHOW)
        .If yes it starts with MGN_LOGIN.cs
        .If not it starts with MGN_IMPOSTAZIONI.cs
    .If not, it starts with MGN_SET_PASSWORD.cs (it means it is the very first time the application is started)
.When the MGN_IMPOSTAZIONI.cs is opened for the first time, the user is prompted to insert the IMAP server, the IMAP PORT, the USERNAME and the PASSWORD to establish a connection. All these info are stored in the properties of the application and can be changed anytime.
.There're few options which can be set to customize the application (timer frequency, auto notifications, log recording, notification aspect...)

When the user starts the application from the second time onwards, it starts as a tray icon (similar to the original Gmail Notifier). The tray icon becomes green when it checks the imap folder, then gray if it doesn't find any new messages, or blue if there're new messages to be opened. There's a context menu with some options (open settings, open gmail, pop up the last notification and check now the imap folder).
Double clicking the tray icon, the application will open "gmail.google.com" with your default browser (this option can be disabled).

IMPORTANT: the application does NOT change the flag of the imap emails, but mark them as unread another time:
              var flags = new FlagCollection();
              flags.Add("Seen");
              inbox.RemoveFlags(ids[i], flags);
              

I used the Active.Up.Net library to scan the imap and the RadDesktopAlert by Telerik as the pop up. As an alternative to the Telerik tool, I also used the NotificationWindow.dll which I think it is under MIT license.

Enjoy and let me know!
