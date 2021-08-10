using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Media
{
    public class PictureVideoTransfer
    {
        public static PictureVideoTransfer pvTransfer;
        private PictureVideoTransfer()
        {

        }

        /// <summary>
        /// 获取图片视频转换方法实例
        /// </summary>
        /// <returns></returns>
        public static PictureVideoTransfer GetInstance() => pvTransfer ?? new PictureVideoTransfer();

        [STAThread]
        public void TransferByExe(STransferParam param)
        {
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ffmpeg.exe");
            ExtractEmbeddedExe(exePath); return;
            if (!Directory.Exists(param.pictureInputDir)) throw new Exception($"{param.pictureInputDir} 图片存放文件夹不存在");
            Process p = new Process();
            p.StartInfo.FileName = exePath;
            p.StartInfo.Arguments = @"-y -r 1 -i " +
                                    param.pictureInputDir + @"FFmpeg\pic\img%2d.jpg -i " +
                                    param.audioInputPath + " -s " + $"{param.size}" + " -vcodec mpeg4" +
                                    param.videoOutDir;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            //p.ErrorDataReceived += new DataReceivedEventHandler((s, message) => { Response.Write(message.Data); });//外部程序(这里是FFMPEG)输出流时候产生的事件,这里是把流的处理过程转移到下面的方法中,详细请查阅MSDN
            p.Start();//启动线程
            p.BeginErrorReadLine();//开始异步读取
            p.WaitForExit();//阻塞等待进程结束
            p.Close();//关闭进程
            p.Dispose();//释放资源
        }

        /// <summary>
        /// 抽取嵌入的EXE资源
        /// </summary>
        /// <param name="exeOutputPath"></param>
        [STAThread]
        private void ExtractEmbeddedExe(string exeOutputPath)
        {
            string dir = Path.GetDirectoryName(exeOutputPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            //string projectName = Assembly.GetEntryAssembly().GetName().Name.ToString();//项目名
            string resourceName = "CommonLib" + ".Resources" + ".ffmpeg.exe";//命名空间+内嵌资源名称
            //内嵌资源转换成stream
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (BufferedStream inStream = new BufferedStream(stream))
            using (FileStream outStream = new FileStream(exeOutputPath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[1024];
                int length;

                while ((length = inStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, length);
                }
                outStream.Flush();
            }
        }
    }

    /// <summary>
    /// 图片转视频的参数
    /// </summary>
    public struct STransferParam
    {
        /// <summary>
        /// 图片文件夹路径
        /// </summary>
        public string pictureInputDir { get; set; }
        /// <summary>
        /// 音频文件全路径
        /// </summary>
        public string audioInputPath { get; set; }
        /// <summary>
        /// 尺寸（w x h）
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 视频输出文件全路径
        /// </summary>
        public string videoOutDir { get; set; }
    }
}
