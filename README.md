# CommandShell
CommandShell to interact with C# code by commands.


**[command] [arg1] [arg2] ... [argn]**


Create your own class by extending CmdShell class.
Command is a method with [Command] attribute.


**[Command(Name = "create")] void CreateRobot(string name, decimal balance);**



**>>> SHELL**

```shell

create TSLA 15000.00

start TSLA
Robot => TSLA is working...

stop TSLA
Robot TSLA  is stoped...


```

