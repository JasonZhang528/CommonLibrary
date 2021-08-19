using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Win32Api
{
    public class Win32ApiHelper : Win32ApiImport
    {
        private static Win32ApiHelper helper = null;

        /// <summary>
        /// Private Constrator
        /// </summary>
        private Win32ApiHelper()
        {

        }

        /// <summary>
        /// 获取Win32Helper辅助类的实例对象
        /// </summary>
        /// <returns></returns>
        public static Win32ApiHelper GetInstance() => helper ??= new();

        #region user32.dll

        /// <summary>
        /// 根据窗体类名/标题，获取窗体指针
        /// </summary>
        /// <param name="className">窗体类名</param>
        /// <param name="windowName">窗体标题</param>
        /// <returns>返回窗体的句柄（IntPtr.Zero=>没有找到窗体）</returns>
        /// <remarks>两个参数可以只写一个，但另一个必须为null/string.Empty</remarks>
        public IntPtr GetWindowIp(string className, string windowName) => FindWindow(className, windowName);

        /// <summary>
        /// 获取指定窗体的边界坐标
        /// </summary>
        /// <remarks>如果坐标都为负值，窗口未显示</remarks>
        /// <param name="windowName">窗体标题</param>
        /// <returns>边界坐标（顺时针方向：上、右，下，左）</returns>
        public int[] GetWindowsRectangle(string windowName)
        {
            IntPtr window = FindWindow(null, windowName);
            GetWindowRect(window, out SRect rect);
            return new int[] { rect.Top, rect.Right, rect.Bottom, rect.Left };
        }

        /// <summary>
        /// 移动光标至屏幕指定位置
        /// </summary>
        /// <param name="x">屏幕坐标x</param>
        /// <param name="y">屏幕坐标y</param>
        /// <returns>true=>Success；false=>failure</returns>
        public bool SetCursorPosition(int x, int y) => SetCursorPos(x, y) != 0;
        #endregion


    }
}
