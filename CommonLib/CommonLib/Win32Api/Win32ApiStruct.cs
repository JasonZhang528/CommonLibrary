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
        /// 窗体信息
        /// </summary>
        protected struct SWindowInfo
        {
            /// <summary>
            /// 窗体句柄
            /// </summary>
            public IntPtr hWnd;
            /// <summary>
            /// 窗口标题
            /// </summary>
            public string szWindowName;
            /// <summary>
            /// 窗体类名
            /// </summary>
            public string szClassName;
            /// <summary>
            /// 窗体位置大小信息
            /// </summary>
            public System.Drawing.Rectangle Rect;
        }

        /// <summary>
        /// 窗体的屏幕坐标
        /// </summary>
        protected struct SRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// 常量值
        /// </summary>
        /// <remarks>WM-鼠标;WS-窗体尺寸</remarks>
        protected struct SConst
        {
            /// <summary>
            /// 鼠标点击
            /// </summary>
            public const int WM_CLICK = 0x00F5;
            /// <summary>
            /// 窗体正常
            /// </summary>
            public const int WS_SHOWNORMAL = 1;
            /// <summary>
            /// 窗体最小化
            /// </summary>
            public const int WS_SHOWMINIMIZED = 2;
            /// <summary>
            /// 窗体最大化
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
}
