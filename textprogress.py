from threading import currentThread, Thread
from time import sleep

def _wheel(text, interval=0.2):
    t = currentThread() #Get current thread
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


class wheel:
    def __init__(self, text, interval=0.2):
        self.interval = interval
        self.progress = Thread(
            target=_wheel,
            args=(text,),
            kwargs={"interval": interval}
            )

    def start(self):
        self.progress.start()

    def stop(self):
        self.progress.do_run = False
        self.progress.join()


