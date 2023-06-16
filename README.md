# URLShortener
It's a url shortener project. There are 3 functions you can use.
First of them is "ShortenUrl", when you give a long url, it generates shorter your url and the function returns it back.
The  second one is "GetOriginalUrlByShortUrl",when you give short url you created before, it returns you the original url of that short url.
The last one is "CreateCustomShortUrl", you can create your custom short url in this way by giving the original url you gave the short url you want.

#Before you start use this project:
  * I use local db for this project to store records, before you start use this project you need to change db connection string with your own db connection in .appsettings.json, the parameter called "SQLServerdb".
  * You need to download this packages for this project;
    - Microsoft.EntityFrameworkCore
    - Microsoft.EntityFrameworkCore.SqlServer
    -  Microsoft.EntityFrameworkCore.Tools
    -  Microsoft.Extensions.DependencyInjector
    
