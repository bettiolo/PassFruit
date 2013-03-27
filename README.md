PassFruit
=========

*Open source secure cross platform cloud password manager.*



Security
--------

PassFruit lets you manage your password in a secure way by ensuring they are never displayed in clear when not needed.

Your passwords are encrypted with your master key in the client and sent encrypted through the wire to the cloud where they remain encrypted. When a password is requested, it is sent encrypted to the client.

Your masterkey is never persisted and never sent to the server.

Only one password at a time is in memory in cleartext and they are decrypted always in the client.

If using a cloud password storage service you can enable double security that consist in requesting an access token with limited duration which is sent by email to the registered addess and enables a one-time password storage access. No one will be able to access your passwords without access to your email addess.

All the code is opensource, you can check by yourself the implementation.


Client UI
---------

This is the list of the clients that are going to be implemented. 
The list is ordered by development priority, vote please to change the order:

- [Console (Windows/Linux/Mac)](https://trello.com/c/GJtSMg6d)
- [Web (HTML + JS)](https://trello.com/c/bzMgVngO)
- [Windows 8 (Modern UI, Windows Store App)](https://trello.com/c/vlpdPyvV)
- [Mac OS X (Cocoa)](https://trello.com/c/7chNcCXr)
- [Windows Phone 8](https://trello.com/c/FC1sdfXH)
- [Windows Phone (7.5)](https://trello.com/c/0a5Qc5Oi)
- [Windows Desktop (WPF)](https://trello.com/c/6cdtXXMZ)
- [iOS (Cocoa Touch)](https://trello.com/c/iLvncg3Z)
- [Android](https://trello.com/c/M26yUbVf)
- [Blackberry 10](https://trello.com/c/VwyUL5uO)
- [Linux Desktop](https://trello.com/c/Sl4vF7Tc)
- [Firefox OS](https://trello.com/c/twRmfqOl)
- [Ubuntu Touch](https://trello.com/c/RUIlO09i)


Password storage
----------------

The following back end technologies for storing the encrypted password will be implemented.
The list is ordered by development priority, vote please to change the order:

- [Json file](https://trello.com/c/nXGSmliT)
- [InMemory](https://trello.com/c/AIW4nTAW)
- [ProtoBuf file] (https://trello.com/c/RIQ7mheZ)
- [Windows Azure](https://trello.com/c/ImI6ZkrI)
- [AppHarbor](https://trello.com/c/JwbYlKC4)
- [Dropbox](https://trello.com/c/RPC56yxs)
- [MongoDB](https://trello.com/c/6kE6rdUK)
- [SQLite](https://trello.com/c/DBu49HXE)
- [SQL Server](https://trello.com/c/Gr5TaW4I)
- [Amazon](https://trello.com/c/FeQyWNUI)
- [SkyDrive](https://trello.com/c/Rqxb2V5t)
- [XML file](https://trello.com/c/ZwoEyIgn)
- [Heroku/Node](https://trello.com/c/d0U08lII)
- [CloudApp](https://trello.com/c/QiaOKOCn)


Discussion
----------

[Trello PassFruit Board](https://trello.com/board/passfruit/4f1f1713ffa52a1e57084422) 
Add ideas, or claim an idea and start working on it!

[JabbR PassFruit Chatroom](http://jabbr.net/#/rooms/PassFruit)
Discuss things in real-time.

[Ohloh Project Statistics](https://www.ohloh.net/p/passfruit)
View statistics about the project

Wiki
----
[More about PassFruit in the Wiki](https://github.com/bettiolo/PassFruit/wiki)


Author
------

[Marco Bettiolo](http://bettiolo.it) ([@bettiolo](https://twitter.com/bettiolo))



Credits
-------

I'd appreciate it to mention the use of this code somewhere if you use it in an app. On a website, in an about page, in the app itself, whatever. Or let me know by email or through github. It's nice to know where one's code is used.


High-level diagram
------------------
![alt text](docs/PassFruit-diagram.png "PassFruit diagram")


License
-------

PassFruit | Copyright (C) 2013, Marco Bettiolo
* * *
PassFruit by Marco Bettiolo is licensed under a 
[Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License](http://creativecommons.org/licenses/by-nc-sa/3.0/).
