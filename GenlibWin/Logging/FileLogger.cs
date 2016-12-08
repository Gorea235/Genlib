using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Logging
{
    /// <summary>
    /// A logger which outputs to a file.
    /// </summary>
    public class FileLogger : Logger
    {
        private string CurrentFile;
        private StreamWriter writer;

        /// <summary>
        /// Constructs a new FileLogger instance.
        /// </summary>
        /// <param name="file">The path to the file to log to.</param>
        /// <param name="append">Whether to append to the file or overwrite all contents.</param>
        public FileLogger(string file, bool append = true)
        {
            CurrentFile = file;
            writer = new StreamWriter(file, append);
            OnWrite += FileLogger_OnWrite;
            OnFlush += FileLogger_OnFlush;
        }

        /// <summary>
        /// Closes the logger.
        /// </summary>
        public override void Close()
        {
            Flush();
            writer.Close();
        }

        /// <summary>
        /// Disposes the logger.
        /// </summary>
        public override void Dispose()
        {
            Close();
            writer.Dispose();
        }

        private void FileLogger_OnFlush(object sender, OnFlushEventArgs e)
        {
            writer.Flush();
        }

        private void FileLogger_OnWrite(object sender, OnWriteEventArgs e)
        {
            writer.Write(e.Written);
        }
    }
}
