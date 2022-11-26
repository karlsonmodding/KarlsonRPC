//UnityPlayer.dll+0x01683318,0x48,0x10,0x00
use discord_rich_presence::{
    activity::{Activity, Assets, Timestamps},
    DiscordIpc, DiscordIpcClient,
};
use std::thread::sleep;
use std::time::{Duration, SystemTime};
use toy_arms::external::{read, Process};

const LEVEL_MAP: &[(&str, usize)] = &[
    ("Assets/Scenes/MainMenu.unity", 0),
    ("Assets/Scenes/Stages/Escape/0Tut", 1),
    ("Assets/Scenes/Stages/Sanbox/1San", 2),
    ("Assets/Scenes/Stages/Sanbox/2San", 3),
    ("Assets/Scenes/Stages/Sanbox/3San", 4),
    ("Assets/Scenes/Stages/Escape/4Esc", 5),
    ("Assets/Scenes/Stages/Escape/5Esc", 6),
    ("Assets/Scenes/Stages/Escape/6Esc", 7),
    ("Assets/Scenes/Stages/Escape/7Esc", 8),
    ("Assets/Scenes/Stages/Sky/8Sky0.u", 9),
    ("Assets/Scenes/Stages/Sky/9Sky1.u", 10),
    ("Assets/Scenes/Stages/Sky/10Sky2.", 11),
];

const NAME_MAP: &[&str] = &[
    "Main Menu",
    "Tutorial",
    "Sandbox 0",
    "Sandbox 1",
    "Sandbox 2",
    "Escape 0",
    "Escape 1",
    "Escape 2",
    "Escape 3",
    "Sky 0",
    "Sky 1",
    "Sky 2",
];

const CLIENT_ID: &str = "798687396148281404";

fn main() -> Result<(), Box<dyn std::error::Error>> {
    loop {
        if let Ok(process) = Process::from_process_name("Karlson.exe") {
            start_loop(process)?;
        }
        sleep(Duration::from_secs(1));
    }

    Ok(())
}

fn start_loop(process: Process) -> Result<(), Box<dyn std::error::Error>> {
    let mut client = DiscordIpcClient::new(CLIENT_ID).unwrap();
    client.connect()?;
    let mut last_level = usize::MAX;
    let start_time = SystemTime::now()
        .duration_since(SystemTime::UNIX_EPOCH)?
        .as_secs() as i64;

    println!("Start time is {}", start_time);
    loop {
        sleep(Duration::from_millis(100));
        let ptr1 = read::<usize>(
            process.process_handle,
            process.get_module_base("UnityPlayer.dll").unwrap() + 0x01683318,
        )?;
        let ptr2 = read::<usize>(process.process_handle, ptr1 + 0x48)?;
        let ptr3 = read::<usize>(process.process_handle, ptr2 + 0x10)?;
        let val = read::<[u8; 32]>(process.process_handle, ptr3)?;
        let string = core::str::from_utf8(&val)?;
        let level = if &string[0..28] == "Assets/Scenes/MainMenu.unity" {
            0
        } else {
            LEVEL_MAP.iter().find(|x| x.0 == string).unwrap().1
        };

        if last_level == level {
            continue;
        } else {
            last_level = level;
            println!("New level: {}", level);
        }

        let name = NAME_MAP[level];

        let level_text = match level {
            0 => "In Main Menu".to_string(),
            x => format!("Playing {}", NAME_MAP[x]),
        };

        let assets = Assets::new()
            .large_image("mainmenu")
            .large_text("Main Menu");

        let image_name = name.replace(' ', "").to_lowercase();
        let assets = if level != 0 {
            assets.small_image(&image_name).small_text(name)
        } else {
            assets
        };

        client.set_activity(
            Activity::new()
                .timestamps(Timestamps::new().start(start_time))
                .details(&level_text)
                .assets(assets),
        )?;
    }

    Ok(())
}

//UnityPlayer.dll+0x01683318,0x48,0x10,0x00
