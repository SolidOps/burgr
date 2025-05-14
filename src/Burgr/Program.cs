using SolidOps.Burgr.Core;
using SolidOps.Burgr.Core.Generators;
using System.Text;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.IO;
using System.Text.Json;
using System.Reflection;

namespace SolidOps.Burgr
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            Console.WriteLine("Start Generator");
            try
            {
                return BurgrLauncher.Launch(args);
            }
            catch (Exception e)
            {
                StringBuilder stb = new();
                GetError(e, stb);
                Console.Write("Error occurred in Generation : " + stb);
                return e.Message.Contains("Could not find a part of the path") ? -1 : -1;
            }
        }

        private static void GetError(Exception e, StringBuilder stb)
        {
            _ = stb.AppendFormat(@"[{0}] {1} ({2})
StackTrace :", e.GetType().Name, e.Message, e.Data);
            _ = stb.Append(e.StackTrace + @"
");
            if (e.InnerException != null)
            {
                GetError(e.InnerException, stb);
            }
        }
    }
}
