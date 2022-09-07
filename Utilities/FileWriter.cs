using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YoutubeAPI.Utilities
{
    public class FileWriter
    {
        private static ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();
        public void WriteData(string dataWh, string filePath)
        {
            lock_.EnterWriteLock();
            try
            {
                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {


                    byte[] dataAsByteArray = new UTF8Encoding(true).GetBytes(dataWh);
                    fs.Write(dataAsByteArray, 0, dataAsByteArray.Length);
                }
            }
            finally
            {
                lock_.ExitWriteLock();
            }
        }

        public string ReadData(string filePath)
        {
            lock_.EnterWriteLock();
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        var data = sr.ReadToEnd();
                        return data;
                    }
                }
            }
            finally
            {
                lock_.ExitWriteLock();
            }
        }
    }
}
