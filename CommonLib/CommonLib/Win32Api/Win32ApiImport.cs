using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Win32Api
{
    public class Win32ApiImport : Win32ApiStruct
    {
        #region User32.dll
        /// <summary>
        /// 获取桌面窗口的句柄
        /// </summary>
        /// <remarks>桌面窗口覆盖整个屏幕,是一个要在其上绘制所有的图标和其他窗口的区域。</remarks>
        /// <returns>返回桌面窗口的句柄</returns>
        [DllImport("User32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 获取与指定窗口有特定关系（如Z序或所有者）的窗口句柄
        /// </summary>
        /// <param name="hWnd">要获得的窗口句柄是依据nCmd参数值相对于这个窗口的句柄</param>
        /// <param name="uCmd">指定窗口与要获得句柄的窗口之间的关系（该参数值参考GetWindowCmd枚举）</param>
        /// <returns>返回值为窗口句柄（与指定窗口有特定关系的窗口不存在，则返回值为NULL）</returns>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        /// <summary>
        /// 根据窗口类名/标题名，获取窗口句柄
        /// </summary>
        /// <param name="lpClassName">整个窗口的类名</param>
        /// <param name="lpWindowName">窗口的标题</param>
        /// <returns>返回窗口的句柄（IntPtr.Zero=>没有找到窗口）</returns>
        /// <remarks>
        /// 1、如同时开多个窗口，则返回最顶层的窗口句柄；
        /// 2、窗口A、B并排,即不存在重叠(包括部分重叠),先切换窗口A，然后切换窗口B，切换别程序，会返回窗口B的句柄，反之返回窗口A的句柄;
        /// 3、两个参数可以只写一个，但另一个必须为null/string.Empty
        /// </remarks>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        protected static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 设置指定窗口的显示状态
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="command">控制窗口的显示方式</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(int hwnd, WindowState command);

        /// <summary>
        /// 激活窗口
        /// </summary>
        /// <param name="hWnd">要激活的顶级窗口的句柄</param>
        /// <remarks>窗口必须附加到调用线程的消息队列</remarks>
        /// <returns>true=>Success；false=>Failure</returns>
        [DllImport("User32.dll", EntryPoint = "SetActiveWindow")]
        private static extern bool SetActiveWindow(IntPtr hWnd);

        /// <summary>
        /// 将创建指定窗口的线程带到前台并激活该窗口
        /// </summary>
        /// <param name="hWnd">应该被激活并带到前台的窗口的句柄</param>
        /// <returns>true=>Success；false=>Failure</returns>
        /// <remarks>键盘输入直接指向窗口，并为用户改变各种视觉提示;系统给创建前台窗口的线程分配的优先级略高于其他线程</remarks>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        private static extern IntPtr GetActiveWindow();

        /// <summary>
        /// 将指定窗口标题栏的文本(如果有的话)复制到缓冲区中
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString">将接收文本的缓冲区。如果字符串与缓冲区一样长或更长，则字符串将被截断并以空字符结束</param>
        /// <param name="nMaxCount">复制到缓冲区的最大字符数，包括空字符。如果文本超过这个限制，它将被截断</param>
        /// <returns>
        /// 如果函数成功，返回值是复制字符串的长度(以字符为单位)，不包括结束的空字符。
        /// 如果窗口没有标题栏或文本，如果标题栏为空，或者窗口或控件句柄无效，则返回值为零。
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 检索指定窗口标题栏文本(如果窗口有标题栏)的字符长度
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns>窗口没有文本，则返回值为零</returns>
        [DllImport("User32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        /// <summary>
        /// 销毁指定的窗口
        /// </summary>
        /// <param name="hndw">要销毁的窗口的句柄</param>
        /// <returns></returns>
        /// <remarks>
        /// 1、该函数向窗口发送WM_DESTROY和WM_NCDESTROY消息，以使其失效，并从该窗口删除键盘焦点。该函数还会销毁窗口的菜单、刷新线程消息队列、销毁计时器、移除剪贴板所有权，并破坏剪贴板查看器链(如果窗口位于查看器链的顶部)；
        /// 2、如果指定的窗口是父窗口或所有者窗口，DestroyWindow在销毁父窗口或所有者窗口时自动销毁关联的子窗口或所有者窗口。该函数首先销毁子窗口或拥有的窗口，然后销毁父窗口或所有者窗口；
        /// 3、DestroyWindow也会销毁由CreateDialog函数创建的非模态对话框。
        /// </remarks>
        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool DestroyWindow(int hndw);

        protected delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
        [DllImport("user32.dll", EntryPoint = "EnumChildWindows", ExactSpelling = true)]
        protected static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        protected static extern int GetClassName(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 获取指定窗口的边框矩形的尺寸
        /// </summary>
        /// <remarks>
        /// 1、该尺寸以相对于屏幕坐标左上角的屏幕坐标给出；
        /// 2、如果坐标都为负值，窗口未显示
        /// </remarks>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lpRect">指向一个RECT结构的指针，该结构接收窗口的左上角和右下角的屏幕坐标</param>
        /// <returns>成功，返回值为非零：失败，返回值为零</returns>
        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        protected static extern bool GetWindowRect(IntPtr hWnd, out SRect lpRect);

        /// <summary>
        /// 移到光标至屏幕指定位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>如果成功，返回非零值；如果失败，返回值是零</returns>
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        protected static extern int SetCursorPos(int x, int y);

        [DllImport("user32", EntryPoint = "OffsetRect")]
        protected static extern int OffsetRect(ref SRect lpRect, int x, int y);

        /// <summary>
        /// 给控件发送消息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        protected static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 把坐标从当前窗体转化成全屏幕的
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ClientToScreen")]
        public static extern int ClientToScreen(int hwnd, out POINT lpPoint);

        /// <summary>
        /// 把屏幕坐标转化成相对当前窗体的坐标
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ScreenToClient")]
        public static extern int ScreenToClient(int hwnd, out POINT lpPoint);


        #endregion
    }
}
