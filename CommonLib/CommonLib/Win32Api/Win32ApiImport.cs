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
        /// 根据窗体类名/标题名，获取窗体句柄
        /// (两个参数可以只写一个，但另一个必须为null/string.Empty)
        /// </summary>
        /// <param name="lpClassName">整个窗体的类名</param>
        /// <param name="lpWindowName">窗体的标题</param>
        /// <returns>返回窗体的句柄（IntPtr.Zero=>没有找到窗体）</returns>
        /// <remarks>
        /// 1、如同时开多个窗体，则返回最顶层的窗体句柄；
        /// 2、如果两个窗体是并排的,即不存在重叠(包括部分重叠),先切换窗体A，然后切换窗体B
        /// =>切换别程序，会返回窗体B的句柄，反之返回窗体A的句柄
        /// </remarks>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        protected static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        protected delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
        [DllImport("user32.dll", EntryPoint = "EnumChildWindows", ExactSpelling = true)]
        protected static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextW")]
        protected static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetClassNameW")]
        protected static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 移到光标至屏幕指定位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>如果成功，返回非零值；如果失败，返回值是零</returns>
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        protected static extern int SetCursorPos(int x, int y);

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
        #endregion
    }
}
