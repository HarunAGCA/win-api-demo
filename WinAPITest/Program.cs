using System.Diagnostics;
using System.Management;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace WinAPITest;

internal class Program
{
    static void Main(string[] args)
    {
        Buzzer();
        MessageBoxExample();
        BrightnessTest();
        Console.ReadLine();

    }

    #region Buzzer
    [DllImport("Kernel32.dll")]
    static extern bool Beep(uint frequency, uint duration);

    static void Buzzer()
    {
        Beep(1500, 1000);
    }
    #endregion

    #region MessageBox
    static void MessageBoxExample()
    {
        MessageBox(IntPtr.Zero, "Hello", "Test Başlık", 0x00000002 | 0x00000010 | 0x00080000);
    }

    [DllImport("User32.dll")]
    static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);
    #endregion

    #region MessageBeep
    static void Win32Test()
    {
        MessageBeep(0x00000000L);
    }

    [DllImport("User32.dll")]
    static extern bool MessageBeep(ulong uType);
    #endregion

    #region Brightness
    static void  BrightnessTest()
    {
        Console.WriteLine("Press and hold spacebar to change brightness");
        
        ManagementClass mclass = new ManagementClass("WmiMonitorBrightness")
        {
            Scope = new ManagementScope(@"\\.\root\wmi")
        };
        var instances = mclass.GetInstances();
        foreach (ManagementObject instance in instances)
        {
            //Print the current brightness
            //Console.WriteLine((byte)instance.GetPropertyValue("CurrentBrightness"));
        }

        var m2class = new ManagementClass("WmiMonitorBrightnessMethods")
        {
            Scope = new ManagementScope(@"\\.\root\wmi")
        };
        var instances2 = m2class.GetInstances();
        foreach (ManagementObject instance in instances2)
        {
            int brigthness = 0;
            int stepfactor = 1;


            while (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {

                instance.InvokeMethod("WmiSetBrightness", new object[] { 1, brigthness });

                brigthness += stepfactor;

                if (brigthness > 100)
                {
                    brigthness = 100;
                    stepfactor = -1;
                }
                if (brigthness < 0)
                {
                    brigthness = 0;
                    stepfactor = 1;
                }

                Thread.Sleep(1);
            }
        }

    }
    #endregion

}




