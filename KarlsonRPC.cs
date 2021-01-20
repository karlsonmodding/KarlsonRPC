using MelonLoader;
using System.Collections.Generic;
using System;

[assembly: MelonInfo(typeof(MyProject.MyMod), "KarlsonRPC", "1.0.0", "Dark42ed#4572")]
[assembly: MelonGame("Dani", "Karlson")]


namespace MyProject
{

	public class MyMod : MelonMod
	{

		public Dictionary<int, string> leveldict = new Dictionary<int, string>
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

		public Discord.Discord discord = new Discord.Discord(798687396148281404, (ulong)Discord.CreateFlags.NoRequireDiscord);

		public override void OnUpdate()
        {
			discord.RunCallbacks();
        }

        public override void OnLevelWasInitialized(int level)
        {
			if (level == 0)
            {
				return;
            }
			var activityManager = discord.GetActivityManager();
			string leveltxt = "";
            if (level == 1)
            {
                leveltxt = "In Main Menu";
            }
			else
            {
				leveltxt = "Playing " + leveldict[level];

            }
			TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			
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