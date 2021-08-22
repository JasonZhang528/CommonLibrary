using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Win32Api
{
    public class Win32ApiStruct
    {
        /// <summary>
        /// 窗口与要获得句柄的窗口之间的关系。
        /// </summary>
        public enum GetWindowCmd : uint
        {
            /// <summary>
            /// 返回的句柄标识了在Z序最高端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// 返回的句柄标识了在z序最低端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。
            /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。
            /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。
            /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；
            /// 如果无使能窗口，则获得的句柄与指定窗口相同。
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }

        /// <summary>
        /// 窗口信息
        /// </summary>
        protected struct SWindowInfo
        {
            /// <summary>
            /// 窗口句柄
            /// </summary>
            public IntPtr hWnd;
            /// <summary>
            /// 窗口标题
            /// </summary>
            public string szWindowName;
            /// <summary>
            /// 窗口类名
            /// </summary>
            public string szClassName;
            /// <summary>
            /// 窗口位置大小信息
            /// </summary>
            public System.Drawing.Rectangle Rect;
        }

        /// <summary>
        /// 窗口的屏幕坐标
        /// </summary>
        protected struct SRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// hwnd窗口的客户区坐标表示的点
        /// </summary>
        public struct POINT
        {
            public long x;
            public long y;
        }

        /// <summary>
        /// 控制窗口的显示方式
        /// </summary>
        public enum WindowState
        {
            /// <summary>
            /// 隐藏窗口并激活另一个窗口
            /// </summary>
            SW_HIDE = 0,
            /// <summary>
            /// 激活并显示一个窗口。如果窗口被最小化或最大化，系统会将其恢复到原来的大小和位置。应用程序应该在第一次显示窗口时指定这个标志
            /// </summary>
            SW_SHOWNORMAL,
            /// <summary>
            /// 激活窗口并将其显示为最小化的窗口
            /// </summary>
            SW_SHOWMINIMIZED,
            /// <summary>
            /// 激活窗口并将其显示为一个最大化的窗口
            /// </summary>
            SW_SHOWMAXIMIZED,
            /// <summary>
            /// 显示窗口最近的大小和位置。该值与SW_SHOWNORMAL类似，只是窗口没有被激活
            /// </summary>
            SW_SHOWNOACTIVATE,
            /// <summary>
            /// 激活窗口并以其当前大小和位置显示
            /// </summary>
            SW_SHOW,
            /// <summary>
            /// 最小化指定的窗口并按Z顺序激活下一个顶级窗口
            /// </summary>
            SW_MINIMIZE,
            /// <summary>
            /// 将窗口显示为最小化的窗口。这个值类似于SW_SHOWMINIMIZED，只是窗口没有被激活
            /// </summary>
            SW_SHOWMINNOACTIVE,
            /// <summary>
            /// 显示窗口的当前大小和位置。这个值与SW_SHOW类似，只是窗口没有被激活
            /// </summary>
            SW_SHOWNA,
            /// <summary>
            /// 激活并显示窗口。如果窗口被最小化或最大化，系统会将其恢复到原来的大小和位置。应用程序在恢复最小化的窗口时应该指定此标志
            /// </summary>
            SW_RESTORE,
            /// <summary>
            /// 根据启动应用程序的程序传递给CreateProcess函数的STARTUPINFO结构中指定的SW_值设置显示状态
            /// </summary>
            SW_SHOWDEFAULT,
            /// <summary>
            /// 最小化一个窗口，即使拥有该窗口的线程没有响应。此标志仅在最小化来自不同线程的窗口时使用
            /// </summary>
            SW_FORCEMINIMIZE
        }

        /// <summary>
        /// 常量值
        /// </summary>
        /// <remarks>WM-鼠标;WS-窗口尺寸</remarks>
        protected struct SConst
        {
            /// <summary>
            /// 鼠标点击
            /// </summary>
            public const int WM_CLICK = 0x00F5;
            /// <summary>
            /// 窗口正常
            /// </summary>
            public const int WS_SHOWNORMAL = 1;
            /// <summary>
            /// 窗口最小化
            /// </summary>
            public const int WS_SHOWMINIMIZED = 2;
            /// <summary>
            /// 窗口最大化
            /// </summary>
            public const int WS_SHOWMAXIMIZED = 3;
        }

        /// <summary>
        /// 鼠标动作类型
        /// </summary>
        [Flags]
        protected enum MouseEventFlags : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            Absolute = 0x8000
        }
    }

    /// <summary>
    /// Win32Api函数执行结果
    /// </summary>
    public struct SActionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Flag { get; private set; }
        /// <summary>
        /// 执行结果信息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Flag"></param>
        /// <param name="Message"></param>
        public SActionResult(bool Flag, string Message)
        {
            this.Flag = Flag;
            this.Message = Message;
        }
    }
}
