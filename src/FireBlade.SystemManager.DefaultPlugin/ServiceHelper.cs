using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FireBlade.SystemManager.DefaultPlugin
{
    public static class ServiceHelper
    {
        [DllImport("Advapi32.dll", SetLastError = true)]
        private static extern bool QueryServiceConfigW(
          nint hService,
          IntPtr lpServiceConfig,
          uint cbBufSize,
          out uint pcbBytesNeeded
        );

        private struct QUERY_SERVICE_CONFIGW
        {
            public uint dwServiceType;
            public uint dwStartType;
            public uint dwErrorControl;
            public nint lpBinaryPathName;
            public nint lpLoadOrderGroup;
            public uint dwTagId;
            public nint lpDependencies;
            public nint lpServiceStartName;
            public nint lpDisplayName;
        }

        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenSCManager(
    string lpMachineName,
    string lpDatabaseName,
    uint dwDesiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr OpenService(
            IntPtr hSCManager,
            string lpServiceName,
            uint dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CloseServiceHandle(
            IntPtr hSCObject
        );

        private const uint SC_MANAGER_CONNECT = 0x0001;
        private const uint SERVICE_QUERY_CONFIG = 0x0001;

        extension(ServiceController sc)
        {
            public ServiceStartMode StartType
            {
                get
                {
                    //try
                    //{
                    //    var serviceHandle = sc.ServiceHandle;

                    //    bool success = QueryServiceConfigW(serviceHandle.DangerousGetHandle(), IntPtr.Zero, 0, out uint bytesNeeded);

                    //    int lastError = Marshal.GetLastWin32Error();
                    //    if (lastError != ERROR_INSUFFICIENT_BUFFER)
                    //        throw new Win32Exception(lastError);

                    //    // get the info
                    //    IntPtr bufPtr = Marshal.AllocHGlobal((IntPtr)bytesNeeded);
                    //    try
                    //    {
                    //        success = QueryServiceConfigW(serviceHandle.DangerousGetHandle(), bufPtr, bytesNeeded, out bytesNeeded);
                    //        if (!success)
                    //            throw new Win32Exception();

                    //        QUERY_SERVICE_CONFIGW config = new QUERY_SERVICE_CONFIGW();
                    //        Marshal.PtrToStructure(bufPtr, config);

                    //        return (ServiceStartMode)config.dwStartType;
                    //    }
                    //    finally
                    //    {
                    //        Marshal.FreeHGlobal(bufPtr);
                    //    }
                    //}
                    //catch
                    //{
                    //    return (ServiceStartMode)(-1);
                    //}

                    IntPtr scm = OpenSCManager(".", null, SC_MANAGER_CONNECT);

                    if (scm == IntPtr.Zero)
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    IntPtr service = OpenService(
                        scm,
                        sc.ServiceName,
                        SERVICE_QUERY_CONFIG);

                    if (service == IntPtr.Zero)
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    bool success = QueryServiceConfigW(service, IntPtr.Zero, 0, out uint bytesNeeded);

                    int lastError = Marshal.GetLastWin32Error();
                    if (lastError != ERROR_INSUFFICIENT_BUFFER)
                        throw new Win32Exception(lastError);

                    // get the info
                    IntPtr bufPtr = Marshal.AllocHGlobal((IntPtr)bytesNeeded);
                    try
                    {
                        success = QueryServiceConfigW(service, bufPtr, bytesNeeded, out bytesNeeded);
                        if (!success)
                            throw new Win32Exception();

                        QUERY_SERVICE_CONFIGW config = Marshal.PtrToStructure<QUERY_SERVICE_CONFIGW>(bufPtr);
                        return (ServiceStartMode)config.dwStartType;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(bufPtr);

                        CloseServiceHandle(service);
                        CloseServiceHandle(scm);
                    }
                }
            }
        }
    }
}
