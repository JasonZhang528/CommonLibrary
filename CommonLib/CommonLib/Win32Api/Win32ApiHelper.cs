﻿using System;
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
        /// 根据窗口类名/标题，获取窗口指针
        /// </summary>
        /// <param name="className">窗口类名</param>
        /// <param name="windowName">窗口标题</param>
        /// <returns>返回窗口的句柄（IntPtr.Zero=>没有找到窗口）</returns>
        /// <remarks>两个参数可以只写一个，但另一个必须为null/string.Empty</remarks>
        public IntPtr GetWindowIp(string className, string windowName) => FindWindow(className, windowName);

        /// <summary>
        /// 获取指定窗口的边界坐标
        /// </summary>
        /// <remarks>如果坐标都为负值，窗口未显示</remarks>
        /// <param name="windowName">窗口标题</param>
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

        /// <summary>
        /// 判断窗口是否打开
        /// </summary>
        /// <param name="winTitle">窗体标题</param>
        /// <param name="winClassName">窗体类名</param>
        /// <returns>true=>已打开；false=>未打开</returns>
        public bool IsOpenWindow(string winTitle, string winClassName)
        {
            bool isNullStr1 = string.IsNullOrWhiteSpace(winTitle);
            bool isNullStr2 = string.IsNullOrWhiteSpace(winClassName);
            //1、获取桌面窗口的句柄
            IntPtr desktopPtr = GetDesktopWindow();
            //2、获得一个子窗口（这通常是一个顶层窗口，当前活动的窗口）
            IntPtr winPtr = GetWindow(desktopPtr, GetWindowCmd.GW_CHILD);
            //3、循环取得桌面下的所有子窗口
            while (winPtr != IntPtr.Zero)
            {
                bool isEqual = true;
                if (!isNullStr1)
                {
                    int length = GetWindowTextLength(winPtr);
                    StringBuilder sbWinName = new StringBuilder(length + 1);
                    GetWindowText(winPtr, sbWinName, sbWinName.Capacity);
                    isEqual = sbWinName.ToString() != winTitle;
                }
                if (!isNullStr2)
                {
                    StringBuilder sbWinClassName = new StringBuilder(256);
                    GetClassName(winPtr, sbWinClassName, 256);
                    isEqual = isEqual && sbWinClassName.ToString() == winClassName;
                }
                if (!isEqual)
                {
                    //4、继续获取下一个子窗口
                    winPtr = GetWindow(winPtr, GetWindowCmd.GW_HWNDNEXT);
                }
                else
                {
                    return isEqual;
                }
            }
            return false;
        }
        #endregion


    }
}
