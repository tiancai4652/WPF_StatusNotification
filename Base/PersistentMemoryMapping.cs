using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using static System.Environment;

namespace ToastNotification.Base
{
    /// <summary>
    /// 持久化内存映射文件
    /// </summary>
    public class PersistentMemoryMapping: IDisposable
    {
        public string FileDir { get; private set; } = $"{GetFolderPath(SpecialFolder.CommonApplicationData)}\\MemoryMapping";
        public string DataName { get; set; }
        public string MemoryMappedFileName { get; set; }
        public string MutexName { get; set; }
        public int MemoryMappedFileSize { get; set; }

        string filename { get; set; }
        //MemoryMappedFile memoryMappedFile { get; set; }
        Mutex mutex { get; set; }

        public static PersistentMemoryMapping GetHelper()
        {
            return new PersistentMemoryMapping("ToastNotification_Mutex","mp.data", "ToastNotification_MemoryName");
        }

        public PersistentMemoryMapping(string memoryMappedFileName,string dataName, string mutexName, int memoryMappedFileSize = 10 * 1024 * 1024)
        {
            MemoryMappedFileName = memoryMappedFileName;
            MemoryMappedFileSize = memoryMappedFileSize;
            DataName = dataName;
            MutexName = mutexName;
            filename = $"{FileDir}\\{DataName}";
            CreatFileIfNeeded(filename);
            bool mutexCreated;
            mutex = new Mutex(false, MutexName, out mutexCreated);
            if (!mutexCreated)
            {
                mutex = Mutex.OpenExisting(MutexName);
            }
            //memoryMappedFile = MemoryMappedFile.CreateFromFile(filename, FileMode.Open, MemoryMappedFileName, MemoryMappedFileSize);

        }

        public void Write<T>(T Object) where T : class
        {
            mutex.WaitOne();
            using (var memoryMappedFile = MemoryMappedFile.CreateFromFile(filename, FileMode.Open, MemoryMappedFileName, MemoryMappedFileSize))
            {
                using (var accessor = memoryMappedFile.CreateViewAccessor(0, MemoryMappedFileSize))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    byte[] buf = new byte[MemoryMappedFileSize];
                    using (var ms = new MemoryStream())
                    {
                        bf.Serialize(ms, Object);
                        buf = ms.ToArray();
                    }
                    accessor.WriteArray(0, buf, 0, buf.Count());
                }
            }
            mutex.ReleaseMutex();
        }

        public T Read<T>() where T : class
        {
            T result = null;

            mutex.WaitOne();

            BinaryFormatter bf = new BinaryFormatter();
            byte[] buf = new byte[MemoryMappedFileSize];
            using (var memoryMappedFile = MemoryMappedFile.CreateFromFile(filename, FileMode.Open, MemoryMappedFileName, MemoryMappedFileSize))
            {
                using (var view = memoryMappedFile.CreateViewAccessor(0, MemoryMappedFileSize))
                {
                    view.ReadArray(0, buf, 0, MemoryMappedFileSize);
                }
                if (buf.All(singleByte => singleByte == 0))
                {
                    result = null;
                }
                else
                {
                    using (var ms = new MemoryStream(buf))
                    {
                        result = bf.Deserialize(ms) as T;
                    }
                }
            }

            mutex.ReleaseMutex();

            return result;
        }


        static void CreatFileIfNeeded(string path)
        {
            FileInfo fi = new FileInfo(path);
            var di = fi.Directory;
            if (!di.Exists)
                di.Create();
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
        }


        #region IDisposable Support
        public void Dispose()
        {
            mutex.Dispose();
        }
        #endregion

    }
}
