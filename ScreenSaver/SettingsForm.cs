/*
 * SettingsForm.cs
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
using Microsoft.Win32;
using System.Security.Permissions;

namespace ScreenSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Load display text from the Registry
        /// </summary>
        private void LoadSettings()
        {
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\vkSharp_Demo_ScreenSaverVertexShader");

            if (key1 != null)
                textBox.Text = (string)key1.GetValue("text");

            RegistryKey key2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\vkSharp_Demo_ScreenSaverPixelShader");

            if (key2 != null)
                textBox1.Text = (string)key2.GetValue("text");

            RegistryKey key3 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\vkSharp_Demo_ScreenSaverMediaFolder");
          
        }

        /// <summary>
        /// Save text into the Registry.
        /// </summary>
        private void SaveSettings()
        {
            // Create or get existing subkey
            RegistryKey key1 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\vkSharp_Demo_ScreenSaverVertexShader");
            key1.SetValue("text", textBox.Text);

            // Create or get existing subkey
            RegistryKey key2 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\vkSharp_Demo_ScreenSaverPixelShader");
            key2.SetValue("text", textBox1.Text);
            
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
