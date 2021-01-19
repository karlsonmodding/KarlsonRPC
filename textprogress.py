import threading
from time import sleep

def _textdot(text, interval=0.5):
    t = threading.currentThread() #Get current thread
    while getattr(t, "do_run", True): #While we should be doing
        print(f"{text}", end="\r", flush=True)
        if not getattr(t, "do_run", True): break #While only checks for the condition when it loops
        sleep(interval)
        print(f"{text}.", end="\r", flush=True)
        if not getattr(t, "do_run", True): break
        sleep(interval)
        print(f"{text}..", end="\r", flush=True)
        if not getattr(t, "do_run", True): break
        sleep(interval)
        print(f"{text}...", end="\r", flush=True)
        if not getattr(t, "do_run", True): break
        sleep(interval)
        print(f"\r{text}   ", end="\r", flush=True)
        if not getattr(t, "do_run", True): break
    print(" "*(len(text)+3), end="\r") #Clear the bar
    t.finished = True #Notify it's finished


class textdot:
    def __init__(self, text, interval=0.5):
        self.interval = interval
        self.progress = threading.Thread(
            target=_textdot,
            args=(text,),
            kwargs={"interval": interval}
            )

    def start(self):
        self.progress.start()

    def stop(self):
        self.progress.do_run = False
        while not getattr(self.progress, "finished", False):
            self.progress.join() #Make sure it's finished





def _wheel(text, interval=0.2):
    t = threading.currentThread() #Get current thread
    while getattr(t, "do_run", True): #While we should be doing
        print(f"{text} \\", end="\r", flush=True)
        if not getattr(t, "do_run", True): break #While only checks for the condition when it loops
        sleep(interval)
        print(f"{text} |", end="\r", flush=True)
        if not getattr(t, "do_run", True): break
        sleep(interval)
        print(f"{text} /", end="\r", flush=True)
        if not getattr(t, "do_run", True): break
        sleep(interval)
        print(f"{text} -", end="\r", flush=True)
        if not getattr(t, "do_run", True): break
        sleep(interval)
    print(" "*(len(text)+2), end="\r") #Clear the bar
    t.finished = True #Notify it's finished


class wheel:
    def __init__(self, text, interval=0.2):
        self.interval = interval
        self.progress = threading.Thread(
            target=_wheel,
            args=(text,),
            kwargs={"interval": interval}
            )

    def start(self):
        self.progress.start()

    def stop(self):
        self.progress.do_run = False
        while not getattr(self.progress, "finished", False):
            self.progress.join() #Make sure it's finished


