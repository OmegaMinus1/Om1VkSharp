using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace rayCasterTest
{
    public partial class Form1 : Form
    {
        public GameLoop m_gameLoop;
        public string fps = string.Empty;

        #region "C++ Communication"

        public int PlayerArray;
        public IntPtr pointerToPlayerArray;

        public int CameraArray;
        public IntPtr pointerToCameraArray;

        public int MapArray;
        public IntPtr pointerToMapArray;

        public int MapCellsArray;
        public IntPtr pointerToMapCellsArray;

        #endregion "C++ Communication"

        #region "RayCaster Members"
                
        int SCREEN_WIDTH = 0;
        int SCREEN_HEIGHT = 0;
                
        #endregion "RayCaster Members"

        #region "RayCaster Methods"

        #region "This App Helper Methods"

        /// <summary>
        /// Set the Cell with a primative for RayTracing
        /// </summary>
        /// <param name="cellx"></param>
        /// <param name="celly"></param>
        /// <param name="filename"></param>
        private void SetCell3DObject(int cellx, int celly, string filename)
        {
            
        }

        #endregion "This App Helper Methods"

        #endregion "RayCaster Methods"

        void GameLoad()
        {
            //int ret = unsafeAllocPayersInfo(16, out PlayerArray, out pointerToPlayerArray);
            //ret = unsafeAllocCameraInfo(16, out CameraArray, out pointerToCameraArray);
            //ret = unsafeAllocMapInfo(16, out MapArray, out pointerToMapArray);
            //ret = unsafeAllocMapInfoCells(m_Map.m_Grid.Length, out MapCellsArray, out pointerToMapCellsArray);

            ////Get fileSize
            //string shaderFileName = "Shaders/PixelShader.glsl";
            //FileInfo fi = new FileInfo(shaderFileName);

            //if (!fi.Exists) { return; }

            //long fileSize = fi.Length;
            //int fileArraySize = 0;
            //IntPtr pointerToTextFileArray;
            //ret = unsafeAllocTextShader((int)fileSize + 1, out fileArraySize, out pointerToTextFileArray);

            //int shaderOk = unsafeLoadCompileTextPixelShader(shaderFileName, out fileArraySize, out pointerToTextFileArray);

            unsafe
            {
                //Send Map To C++ Code Space
                //float* mapInfo = (float*)pointerToMapArray;
                //mapInfo[0] = (float)m_Map.m_fLight;
                //mapInfo[1] = (float)m_Map.m_Size;
                //mapInfo[2] = (float)m_Map.m_Grid.Length;

                //float* mapCellInfo = (float*)pointerToMapCellsArray;
                //for (int loop = 0; loop < m_Map.m_Grid.Length; loop++)
                //{
                //    mapCellInfo[loop] = (float)m_Map.m_Grid[loop].value;
                //}

                //UpdateMapFromMapCellInfoArray();
            }
        }

        void GameLoopSharp(double DeltaTime, double FPS)
        {
            fps = string.Format("{0}", FPS);

            unsafe
            {
                //float* playersInfo = (float*)pointerToPlayerArray;
                //playersInfo[0] = (float)m_Player.PosX;
                //playersInfo[1] = (float)m_Player.PosY;
                //playersInfo[2] = (float)m_Player.Peace;
                //playersInfo[3] = (float)m_Player.Direction;
                //playersInfo[4] = (float)m_Player.GetLocation().x;
                //playersInfo[5] = (float)m_Player.GetLocation().y;

                //Not Needed in RayCaster
                //float* camerasInfo = (float*)pointerToCameraArray;
                //camerasInfo[0] = (float)m_Camera.fov;
                //camerasInfo[1] = (float)m_Camera.height;
                //camerasInfo[2] = (float)m_Camera.lightRange;
                //camerasInfo[3] = (float)m_Camera.range;
                //camerasInfo[4] = (float)m_Camera.resoultion;
                //camerasInfo[5] = (float)m_Camera.scale;
                //camerasInfo[6] = (float)m_Camera.spacing;
                //camerasInfo[7] = (float)m_Camera.width;

            }

            RenderFrameOfSence();
        }

        #region "WinForms"

        public Form1()
        {
            InitializeComponent();

            //Kill all Controls on this form
            foreach (Control cntl in this.Controls)
            {
                this.Controls.Remove(cntl);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Start the C++ Gpu Render
            int ret = StartUpVk(this.Handle, 0);
            //MessageBox.Show(string.Format("Result 0=Error 1=Success: {0}", ret));

            //Get info from Vulkan and C++
            int numberOfCpu = 0;
            int numberOfGpu = 0;
            GetWorkerCount(out numberOfCpu, out numberOfGpu);
            //MessageBox.Show(string.Format("Cpu Count: {0} Gpu Count: {1}", numberOfCpu, numberOfGpu));
            
            GetDisplayRect(out SCREEN_WIDTH, out SCREEN_HEIGHT);
            
            //Turn off C# Winform Painting
            SendMessage(this.Handle, WM_SETREDRAW, false, 0);

            //Minimize and hide
            this.Hide();
            this.WindowState = FormWindowState.Minimized;

            //Load Data to C++ Space
            GameLoad();

            //Enter the Thread
            m_gameLoop = new GameLoop(GameLoopSharp);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

        }

        //Prevent C# Painting
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }

        //Prevent C# Painting
        protected override void OnPaint(PaintEventArgs pe)
        {

        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        #endregion "WinForms"

        #region "DLL Inport Windows"

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        #endregion "DLL Inport Windows"

        #region "DLL Inport Windows Om1VkSharp"

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("Om1VkSharp.dll", EntryPoint = "?StartUpVk@Om1VkSharp@@SAHPEAHH@Z", CharSet = CharSet.Auto)]
        public static extern int StartUpVk(IntPtr handle, int toolWindow);

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

       

        #endregion "DLL Inport Windows Om1VkSharp"

    }

}
