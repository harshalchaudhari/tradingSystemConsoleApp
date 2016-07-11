using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace tradingSystemConsoleApplcation
{
    public class RollingFileTraceListener : TextWriterTraceListener
    {
        private string _pathPattern;
        public string PathPattern
        {
            get { return _pathPattern; }
            set { _pathPattern = value; }
        }

        private const string CustomAttributeAppendFile = "appendFile";
        private bool _appendFile;
        private bool _appendFileInitialized = false;
        public bool AppendFile
        {
            get
            {
                if (_appendFileInitialized)
                {
                    return _appendFile;
                }
                string s = Attributes[CustomAttributeAppendFile];
                _appendFile = s == null ? true : bool.Parse(s);
                _appendFileInitialized = true;
                return _appendFile;
            }
            set
            {
                _appendFile = value;
                _appendFileInitialized = true;
            }
        }

        private string _lastFilePath;
        private DateTime _lastDateTime;

        public RollingFileTraceListener(string pathPattern)
        {
            _pathPattern = pathPattern;

            _lastDateTime = DateTime.MinValue;
            _lastFilePath = Path.GetFullPath(string.Format(_pathPattern, _lastDateTime));
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[] { CustomAttributeAppendFile };
        }

        private void EnsureWriter()
        {
            DateTime now = DateTime.Now;
            string nowFilePath = Path.GetFullPath(string.Format(_pathPattern, now));

            if (nowFilePath != _lastFilePath)
            {
                if (Writer != null)
                {
                    Writer.Dispose();
                }
                string nowDirectoryPath = Path.GetDirectoryName(nowFilePath);
                Directory.CreateDirectory(nowDirectoryPath);
                Writer = new StreamWriter(nowFilePath, AppendFile);
                _lastDateTime = now;
                _lastFilePath = nowFilePath;
            }
        }

        public override void Flush()
        {
            EnsureWriter();
            base.Flush();
        }

        public override void Write(string message)
        {
            EnsureWriter();
            base.Write(message);
        }

        public override void WriteLine(string message)
        {
            EnsureWriter();
            base.WriteLine(message);
        }
    }
}
