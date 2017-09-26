/*
 * ScreenSaverForm.cs
 * By Frank McCown
 * Summer 2010
 * 
 * Feel free to modify this code.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ScreenSaver
{
    public partial class ScreenSaverForm : Form
    {
        #region Win32 API functions

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private Point mouseLocation;
        private bool previewMode = false;
        private Random rand = new Random();
        public ScreenSaverForm[] screensavers;
        private bool primary;

        public ScreenSaverForm(ref ScreenSaverForm[] Screensavers)
        {
            InitializeComponent();
            screensavers = Screensavers;
        }

        public ScreenSaverForm(Rectangle Bounds, ref ScreenSaverForm[] Screensavers, bool Primary)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            screensavers = Screensavers;
            primary = Primary;

            if (primary == true)
                textLabel.Visible = false;
        }

        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(200, 200);

            // Make text smaller
            //textLabel.Font = new System.Drawing.Font("Arial", 6);

            previewMode = true;
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            LoadSettings();

            Cursor.Hide();
            TopMost = true;

            moveTimer.Interval = 10;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();
        }

        int deltaX = 5;
        int deltaY = 5;

        private void moveTimer_Tick(object sender, System.EventArgs e)
        {
            moveTimer.Enabled = false;
            // Move text to new location
            if (!primary)
            {
                textLabel.Left += deltaX;
                textLabel.Top += deltaY;

                if (textLabel.Left + textLabel.Width >= Bounds.Width)
                {
                    deltaX = -5;
                    textLabel.Left += deltaX;
                    textLabel.Left += deltaX;
                }
                else if (textLabel.Left <= 0)
                {
                    deltaX = 5;
                    textLabel.Left += deltaX;
                    textLabel.Left += deltaX;
                }

                if (textLabel.Top + textLabel.Height >= Bounds.Height)
                {
                    deltaY = -5;
                    textLabel.Top += deltaY;
                    textLabel.Top += deltaY;
                }
                else if (textLabel.Top <= 0)
                {
                    deltaY = 5;
                    textLabel.Top += deltaY;
                    textLabel.Top += deltaY;
                }


            }
            else
            {
                RenderFrameOfSence();
            }

            moveTimer.Enabled = true;
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?RenderFrameOfSence@Om1VkSharp@@SAXXZ", CharSet = CharSet.Auto)]
        public static extern void RenderFrameOfSence();

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?ShutDownVk@Om1VkSharp@@SAXXZ", CharSet = CharSet.Auto)]
        public static extern void ShutDownVk();

        private void LoadSettings()
        {
            // Use the string from the Registry if it exists
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\vkSharp_Demo_ScreenSaverVertexShader");
            if (key1 != null)
                textLabel.Text = (string)key1.GetValue("text");

            RegistryKey key2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\vkSharp_Demo_ScreenSaverPixelShader");
            //if (key2 != null)
            //    textLabel.Text = (string)key2.GetValue("text");


        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    // Terminate if mouse is moved a significant distance
                    if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                        Math.Abs(mouseLocation.Y - e.Y) > 5)
                        Application.Exit();
                }

                // Update current mouse location
                mouseLocation = e.Location;
            }
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
            {
                //Vulkan Dispose here via screensaver.Handle
                int display = 0;
                foreach (Screen screen in Screen.AllScreens)
                {
                    //Vulkan Dispose here via screensavers[display].Handle
                    if (screen.Primary)
                    {
                        ShutDownVk();
                    }

                    display++;
                }

                Application.Exit();
            }
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                int display = 0;
                foreach (Screen screen in Screen.AllScreens)
                {
                    //Vulkan Dispose here via screensavers[display].Handle
                    if (screen.Primary)
                    {
                        ShutDownVk();
                    }

                    display++;
                }

                Application.Exit();
            }
        }

        private void moveTimer_Tick_1(object sender, EventArgs e)
        {

        }
    }
}
