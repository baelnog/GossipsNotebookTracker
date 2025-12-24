using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ChecklistTracker.ViewModel
{
    internal static class DisplayHardwareMapper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public uint StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool EnumDisplayDevices(string? lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        private const uint EDD_GET_DEVICE_INTERFACE_NAME = 0x00000001;

        private static readonly Dictionary<string, string> DisplayToHardwareId;
        private static readonly Dictionary<string, string> HardwareIdToDisplay;

        /// <summary>
        /// Maps display device names (e.g., "\\.\DISPLAY1") to their hardware IDs
        /// </summary>
        /// <returns>Dictionary mapping device names to hardware IDs</returns>
        static DisplayHardwareMapper()
        {
            var mapping = new Dictionary<string, string>();
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            d.cb = Marshal.SizeOf(d);

            // Enumerate all display adapters
            for (uint adapterIndex = 0; EnumDisplayDevices(null, adapterIndex, ref d, 0); adapterIndex++)
            {
                DISPLAY_DEVICE m = new DISPLAY_DEVICE();
                m.cb = Marshal.SizeOf(m);

                // Enumerate all monitors on this adapter
                for (uint monitorIndex = 0; EnumDisplayDevices(d.DeviceName, monitorIndex, ref m, EDD_GET_DEVICE_INTERFACE_NAME); monitorIndex++)
                {
                    // Map the device name to the hardware ID
                    var deviceName = m.DeviceName.Substring(0, m.DeviceName.LastIndexOf(@"\"));
                    mapping[deviceName] = m.DeviceID;
                }
            }

            DisplayToHardwareId = mapping;
            HardwareIdToDisplay = mapping.ToDictionary(kv => kv.Value, kv => kv.Key);
        }

        /// <summary>
        /// Gets the hardware ID for a specific display device name
        /// </summary>
        /// <param name="deviceName">Device name like "\\.\DISPLAY1"</param>
        /// <returns>Hardware ID or null if not found</returns>
        public static bool TryGetHardwareId(string deviceName, out string? hardwareId)
        {
            return DisplayToHardwareId.TryGetValue(deviceName, out hardwareId);
        }

        public static bool TryGetDeviceName(string hardwareId, out string? deviceName)
        {
            return HardwareIdToDisplay.TryGetValue(hardwareId, out deviceName);
        }

    }
}