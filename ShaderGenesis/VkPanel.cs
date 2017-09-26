using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderGenesis
{
    class VkPanel : Panel
    {
        public VkPanel()
        {
        
        }

        //Prevent C# Painting
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            RenderFrameOfSence();
            
        }

        //Prevent C# Painting
        protected override void OnPaint(PaintEventArgs pe)
        {

        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?RenderFrameOfSence@Om1VkSharp@@SAXXZ", CharSet = CharSet.Auto)]
        public static extern void RenderFrameOfSence();

    }
}
