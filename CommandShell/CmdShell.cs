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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CommandShell
{
    public class Command : Attribute
    {
        public string Name { get; set; }
    }

    public abstract class CmdShell
    {
        private class CommandParams
        {
            public string Command { get; set; }

            public string[] Args { get; set; }
        }

        protected Dictionary<string, MethodInfo> Methods { get; set; }

        public CmdShell()
        {
            Methods = new Dictionary<string, MethodInfo>();

            foreach(var method in GetType().GetMethods().Where(i => i.GetCustomAttribute(typeof(Command)) != null))
            {
                var attr = method.GetCustomAttribute<Command>();
                
                if(attr.Name != null && 
                   attr.Name != string.Empty)
                {
                    Methods.Add(attr.Name, method);
                }
            }

        }

        public MethodInfo GetMethod(string command)
        {
            MethodInfo info = null;

            Methods.TryGetValue(command, out info);

            return info;
        }

        public object ExecuteCommand(string command, string[] args)
        {
            object result = null;

            MethodInfo method = GetMethod(command);

            if (method != null)
            {
                var parameters = method.GetParameters();

                object[] values = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    values[i] = TypeDescriptor.GetConverter(parameters[i].ParameterType).ConvertFromInvariantString(args[i]);
                }


                if ((result = method.Invoke(this, values)) != null)
                    Console.WriteLine("=> {0}", result);

            }
            else
            {
                Console.WriteLine("Method not found!");
            }

            return result;
        }

        private CommandParams Parse(string line)
        {
            string[] data = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            CommandParams cp = null;

            if (data.Length > 0)
            {
                cp = new CommandParams();

                cp.Command = data[0];

                if (data.Length > 1)
                {
                    cp.Args = data.Skip(1).ToArray();
                }
            }

            return cp;
        }

        public void Run()
        {
            string line = string.Empty;

            while ((line = Console.ReadLine()) != "out")
            {
                if (line != string.Empty)
                {
                    CommandParams cp = null;

                    if ((cp = Parse(line)) != null)
                    {
                        ExecuteCommand(cp.Command, cp.Args);
                    }
                    else
                    {
                        Console.WriteLine("Wrong input!");
                    }
                }
            }
        }

        [Command(Name = "scmds")]
        public void ShowCommands()
        {
            foreach (var method in Methods.Values)
            {
                var attr = (Command)method.GetCustomAttribute(typeof(Command));

                Console.Write("{0}", attr.Name);

                var parameters = method.GetParameters();

                Console.Write(" => ");

                foreach (var p in parameters)
                {
                    Console.Write(" {0}:{1} |", p.Name, p.ParameterType.ToString().Replace("System.", ""));
                }

                Console.WriteLine("\n");
            }
        }

        [Command(Name = "open")]
        public void ReadFile(string path)
        {
            foreach(var line in File.ReadAllLines(path))
            {
                if (line != string.Empty)
                {
                    CommandParams cp = null;

                    if ((cp = Parse(line)) != null)
                    {
                        ExecuteCommand(cp.Command, cp.Args);
                    }
                    else
                    {
                        Console.WriteLine("Wrong input!");
                    }
                }
            }
        }
    }
}

