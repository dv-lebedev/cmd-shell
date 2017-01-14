using System;

namespace CommandShell.ConsoleApp
{
    public class Robot
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        private bool _isStarted = false;

        public void Start()
        {
            if (!_isStarted)
            {
                _isStarted = true;

                Console.WriteLine("Robot [{0}] is working...", Name);
            }
            else
            {
                Console.WriteLine("Robot [{0}] is already started...", Name);
            }
        }

        public void Stop()
        {
            if (_isStarted)
            {
                _isStarted = false;
                Console.WriteLine("Robot [{0}]  is stoped...", Name);
            }
            else
            {
                Console.WriteLine("Robot [{0}] is alredy stopped...", Name);
            }
        }
    }
}
