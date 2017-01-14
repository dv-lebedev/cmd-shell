/*
MIT License

Copyright(c) 2017 Denis Lebedev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
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
