PassFruit
=========

*Secure opensource cloud password manager.*

* * *

Security
--------

PassFruit lets you manage your password in a secure way by ensuring they are never displayed in clear when not needed.

Your passwords are crypted with your master key in the client and sent crypted through the wire to the cloud where they remain crypted. When a password is requested, it is sent crypted to the client.

Your masterkey is never persisted and never sent to the server.

Only one password at a time is in memory in cleartext and they are decrypted always in the client.

If using a cloud password storage service you can enable double security that consist in requesting an access token with limited duration which is sent by email to the registered addess and enables a one-time password storage access. No one will be able to access your passwords without access to your email addess.

All the code is opensource, you can check by yourself the implementation.

* * *

Password storage services
-------------------------

You can have your crypted password persisted using the following services:

- Local XML file (Single device mode)
- Dropbox
- Windows SkyDrive
- Windows Azure (REST Service)
- AppHarbor (REST Service)

You need your own account to persist the data in the cloud.

* * *

Client password access
----------------------

This is the selection of available clients

- Windows (WPF)
- Web (HTML + JS + Crypted JSON)
- Windows Phone (Silverlight)
- Android
- Mac OS X (Cocoa)
- IOS (Cocoa Touch)

* * *

Discussion
----

[Trello PassFruit Board](https://trello.com/board/passfruit/4f1f1713ffa52a1e57084422) 
Add ideas, or claim an idea and start working on it!

[JabbR PassFruit Chatroom](http://jabbr.net/#/rooms/PassFruit)
Discuss things in real-time.

* * *

Author
------

Marco Bettiolo - [@bettiolo](https://twitter.com/bettiolo) - http://bettiolo.it

* * *

Credits
-------

I'd appreciate it to mention the use of this code somewhere if you use it in an app. On a website, in an about page, in the app itself, whatever. Or let me know by email or through github. It's nice to know where one's code is used.

* * *

License
-------

PassFruit | Copyright (C) 2012, Marco Bettiolo

* * *
