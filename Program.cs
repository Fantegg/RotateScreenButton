using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Rotate_And_Read
{
    class Program
    {
        // The mapping of DEVMODE structure
        // Reference may be made to define a pointer to DEVMODE structure: 
        // http://msdn.microsoft.com/en-us/library/windows/desktop/dd183565(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            public short dmLogPixels;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        public class NativeMethods
        {
            // Affirms that the platform invoke
            [DllImport("user32.dll")]
            public static extern int EnumDisplaySettings(
              string deviceName, int modeNum, ref DEVMODE devMode);
            [DllImport("user32.dll")]
            public static extern int ChangeDisplaySettings(
                  ref DEVMODE devMode, int flags);

            // Control to change the screen resolution.
            public const int ENUM_CURRENT_SETTINGS = -1;
            public const int CDS_UPDATEREGISTRY = 0x01;
            public const int CDS_TEST = 0x02;
            public const int DISP_CHANGE_SUCCESSFUL = 0;
            public const int DISP_CHANGE_RESTART = 1;
            public const int DISP_CHANGE_FAILED = -1;

            // Constant definition control to change direction
            public const int DMDO_DEFAULT = 0;
            public const int DMDO_90 = 1;
            public const int DMDO_180 = 2;
            public const int DMDO_270 = 3;
        };


        static void Main(string[] args)
        { 
           SerialPort mySerialPort = new SerialPort("COM3");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            mySerialPort.Open();
            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            
        }
        private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)

        { 
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("Data Received:", indata);
            if (indata.Equals("pressed"))   
            

        }
        // Change the resolution
        public void ChangeResolution(int width, int height)
        {
            // Initialize the DEVMODE structure
            DEVMODE devmode = new DEVMODE();
            devmode.dmDeviceName = new String(new char[32]);
            devmode.dmFormName = new String(new char[32]);
            devmode.dmSize = (short)Marshal.SizeOf(devmode);

            if (0 != NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref devmode))
            {
                devmode.dmPelsWidth = width;
                devmode.dmPelsHeight = height;

                // Change the screen resolution
                int iRet = NativeMethods.ChangeDisplaySettings(ref devmode, NativeMethods.CDS_TEST);

                if (iRet == NativeMethods.DISP_CHANGE_FAILED)
                {
                    MessageBox.Show("Can't you request execution", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    iRet = NativeMethods.ChangeDisplaySettings(ref devmode, NativeMethods.CDS_UPDATEREGISTRY);

                    
                        
                          
                    }
                }
            }
        }
    }
             
        }
    }

}