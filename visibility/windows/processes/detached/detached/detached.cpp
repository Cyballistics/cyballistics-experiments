/**
* 
* Copyright (c) 2024-present, Cyballistics, LLC. All Rights Reserved.
* 
* License      The MIT License
* 
*-------------------------------------------------------------------------------------
* Author       carlos_diaz | @dfirence
* About        Applied Conceptual Usage Of CreateProcessA(...) with DETACHED child
* Purpose      Non Production grade, testing and teaching
*
* ------------------------------------------------------------------------------------
* 
* This program is released as a teaching instrument for practical usage.
* The student will learn the key aspects of initiating and disposing of resources 
* associated with the AMSI API - Antimalware Scanning Interface.
* 
* ------------------------------------------------------------------------------------
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
* ------------------------------------------------------------------------------------
*/
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