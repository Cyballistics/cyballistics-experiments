using System;
using System.Runtime.InteropServices;

namespace AmsiWatcher.AmsiDefinition;


/// <summary>
/// Provides native methods for interacting with AMSI
/// (Anti-Malware Scan Interface).
/// </summary>
public static class AmsiNativeMethods
{
    private const string AmsiDll = "amsi.dll";

    /// <summary>
    /// Initializes the AMSI interface.
    /// </summary>
    /// <param name="appName">The name of the application.</param>
    /// <param name="amsiContext">The AMSI context handle.</param>
    /// <returns>Returns 0 on success, or an error code on failure.</returns>
    [DllImport(
        AmsiDll,
        EntryPoint = "AmsiInitialize",
        CallingConvention = CallingConvention.StdCall)]
    public static extern int AmsiInitialize(
        [MarshalAs(UnmanagedType.LPWStr)] string appName,
        out IntPtr amsiContext);

    /// <summary>
    /// Uninitializes the AMSI interface.
    /// </summary>
    /// <param name="amsiContext">The AMSI context handle.</param>
    [DllImport(
        AmsiDll,
        EntryPoint = "AmsiUninitialize",
        CallingConvention = CallingConvention.StdCall)]
    public static extern void AmsiUninitialize(
        IntPtr amsiContext);

    /// <summary>
    /// Opens an AMSI session.
    /// </summary>
    /// <param name="amsiContext">The AMSI context handle.</param>
    /// <param name="amsiSession">The AMSI session handle.</param>
    /// <returns>Returns 0 on success, or an error code on failure.</returns>
    [DllImport(
        AmsiDll,
        EntryPoint = "AmsiOpenSession",
        CallingConvention = CallingConvention.StdCall)]
    public static extern int AmsiOpenSession(
        IntPtr amsiContext,
        out IntPtr amsiSession);

    /// <summary>
    /// Closes an AMSI session.
    /// </summary>
    /// <param name="amsiContext">The AMSI context handle.</param>
    /// <param name="amsiSession">The AMSI session handle.</param>
    [DllImport(
        AmsiDll,
        EntryPoint = "AmsiCloseSession",
        CallingConvention = CallingConvention.StdCall)]
    public static extern void AmsiCloseSession(
        IntPtr amsiContext,
        IntPtr amsiSession);

    /// <summary>
    /// Scans a buffer for malware.
    /// </summary>
    /// <param name="amsiContext">The AMSI context handle.</param>
    /// <param name="buffer">Pointer to the buffer to be scanned.</param>
    /// <param name="length">Length of the buffer.</param>
    /// <param name="contentName">Name of the content.</param>
    /// <param name="amsiSession">The AMSI session handle.</param>
    /// <param name="result">Scan result.</param>
    /// <returns>Returns 0 on success, or an error code on failure.</returns>
    [DllImport(
        AmsiDll,
        EntryPoint = "AmsiScanBuffer",
        CallingConvention = CallingConvention.StdCall)]
    public static extern int AmsiScanBuffer(
        IntPtr amsiContext,
        IntPtr buffer,
        uint length,
        [MarshalAs(UnmanagedType.LPWStr)] string contentName,
        IntPtr amsiSession,
        out AMSI_RESULT result);

    public enum AMSI_RESULT
    {
        AMSI_RESULT_CLEAN = 0,
        AMSI_RESULT_NOT_DETECTED = 1,
        AMSI_RESULT_DETECTED = 32768
    }
}


public static class AMSIProviderBasic
{
    public static void RunTest()
    {
        IntPtr amsiContext, amsiSession;
        AmsiNativeMethods.AMSI_RESULT result;
        string amsiProviderName = "AMSI-DefenderSquad-Scanner";
        // Initialize AMSI
        int hr = AmsiNativeMethods.AmsiInitialize(
            amsiProviderName,
            out amsiContext);
        if (hr != 0)
        {
            Console.WriteLine("AmsiInitialize failed with error: " + hr);
            return;
        }

        // Open a session
        hr = AmsiNativeMethods.AmsiOpenSession(
            amsiContext,
            out amsiSession);
        if (hr != 0)
        {
            Console.WriteLine("AmsiOpenSession failed with error: " + hr);
            AmsiNativeMethods.AmsiUninitialize(amsiContext);
            return;
        }

        // The buffer to be scanned
        var eicar = "X5O!P%@AP[4\\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(
            eicar);
        uint bufferSize = (uint)buffer.Length;

        // Allocate unmanaged memory for the buffer
        IntPtr bufferPtr = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, bufferPtr, buffer.Length);

        // Scan the buffer
        hr = AmsiNativeMethods.AmsiScanBuffer(
            amsiContext,
            bufferPtr,
            bufferSize,
            "BufferScan",
            amsiSession,
            out result);
        if (hr != 0)
        {
            Console.WriteLine("AmsiScanBuffer failed with error: " + hr);
        }
        else
        {
            if (result == AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_DETECTED)
            {
                Console.WriteLine("Malware detected!");
            }
            else
            {
                Console.WriteLine("No malware detected.");
            }
        }

        // Free the allocated memory
        Marshal.FreeHGlobal(bufferPtr);

        // Close the session
        AmsiNativeMethods.AmsiCloseSession(
            amsiContext,
            amsiSession);

        // Uninitialize AMSI
        AmsiNativeMethods.AmsiUninitialize(amsiContext);

        Console.ReadKey();
    }
}