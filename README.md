# Hard-Drive-Waker
A program to keep an external hard drive awake.

I will try to make a version of this for programming languages I pick up since it introduces input/output and threading.


##C++ version
I compiled with Visual Studio 2017 Community. I believe any C++11 compiler should work.

Source code note:
There is a constexpr value (FILE_WRITE_INTERVAL_SECONDS) set so a file is written every 45 seconds since my external HDD goes to sleep every minute. Adjust for your needs.

Usage:
Place the compiled executable on the external HDD and use from there. Press Enter to quit.
