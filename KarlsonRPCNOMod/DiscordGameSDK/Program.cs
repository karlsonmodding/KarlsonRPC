using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using Memory;


namespace KarlsonRPC
{
    class Program
    {
        public static Mem MemLib = new Mem();
        public static Dictionary<int, string> leveldict = new Dictionary<int, string>
        {
            {1, "Main Menu"},
            {2, "Tutorial"},
            {3, "Sandbox 0"},
            {4, "Sandbox 1"},
            {5, "Sandbox 2"},
            {6, "Escape 0"},
            {7, "Escape 1"},
            {8, "Escape 2"},
            {9, "Escape 3" },
            {10, "Sky 0"},
            {11, "Sky 1"},
            {12, "Sky 2"}
        };

        public static Dictionary<string, int> levellocdict = new Dictionary<string, int>
        {
            {"Assets/Scenes/MainMenu.unity", 1},
            {"Assets/Scenes/Stages/Escape/0Tut", 2},
            {"Assets/Scenes/Stages/Sanbox/1San", 3},
            {"Assets/Scenes/Stages/Sanbox/2San", 4},
            {"Assets/Scenes/Stages/Sanbox/3San", 5},
            {"Assets/Scenes/Stages/Escape/4Esc", 6},
            {"Assets/Scenes/Stages/Escape/5Esc", 7},
            {"Assets/Scenes/Stages/Escape/6Esc", 8},
            {"Assets/Scenes/Stages/Escape/7Esc", 9},
            {"Assets/Scenes/Stages/Sky/8Sky0.u", 10},
            {"Assets/Scenes/Stages/Sky/9Sky1.u", 11},
            {"Assets/Scenes/Stages/Sky/10Sky2.", 12}
        };
        
        public static Discord.Discord discord = new Discord.Discord(798687396148281404, (ulong)Discord.CreateFlags.NoRequireDiscord);
        public static TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
        public static string lastlevel = "";


        public static void Main(string[] args)
        {
            Process.Start("Karlson/Karlson.exe");
            Thread.Sleep(200);
            MemLib.OpenProcess("Karlson");

            Timer timer = new Timer(Check, "", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            Console.WriteLine("Started");
            while (true)
            {
                Thread.Sleep(10);
                discord.RunCallbacks();
            }
        }
        public static void Check(object state)
        {
            string levelLocation = MemLib.ReadString("UnityPlayer.dll+0x01683318,0x48,0x10,0x00");
            if (levelLocation == lastlevel || levelLocation == "Assets/Scenes/Initialize.unity") // Prevent updating every second if there is no need to
            {
                return;
            }
            lastlevel = levelLocation;
            if (levelLocation == "") // This will check if the game is closed
            {
                Environment.Exit(0);
            }
            int level = levellocdict[levelLocation];
            var activityManager = discord.GetActivityManager(); // Discord stuff
            string leveltxt = "";
            if (level == 1)
            {
                leveltxt = "In Main Menu";
            }
            else
            {
                leveltxt = "Playing " + leveldict[level];
            }
            var activity = new Discord.Activity
            {
                Details = leveltxt,
                Timestamps =
                {
                    Start = (int)t.TotalSeconds
                },
                Assets =
                {
                    LargeImage = leveldict[level].Replace(" ", "").ToLower(),
                    LargeText = leveldict[level]
                }
            };
            activityManager.UpdateActivity(activity, (res) => { });
        }

    }
}
