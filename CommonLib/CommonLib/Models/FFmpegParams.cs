using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Models
{
    public class FFmpegParams
    {
        #region Properties
        /// <summary>
        /// 合成视频的输出全路径
        /// </summary>
        public string OutputVideoPath
        {
            get => OutputVideoPath;
            set => OutputVideoPath = $" {value}";
        }
        #endregion

        public FFmpegParams()
        {

        }
    }
}
