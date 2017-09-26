using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ShaderGenesis
{
    public partial class Form1 : Form
    {
        public GameLoop m_gameLoop;
        public string fps = string.Empty;

        #region "C++ Communication"

        public int PlayerArray;
        public IntPtr pointerToPlayerArray;
        
        #endregion "C++ Communication"

        const double PI = Math.PI;
        const double PI2 = PI * 2.0;

        int SCREEN_WIDTH = 0;
        int SCREEN_HEIGHT = 0;
      
        public Player m_Player;

        private VkPanel vkPanel;
        
        void GameLoad()
        {
           
        }

        void GameLoopSharp(double DeltaTime, double FPS)
        {
            fps = string.Format("{0}", FPS);

            m_Player.Update(DeltaTime);

            unsafe
            {
                
                
            }

            if(okToRender)
                RenderFrameOfSence();

        }

        #region "WinForms"

        public Form1()
        {

            InitializeComponent();

            //this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            //    ControlStyles.UserPaint |
            //    ControlStyles.OptimizedDoubleBuffer |
            //    ControlStyles.Opaque, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            m_Player = new Player(14.3f, 14.2f, PI * 0.3f);

            vkPanel = new VkPanel();
            vkPanel.Location = new System.Drawing.Point(0, 0);
            vkPanel.Width = 512;
            vkPanel.Height = 512;
            vkPanel.Dock = DockStyle.Fill;
            VkPanelParent.Controls.Add(vkPanel);
            vkPanel.Refresh();

            //Start the C++ Gpu Render
            int ret = StartUpVk(vkPanel.Handle, vkPanel.Width, vkPanel.Height, 1);
            //MessageBox.Show(string.Format("Result 0=Error 1=Success: {0}", ret));

            //Get info from Vulkan and C++
            int numberOfCpu = 0;
            int numberOfGpu = 0;
            GetWorkerCount(out numberOfCpu, out numberOfGpu);
            //MessageBox.Show(string.Format("Cpu Count: {0} Gpu Count: {1}", numberOfCpu, numberOfGpu));
           
            GetDisplayRect(out SCREEN_WIDTH, out SCREEN_HEIGHT);
            
            //Load Data to C++ Space
            GameLoad();

            //Enter the Thread
            m_gameLoop = new GameLoop(GameLoopSharp);

            tabPage1.Select();
        }
       
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //Call Interop to refresh Vulkan Viewport Size
            okToRender = false;

            ResizeViewport(vkPanel.Width, vkPanel.Height, 1);
            
            okToRender = true;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PostQuitMessage(0x0012);
        }

        #endregion "WinForms"

        #region "DLL Inport Windows"

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;
        private bool okToRender = true;

        [DllImport("user32.dll")]
        static extern void PostQuitMessage(int nExitCode);

        #endregion "DLL Inport Windows"

        #region "DLL Inport Windows Om1VkSharp"

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
        [DllImport("Om1VkSharp.dll", EntryPoint = "?unsafeAlloc@Om1VkSharp@@SAHHAEAHAEAPEAM@Z", CharSet = CharSet.Auto)]
        public static extern int unsafeAlloc(int SizeToAlloc, out int arraySize, out IntPtr array);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?unsafeAllocCameraInfo@Om1VkSharp@@SAHHAEAHAEAPEAM@Z", CharSet = CharSet.Auto)]
        public static extern int unsafeAllocCameraInfo(int SizeToAlloc, out int arraySize, out IntPtr array);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?unsafeAllocPayersInfo@Om1VkSharp@@SAHHAEAHAEAPEAM@Z", CharSet = CharSet.Auto)]
        public static extern int unsafeAllocPayersInfo(int SizeToAlloc, out int arraySize, out IntPtr array);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?unsafeAllocMapInfo@Om1VkSharp@@SAHHAEAHAEAPEAM@Z", CharSet = CharSet.Auto)]
        public static extern int unsafeAllocMapInfo(int SizeToAlloc, out int arraySize, out IntPtr array);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?unsafeAllocMapInfoCells@Om1VkSharp@@SAHHAEAHAEAPEAM@Z", CharSet = CharSet.Auto)]
        public static extern int unsafeAllocMapInfoCells(int SizeToAlloc, out int arraySize, out IntPtr array);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?UpdateMapFromMapCellInfoArray@Om1VkSharp@@SAXXZ", CharSet = CharSet.Auto)]
        public static extern void UpdateMapFromMapCellInfoArray();

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?GetDisplayRect@Om1VkSharp@@SAXPEAH0@Z", CharSet = CharSet.Auto)]
        public static extern int GetDisplayRect(out int p1, out int p2);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?unsafeAllocTextShader@Om1VkSharp@@SAHHAEAHAEAPEAD@Z", CharSet = CharSet.Auto)]
        public static extern int unsafeAllocTextShader(int SizeToAlloc, out int arraySize, out IntPtr array);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?unsafeLoadCompileTextPixelShader@Om1VkSharp@@SAHPEADAEAHAEAPEAD@Z", CharSet = CharSet.Auto)]
        public static extern int unsafeLoadCompileTextPixelShader([MarshalAs(UnmanagedType.LPStr)] string FileNamePtr, out int arraySize, out IntPtr array);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?ResizeViewport@Om1VkSharp@@SAXHHH@Z", CharSet = CharSet.Auto)]
        public static extern void ResizeViewport(int ScreenWidth, int ScreenHeight, int toolWindow);

        #endregion "DLL Inport Windows Om1VkSharp"


    }

}
