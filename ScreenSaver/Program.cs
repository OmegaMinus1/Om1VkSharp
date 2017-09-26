/*
 * Program.cs * By Frank McCown * Summer 2010 * * Feel free to modify this code.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenSaver
{
    static class Program
    {
        const int MaxSupportedDisplays = 8;
        static ScreenSaverForm[] screensavers = new ScreenSaverForm[MaxSupportedDisplays];

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                // Handle cases where arguments are separated by colon. 
                // Examples: /c:1234567 or /P:1234567
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                {
                    secondArgument = args[1];
                }

                if (firstArgument == "/c")           
                {
                    // Configuration mode
                    Application.Run(new SettingsForm());
                }
                else if (firstArgument == "/p")      
                {
                    // Preview mode
                    if (secondArgument == null)
                    {
                        MessageBox.Show("Sorry, but the expected window handle was not provided.", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));
                    Application.Run(new ScreenSaverForm(previewWndHandle));
                }
                else if (firstArgument == "/s")
                {
                    // Full-screen mode
                    ShowScreenSaver();
                    Application.Run();
                }
                else
                {
                    // Undefined argument
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument + "\" is not valid.", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else    
            {
                // No arguments - treat like /c
                Application.Run(new SettingsForm());
            }
        }

        /// <summary>
        /// Display the form on each of the computer's monitors.
        /// </summary>
        static void ShowScreenSaver()
        {
            int display = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                screensavers[display] = new ScreenSaverForm(screen.Bounds, ref screensavers, screen.Primary);

                //Vulkan Init Here via screensavers[display].Handle
                if (screen.Primary == true)
                {
                    //Start the C++ Gpu Render
                    int ret = StartUpVk(screensavers[display].Handle, screen.Bounds.Width, screen.Bounds.Height, 1);
                    //MessageBox.Show(string.Format("Result 0=Error 1=Success: {0}", ret));

                    screensavers[display].Show();
                }
                else
                {
                    screensavers[display].Show();
                }
                
                display++;
                if (display >= MaxSupportedDisplays)
                {
                    break;
                }
            }
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?ShutDownVk@Om1VkSharp@@SAXXZ", CharSet = CharSet.Auto)]
        public static extern void ShutDownVk();

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?StartUpVk@Om1VkSharp@@SAHPEAHH@Z", CharSet = CharSet.Auto)]
        public static extern int StartUpVk(IntPtr handle, int toolWindow);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?StartUpVk@Om1VkSharp@@SAHPEAHHHH@Z", CharSet = CharSet.Auto)]
        public static extern int StartUpVk(IntPtr handle, int ScreenWidth, int ScreenHeight, int toolWindow);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?GetWorkerCount@Om1VkSharp@@SAXPEAH0@Z", CharSet = CharSet.Auto)]
        public static extern int GetWorkerCount(out int p1, out int p2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?RenderFrameOfSence@Om1VkSharp@@SAXXZ", CharSet = CharSet.Auto)]
        public static extern void RenderFrameOfSence();

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?GetDisplayRect@Om1VkSharp@@SAXPEAH0@Z", CharSet = CharSet.Auto)]
        public static extern int GetDisplayRect(out int p1, out int p2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?ResizeViewport@Om1VkSharp@@SAXHHH@Z", CharSet = CharSet.Auto)]
        public static extern void ResizeViewport(int ScreenWidth, int ScreenHeight, int toolWindow);

    }
}
