#include <windows.h>
#include <iostream>

int main()
{
    STARTUPINFOA si;
    PROCESS_INFORMATION pi;

    // Initialize memory for the structures
    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    ZeroMemory(&pi, sizeof(pi));

    // Command line to be executed
    LPCSTR commandLine = "notepad.exe";

    // Create the process
    if (!CreateProcessA(
        NULL,             // No module name (use command line)
        const_cast<LPSTR>(commandLine), // Command line
        NULL,             // Process handle not inheritable
        NULL,             // Thread handle not inheritable
        FALSE,            // Set handle inheritance to FALSE
        DETACHED_PROCESS, // Creation flags
        NULL,             // Use parent's environment block
        NULL,             // Use parent's starting directory
        &si,              // Pointer to STARTUPINFO structure
        &pi))             // Pointer to PROCESS_INFORMATION structure
    {
        std::cerr << "CreateProcessA failed (" << GetLastError() << ").\n";
        return 1;
    }

    // Successfully created the process
    std::cout << "Process created successfully.\n";

    // Close process and thread handles
    CloseHandle(pi.hProcess);
    CloseHandle(pi.hThread);

    return 0;
}