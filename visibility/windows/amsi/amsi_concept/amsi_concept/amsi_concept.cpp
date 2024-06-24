/**
* 
* Copyright (c) 2024-present, Cyballistics, LLC. All Rights Reserved.
* 
* License      The MIT License
* 
*-------------------------------------------------------------------------------------
* Author       carlos_diaz | @dfirence
* About        Applied Conceptual Usage of AMSI API.
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
#include <amsi.h>
#include <iostream>
#include <string>

void ShowBanner()
{
    std::string dashes(64, '-');
    std::string header = "\n" + dashes + "\n";
    std::string pname = header + "Cyballistics - amsi_concept.exe" + "\n" + dashes;
    std::cout << pname << std::endl;
}

bool IsMalware(AMSI_RESULT scan_result)
{
    return (scan_result == AMSI_RESULT_DETECTED);
}

std::string HResultToString(HRESULT hr)
{
    char* errorMsg = nullptr;
    DWORD size = FormatMessageA(
        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        nullptr, hr, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPSTR)&errorMsg, 0, nullptr);

    std::string errorString;
    if (size)
    {
        errorString = errorMsg;
        LocalFree(errorMsg);
    }
    else
    {
        errorString = "Unknown error";
    }

    return errorString;
}

int main()
{
    HAMSICONTEXT amsiContext = nullptr;
    HAMSISESSION amsiSession = nullptr;
    AMSI_RESULT amsiResult;
    HRESULT hrStatus;

    LPCWSTR amsiAppName = L"DefenderSquad-AMSI-Provider";
    LPCWSTR amsiContentName = L"TestContent";

    std::string msg = "";
    ShowBanner();

    // Our Test Buffer For AMSI Engine
    const char* testBuffer = "Hello From Evil World";
    ULONG bufferSize = (ULONG)strlen(testBuffer);

    // Step 1: Initialize AMSI
    hrStatus = AmsiInitialize(amsiAppName, &amsiContext);
    if (FAILED(hrStatus))
    {
        msg = "Unable to initialize AMSI: " + HResultToString(hrStatus);
        goto exitEarly;
    }

    // Step 2: Open New AMSI Session
    hrStatus = AmsiOpenSession(amsiContext, &amsiSession);
    if (FAILED(hrStatus))
    {
        msg = "Unable to open new AMSI session: " + HResultToString(hrStatus);
        goto exitProgram;
    }

    // Step 3: Scan the buffer
    hrStatus = AmsiScanBuffer(amsiContext,
        (PVOID)testBuffer,
        bufferSize,
        amsiContentName,
        amsiSession,
        &amsiResult);
    if (FAILED(hrStatus))
    {
        msg = "[error] - AmsiScanBuffer: " + HResultToString(hrStatus);
        goto exitProgram;
    }

    // Step 4: Check Scan Result Status
    if (IsMalware(amsiResult))
    {
        msg = "Scan Result: MALWARE DETECTION\n";
        goto exitProgram;
    }
    else
    {
        msg = "Scan Result: CLEAN VERDICT\n";
        goto exitProgram;
    }
    // Leaves Early, No Cleanup Needed
exitEarly:
    std::cout << msg << "Early Exit" << std::endl;
    return 1;

    // Proper AMSI Cleanup
exitProgram:
    if (amsiSession)
    {
        AmsiCloseSession(amsiContext, amsiSession);
    }
    if (amsiContext)
    {
        AmsiUninitialize(amsiContext);
    }
    msg += "\nBuffer Size: " + std::to_string(bufferSize) + "\nBuffer Content: " + testBuffer;
    std::cout << msg << std::endl;
    return 0;
}
