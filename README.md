# Hello ChatApp

This solution consists on 3 .net projects:
- Backend api with a signalR implementation for the chat engine, and using AspNet Identity for user authentication and authentication;
- The database used for AspNet Identity is SqLite for simplicity purposes, so, no instalation or container needed, the database is created when the server starts up;
- Simple frontend with blazor (it was fun to write linq on frontend code);
- A console application that works decoulpled from the chat server to process commands, get stock values from a api and send them back to the chat app;
- A RabbitMq Brocker to tye up the chat server and the console application that runs the stock query commands.

# How to run?

The service was build on top of .net 6 wich is docker ready, so that's how you set it up.

Just navigate to root directory with bash or Windows Terminal and start it with:

docker-compose up

The dotnet applications will restart themselves some times, because they'll be waiting for the RabbitMq container to be healthy.

That's it, just browse http://localhost:63474 and you're good to go!
