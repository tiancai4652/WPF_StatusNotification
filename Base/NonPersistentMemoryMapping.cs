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

namespace MemeoryMap
{
    /// <summary>
    /// 非持久化内存映射文件
    /// </summary>
    public class NonPersistentMemoryMapping
    {
        public string MutexName { get; set; }
        public string MemoryMappedFileName { get; set; }
        public int MemoryMappedFileSize { get; set; }
        public bool IsNeedLockReading { get; set; }

        MemoryMappedFile memoryMappedFile { get; set; }
        Mutex mutex { get; set; }

        public NonPersistentMemoryMapping(string memoryMappedFileName, string mutexName, int memoryMappedFileSize = 10 * 1024 * 1024, bool isNeedLockReading = false)
        {
            MemoryMappedFileSize = memoryMappedFileSize;
            MemoryMappedFileName = memoryMappedFileName;
            MutexName = mutexName;

            bool mutexCreated;
            mutex = new Mutex(false, MutexName, out mutexCreated);
            if (!mutexCreated)
            {
                mutex = Mutex.OpenExisting(MutexName);
            }

            try
            {
                memoryMappedFile = MemoryMappedFile.OpenExisting(MemoryMappedFileName);
            }
            catch (Exception ex)
            {
                memoryMappedFile = MemoryMappedFile.CreateNew(MemoryMappedFileName, MemoryMappedFileSize);
            }
            IsNeedLockReading = isNeedLockReading;
        }

        public void Write<T>(T Object) where T : class
        {
            mutex.WaitOne();


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

            mutex.ReleaseMutex();
        }

        public T Read<T>() where T : class
        {
            T result = null;
            if (IsNeedLockReading)
            {
                mutex.WaitOne();
            }

            BinaryFormatter bf = new BinaryFormatter();
            byte[] buf = new byte[MemoryMappedFileSize];
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


            if (IsNeedLockReading)
            {
                mutex.ReleaseMutex();
            }
            return result;
        }

        /// <summary>
        /// 当所有实例执行ManualDispose后，内存文件才真正dispose，而不是当创建memoryMappedFile的实例关闭，memoryMappedFile就dispose
        /// </summary>
        public void ManualDispose()
        {
            memoryMappedFile.Dispose();
            mutex.Dispose();
        }
    }
}
