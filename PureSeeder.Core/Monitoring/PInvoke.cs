using System;
using System.Runtime.InteropServices;

namespace PureSeeder.Core.Monitoring
{
    public static class PInvoke
    {
        public static void ClickInWindow(string wndName, int x, int y)
        {
            var curhWnd = ExternalDefs.GetForegroundWindow();  // Get the handle for the current foreground window

            var hWnd = FindWindow(wndName);  // Get the handle for the game
            int LParam = MakeLParam(x, y);  // create the parameter for 

            ExternalDefs.SendMessage(hWnd, (int) WMessages.WM_LBUTTONDOWN, 0, LParam);  // Click
            ExternalDefs.SendMessage(hWnd, (int) WMessages.WM_LBUTTONUP, 0, LParam);

            ExternalDefs.SetForegroundWindow(curhWnd);  // Set the foreground window back
        }

        public static void SendEnterPress(string wndName)
        {
            var curhWnd = ExternalDefs.GetForegroundWindow();
            var hWnd = FindWindow(wndName);

            ExternalDefs.SetForegroundWindow(hWnd);

            var enterPress = GetEnterInput();

            ExternalDefs.SendInput(1, enterPress, Marshal.SizeOf(typeof(INPUT)));

            ExternalDefs.SetForegroundWindow(curhWnd);
        }

        private static INPUT[] GetEnterInput()
        {
            var keyDown = new INPUT();
            keyDown.type = 1;
            keyDown.ki.wScan = 0x1C;
            keyDown.ki.time = 0;
            keyDown.ki.dwExtraInfo = IntPtr.Zero;

            return new INPUT[1]{keyDown};
        }

        public static void MinimizeWindow(string wndName)
        {
            var curhWnd = ExternalDefs.GetForegroundWindow();  // Get the handle for the current foreground window

            var hWnd = FindWindow(wndName);

            ExternalDefs.ShowWindow(hWnd, ShowWindowCommands.ForceMinimize);

            ExternalDefs.SetForegroundWindow(curhWnd);  // Set the foreground window back
        }

        public static IntPtr GetActiveWindow()
        {
            return ExternalDefs.GetActiveWindow();
        }

        public static IntPtr FindWindow(string wndName)
        {
            return ExternalDefs.FindWindow(null, wndName);
        }

        private static int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }

        public static int GetWindowState(IntPtr hWnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.Length = Marshal.SizeOf(placement);
            ExternalDefs.GetWindowPlacement(hWnd, ref placement);

            return (int)placement.ShowCmd;
        }
    }

    public static class ExternalDefs
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CreateProcess(string lpApplicationName,
           string lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes,
           ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles,
           uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory,
           [In] ref STARTUPINFO lpStartupInfo,
           out PROCESS_INFORMATION lpProcessInformation);

//        [DllImport("user32.dll", SetLastError = true)]
//        public static extern IntPtr GetActiveWindow();

        /// <summary>The GetForegroundWindow function returns a handle to the foreground window.</summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

