using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.Logger
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class Logger
    {
        #region Member Variables
        /// <summary>
        /// 日志队列-线程安全的阻塞队列
        /// </summary>
        protected static BlockingCollection<SLogInfo> logQueue;
        /// <summary>
        /// 日志文件的全路径
        /// </summary>
        private static string _logFilePath;
        /// <summary>
        /// 日志等级
        /// </summary>
        private static LogLevel _logLevel;

        /// <summary>
        /// Logger类是否启动
        /// </summary>
        public static bool IsSrart;
        #endregion

        #region Properties

        /// <summary>
        /// 设置日志文件名称
        /// </summary>
        public static string FileName
        {
            set
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string logFileName = string.IsNullOrWhiteSpace(value) ?
                                     $"{DateTime.Now:yyyy_MM_dd}" :
                                     $"{value}_{DateTime.Now:yyyy_MM_dd}";
                _logFilePath = Path.Combine(appPath, "Log", logFileName + ".log");
            }
        }

        /// <summary>
        /// 日志等级
        /// </summary>
        public static LogLevel LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }

        #endregion

        /// <summary>
        /// Static Contructor
        /// </summary>
        static Logger()
        {
            logQueue = new BlockingCollection<SLogInfo>();
            FileName = null;
            LogLevel = LogLevel.ALL;
        }

        /// <summary>
        /// 启动Logger
        /// </summary>
        public static void Start()
        {
            string logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
            Task.Run(WriteLogToFile);
            IsSrart = true;
        }

        /// <summary>
        /// 日志队列信息写入日志文件
        /// </summary>
        private static void WriteLogToFile()
        {
            foreach (SLogInfo logInfo in logQueue.GetConsumingEnumerable())
            {
                try
                {
                    string logMessage = logInfo.ToString();
                    using (FileStream fs = new FileStream(_logFilePath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Write, 1024, true))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(logMessage);
                        sw.Flush();//执行上一行结束后，就写入fs中
                    }
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 日志写入队列
        /// </summary>
        /// <param name="logContent"></param>
        /// <param name="level"></param>
        /// <param name="title"></param>
        private static void AddLogToQueue(string logContent, LogLevel level, string title = null)
        {
            if (!IsSrart) throw new Exception("未启动Logger类！");
            if (level < _logLevel) return;
            try
            {
                SLogInfo logInfo = new SLogInfo();
                logInfo.eventTime = DateTime.Now;
                logInfo.level = level;
                logInfo.title = title;
                logInfo.message = logContent;
                logQueue.Add(logInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Write Methods

        public static void Write(string log, string title) => AddLogToQueue(log, LogLevel.ALL, title);
        public static void WriteTrace(string log, string title) => AddLogToQueue(log, LogLevel.TRACE, title);
        public static void WriteDebug(string log, string title) => AddLogToQueue(log, LogLevel.DEBUG, title);
        public static void WriteInfo(string log, string title) => AddLogToQueue(log, LogLevel.INFO, title);
        public static void WriteWarn(string log, string title) => AddLogToQueue(log, LogLevel.WARN, title);
        public static void WriteError(string log, string title) => AddLogToQueue(log, LogLevel.ERROR, title);
        public static void WriteFatal(string log, string title) => AddLogToQueue(log, LogLevel.FATAL, title);

        #endregion

    }

    /// <summary>
    /// 日志信息结构体
    /// </summary>
    public struct SLogInfo
    {
        /// <summary>
        /// 日志发生的时间
        /// </summary>
        public DateTime eventTime { get; set; }
        /// <summary>
        /// 日志标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel level { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string message { get; set; }

        public override string ToString()
        {
            return $"{eventTime:yyyy-MM-dd HH:mm:ss:fff}\t{level}\t{title}\t{message}";
        }
    }

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// L1，打开所有日志记录
        /// </summary>
        ALL,
        /// <summary>
        /// L2,很低的日志级别，一般不会使用
        /// </summary>
        TRACE,
        /// <summary>
        /// L3,主要用于开发过程中打印一些运行信息
        /// </summary>
        DEBUG,
        /// <summary>
        /// L4,用于生产环境中输出程序运行的一些重要信息
        /// </summary>
        INFO,
        /// <summary>
        /// L5,表明会出现潜在错误的情形，有些信息不是错误信息，但是也要给程序员的一些提示
        /// </summary>
        WARN,
        /// <summary>
        /// L6,打印错误和异常信息(虽然发生错误事件，但仍然不影响系统的继续运行)
        /// </summary>
        ERROR,
        /// <summary>
        /// L7,导致应用程序退出的错误事件
        /// </summary>
        FATAL,
        /// <summary>
        /// L8,关闭所有日志记录
        /// </summary>
        OFF
    }
}
