/**
	@file hard_drive_waker.cpp
	@author Alexander Poganatz
	@author a_poganatz@outlook.com
	@author http://poganatz.ca
	@date 2017-12-16
	@version 0.1
	@brief Implementation of hard_drive_waker.cpp
*/

#include <thread>
#include <chrono>
#include <condition_variable>
#include <fstream>
#include <atomic>
#include <iostream>
#include <string>
#include <cstdio>
std::atomic_bool quitFlag;

std::condition_variable wakeCondition;
std::mutex threadMutex;

const std::string FILE_TO_WORK_WITH("ca.poganatz.hard_drive_waker_work_file.txt");

constexpr int FILE_WRITE_INTERVAL_SECONDS = 45;

/**
	classname: FileCloseWrapper
	brief: Wraps the given output file so the close method will get called in the event of an exception.
*/
class FileCloseWrapper {
	std::ofstream & m_file;

public:

	FileCloseWrapper() = delete;
	FileCloseWrapper(std::ofstream & fileRef) : m_file(fileRef) {}

	~FileCloseWrapper()
	{
		m_file.close();
	}
};

/**
	@fn fileWriteTask
	@brief writes to a file and then sleeps for FILE_WRITE_INTERVAL_SECONDS or until awoken
*/
void fileWriteTask()
{
	std::unique_lock<std::mutex> aLock(threadMutex);
	do
	{
		{
			std::ofstream file(FILE_TO_WORK_WITH);
			if (!file)
			{
				std::cout << "Error: unable to write to file." << std::endl;
				break;
			}
			FileCloseWrapper wrap(file);

			file << "Temp data." << std::endl;
		}

		if(false == quitFlag)
			wakeCondition.wait_for(aLock, std::chrono::seconds(FILE_WRITE_INTERVAL_SECONDS));

	} while (false == quitFlag);
}

/**
	@fn main
	@brief starts thread to write to file, waits for user to hit enter, and then quits.
	@param argc [in] The number of arguments the user sent
	@param argv [in] a multi-dimensional array of char pointers. Not used for now.
	@return 0
*/
int main(int argc, char** argv)
{
	quitFlag = false;

	std::cout << "Welcome to Alexander Poganatz's Hard Drive Waker!" << std::endl;

	std::thread t(fileWriteTask);

	std::cout << "Writing to file. Press Enter key to quit." << std::endl;

	std::string line;
	std::getline(std::cin, line);

	quitFlag = true;

	// Giving the other thread a bit of time in the event
	// the flag check and going to sleep happens at the same time.
	std::this_thread::sleep_for(std::chrono::milliseconds(200));

	wakeCondition.notify_all();

	t.join();

	std::remove(FILE_TO_WORK_WITH.c_str());

	return 0;
}