//        [DllImport("user32.dll")]
//        public static extern IntPtr FindWindow(string lpClassName,string lpWindowName);
//        [DllImport("user32.dll")]
//        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public short wVk;
        public short wScan;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public int type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }


    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public uint dwProcessId;
        public uint dwThreadId;
    }

    public struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    public struct SECURITY_ATTRIBUTES
    {
        public int length;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct WINDOWINFO
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public WINDOWINFO(Boolean? filler)
            : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
        {
            cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int left, top, right, bottom;
    }

    ///summary>
    /// Virtual Messages
    /// </summary>
    public enum WMessages : int
    {
        WM_LBUTTONDOWN = 0x201, //Left mousebutton down
        WM_LBUTTONUP = 0x202,  //Left mousebutton up
        WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
        WM_RBUTTONDOWN = 0x204, //Right mousebutton down
        WM_RBUTTONUP = 0x205,   //Right mousebutton up
        WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
        WM_KEYDOWN = 0x100,  //Key down
        WM_KEYUP = 0x101,   //Key up
    }

    /// <summary>
    /// Virtual Keys
    /// </summary>
    public enum VKeys : int
    {
        VK_LBUTTON = 0x01,   //Left mouse button 
        VK_RBUTTON = 0x02,   //Right mouse button 
        VK_CANCEL = 0x03,   //Control-break processing 
        VK_MBUTTON = 0x04,   //Middle mouse button (three-button mouse) 
        VK_BACK = 0x08,   //BACKSPACE key 
        VK_TAB = 0x09,   //TAB key 
        VK_CLEAR = 0x0C,   //CLEAR key 
        VK_RETURN = 0x0D,   //ENTER key 
        VK_SHIFT = 0x10,   //SHIFT key 
        VK_CONTROL = 0x11,   //CTRL key 
        VK_MENU = 0x12,   //ALT key 
        VK_PAUSE = 0x13,   //PAUSE key 
        VK_CAPITAL = 0x14,   //CAPS LOCK key 
        VK_ESCAPE = 0x1B,   //ESC key 
        VK_SPACE = 0x20,   //SPACEBAR 
        VK_PRIOR = 0x21,   //PAGE UP key 
        VK_NEXT = 0x22,   //PAGE DOWN key 
        VK_END = 0x23,   //END key 
        VK_HOME = 0x24,   //HOME key 
        VK_LEFT = 0x25,   //LEFT ARROW key 
        VK_UP = 0x26,   //UP ARROW key 
        VK_RIGHT = 0x27,   //RIGHT ARROW key 
        VK_DOWN = 0x28,   //DOWN ARROW key 
        VK_SELECT = 0x29,   //SELECT key 
        VK_PRINT = 0x2A,   //PRINT key
        VK_EXECUTE = 0x2B,   //EXECUTE key 
        VK_SNAPSHOT = 0x2C,   //PRINT SCREEN key 
        VK_INSERT = 0x2D,   //INS key 
        VK_DELETE = 0x2E,   //DEL key 
        VK_HELP = 0x2F,   //HELP key
        VK_0 = 0x30,   //0 key 
        VK_1 = 0x31,   //1 key 
        VK_2 = 0x32,   //2 key 
        VK_3 = 0x33,   //3 key 
        VK_4 = 0x34,   //4 key 
        VK_5 = 0x35,   //5 key 
        VK_6 = 0x36,    //6 key 
        VK_7 = 0x37,    //7 key 
        VK_8 = 0x38,   //8 key 
        VK_9 = 0x39,    //9 key 
        VK_A = 0x41,   //A key 
        VK_B = 0x42,   //B key 
        VK_C = 0x43,   //C key 
        VK_D = 0x44,   //D key 
        VK_E = 0x45,   //E key 
        VK_F = 0x46,   //F key 
        VK_G = 0x47,   //G key 
        VK_H = 0x48,   //H key 
        VK_I = 0x49,    //I key 
        VK_J = 0x4A,   //J key 
        VK_K = 0x4B,   //K key 
        VK_L = 0x4C,   //L key 
        VK_M = 0x4D,   //M key 
        VK_N = 0x4E,    //N key 
        VK_O = 0x4F,   //O key 
        VK_P = 0x50,    //P key 
        VK_Q = 0x51,   //Q key 
        VK_R = 0x52,   //R key 
        VK_S = 0x53,   //S key 
        VK_T = 0x54,   //T key 
        VK_U = 0x55,   //U key 
        VK_V = 0x56,   //V key 
        VK_W = 0x57,   //W key 
        VK_X = 0x58,   //X key 
        VK_Y = 0x59,   //Y key 
        VK_Z = 0x5A,    //Z key
        VK_NUMPAD0 = 0x60,   //Numeric keypad 0 key 
        VK_NUMPAD1 = 0x61,   //Numeric keypad 1 key 
        VK_NUMPAD2 = 0x62,   //Numeric keypad 2 key 
        VK_NUMPAD3 = 0x63,   //Numeric keypad 3 key 
        VK_NUMPAD4 = 0x64,   //Numeric keypad 4 key 
        VK_NUMPAD5 = 0x65,   //Numeric keypad 5 key 
        VK_NUMPAD6 = 0x66,   //Numeric keypad 6 key 
        VK_NUMPAD7 = 0x67,   //Numeric keypad 7 key 
        VK_NUMPAD8 = 0x68,   //Numeric keypad 8 key 
        VK_NUMPAD9 = 0x69,   //Numeric keypad 9 key 
        VK_SEPARATOR = 0x6C,   //Separator key 
        VK_SUBTRACT = 0x6D,   //Subtract key 
        VK_DECIMAL = 0x6E,   //Decimal key 
        VK_DIVIDE = 0x6F,   //Divide key
        VK_F1 = 0x70,   //F1 key 
        VK_F2 = 0x71,   //F2 key 
        VK_F3 = 0x72,   //F3 key 
        VK_F4 = 0x73,   //F4 key 
        VK_F5 = 0x74,   //F5 key 
        VK_F6 = 0x75,   //F6 key 
        VK_F7 = 0x76,   //F7 key 
        VK_F8 = 0x77,   //F8 key 
        VK_F9 = 0x78,   //F9 key 
        VK_F10 = 0x79,   //F10 key 
        VK_F11 = 0x7A,   //F11 key 
        VK_F12 = 0x7B,   //F12 key
        VK_SCROLL = 0x91,   //SCROLL LOCK key 
        VK_LSHIFT = 0xA0,   //Left SHIFT key
        VK_RSHIFT = 0xA1,   //Right SHIFT key
        VK_LCONTROL = 0xA2,   //Left CONTROL key
        VK_RCONTROL = 0xA3,    //Right CONTROL key
        VK_LMENU = 0xA4,      //Left MENU key
        VK_RMENU = 0xA5,   //Right MENU key
        VK_PLAY = 0xFA,   //Play key
        VK_ZOOM = 0xFB, //Zoom key 
    }

    public enum DxVKeys : short
    {
        DIK_ESCAPE = 0x01,
        DIK_1 = 0x02,
        DIK_2 = 0x03,
        DIK_3 = 0x04,
        DIK_4 = 0x05,
        DIK_5 = 0x06,
        DIK_6 = 0x07,
        DIK_7 = 0x08,
        DIK_8 = 0x09,
        DIK_9 = 0x0A,
        DIK_0 = 0x0B,
        DIK_MINUS = 0x0C    /* - on main keyboard */,
        DIK_EQUALS = 0x0D,
        DIK_BACK = 0x0E    /* backspace */,
        DIK_TAB = 0x0F,
        DIK_Q = 0x10,
        DIK_W = 0x11,
        DIK_E = 0x12,
        DIK_R = 0x13,
        DIK_T = 0x14,
        DIK_Y = 0x15,
        DIK_U = 0x16,
        DIK_I = 0x17,
        DIK_O = 0x18,
        DIK_P = 0x19,
        DIK_LBRACKET = 0x1A,
        DIK_RBRACKET = 0x1B,
        DIK_RETURN = 0x1C    /* Enter on main keyboard */,
        DIK_LCONTROL = 0x1D,
        DIK_A = 0x1E,
        DIK_S = 0x1F,
        DIK_D = 0x20,
        DIK_F = 0x21,
        DIK_G = 0x22,
        DIK_H = 0x23,
        DIK_J = 0x24,
        DIK_K = 0x25,
        DIK_L = 0x26,
        DIK_SEMICOLON = 0x27,
        DIK_APOSTROPHE = 0x28,
        DIK_GRAVE = 0x29    /* accent grave */,
        DIK_LSHIFT = 0x2A,
        DIK_BACKSLASH = 0x2B,
        DIK_Z = 0x2C,
        DIK_X = 0x2D,
        DIK_C = 0x2E,
        DIK_V = 0x2F,
        DIK_B = 0x30,
        DIK_N = 0x31,
        DIK_M = 0x32,
        DIK_COMMA = 0x33,
        DIK_PERIOD = 0x34    /* . on main keyboard */,
        DIK_SLASH = 0x35    /* / on main keyboard */,
        DIK_RSHIFT = 0x36,
        DIK_MULTIPLY = 0x37    /* * on numeric keypad */,
        DIK_LMENU = 0x38    /* left Alt */,
        DIK_SPACE = 0x39,
        DIK_CAPITAL = 0x3A,
        DIK_F1 = 0x3B,
        DIK_F2 = 0x3C,
        DIK_F3 = 0x3D,
        DIK_F4 = 0x3E,
        DIK_F5 = 0x3F,
        DIK_F6 = 0x40,
        DIK_F7 = 0x41,
        DIK_F8 = 0x42,
        DIK_F9 = 0x43,
        DIK_F10 = 0x44,
        DIK_NUMLOCK = 0x45,
        DIK_SCROLL = 0x46    /* Scroll Lock */,
        DIK_NUMPAD7 = 0x47,
        DIK_NUMPAD8 = 0x48,
        DIK_NUMPAD9 = 0x49,
        DIK_SUBTRACT = 0x4A    /* - on numeric keypad */,
        DIK_NUMPAD4 = 0x4B,
        DIK_NUMPAD5 = 0x4C,
        DIK_NUMPAD6 = 0x4D,
        DIK_ADD = 0x4E    /* + on numeric keypad */,
        DIK_NUMPAD1 = 0x4F,
        DIK_NUMPAD2 = 0x50,
        DIK_NUMPAD3 = 0x51,
        DIK_NUMPAD0 = 0x52,
        DIK_DECIMAL = 0x53    /* . on numeric keypad */,
        DIK_F11 = 0x57,
        DIK_F12 = 0x58
    }

    public enum ShowWindowCommands
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,
        /// <summary>
        /// Activates and displays a window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window 
        /// for the first time.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,
        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3, // is this the right value?
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>       
        ShowMaximized = 3,
        /// <summary>
        /// Displays a window in its most recent size and position. This value 
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,
        /// <summary>
        /// Activates the window and displays it in its current size and position. 
        /// </summary>
        Show = 5,
        /// <summary>
        /// Minimizes the specified window and activates the next top-level 
        /// window in the Z order.
        /// </summary>
        Minimize = 6,
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,
        /// <summary>
        /// Displays the window in its current size and position. This value is 
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowNA = 8,
        /// <summary>
        /// Activates and displays the window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position. 
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,
        /// <summary>
        /// Sets the show state based on the SW_* value specified in the 
        /// STARTUPINFO structure passed to the CreateProcess function by the 
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,
        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
        /// that owns the window is not responding. This flag should only be 
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        /// <summary>
        /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
        /// <para>
        /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
        /// </para>
        /// </summary>
        public int Length;

        /// <summary>
        /// Specifies flags that control the position of the minimized window and the method by which the window is restored.
        /// </summary>
        public int Flags;

        /// <summary>
        /// The current show state of the window.
        /// 0 - Hidden
        /// 1 - Normal
        /// 2 - Minimized
        /// 3 = Maximized
        /// </summary>
        //public ShowWindowCommands ShowCmd;
        public int ShowCmd;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is minimized.
        /// </summary>
        public System.Drawing.Point MinPosition;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is maximized.
        /// </summary>
        public System.Drawing.Point MaxPosition;

        /// <summary>
        /// The window's coordinates when the window is in the restored position.
        /// </summary>
        public System.Drawing.Rectangle NormalPosition;

        /// <summary>
        /// Gets the default (empty) value.
        /// </summary>
        public static WINDOWPLACEMENT Default
        {
            get
            {
                WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                result.Length = Marshal.SizeOf(result);
                return result;
            }
        }
    }
}