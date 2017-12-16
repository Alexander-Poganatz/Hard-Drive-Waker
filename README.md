# Hard-Drive-Waker
A C++ program to keep an external hard drive awake.

I compiled with Visual Studio 2017 Community. I believe any C++11 compiler should work.

Source code note:
There is a constexpr value (FILE_WRITE_INTERVAL_SECONDS) set so a file is written every 45 seconds since my external HDD goes to sleep every minute. Adjust for your needs.

Usage:
Place the compiled executable on the external HDD and use from there. Press Enter to quit.
