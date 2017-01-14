using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace CommandShell.ConsoleApp
{
    public class RobotsShell : CmdShell
    {

        public Dictionary<string, Robot> Robots { get; private set; }


        public RobotsShell()
        {
            Robots = new Dictionary<string, Robot>();
        }
    

        [Command(Name = "create")]
        public void CreateRobot(string name, decimal balance)
        {
            var robot = new Robot
            {
                Name = name,
                Balance = balance
            };

            Robots.Add(name, robot);

            Console.WriteLine("Robot [{0}] is created.", name);
        }


        [Command(Name = "start")]
        public void StartRobot(string name)
        {
            Robot robot = null;

            if(Robots.TryGetValue(name, out robot))
            {
                Task.Run(() => { robot.Start(); });

                Thread.Sleep(3000);
            }
            else
            {
                Console.WriteLine("Robot not found!");
            }
        }


        [Command(Name = "stop")]
        public void StopRobot(string name)
        {

            Robot robot = null;

            if (Robots.TryGetValue(name, out robot))
            {
                robot.Stop();
            }
            else
            {
                Console.WriteLine("Robot not found!");
            }
        }

    }
}
