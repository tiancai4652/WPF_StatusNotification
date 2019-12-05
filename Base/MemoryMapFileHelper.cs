using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace ToastNotification.Base
{
    public class MemoryMapFileHelper<T> where T : class, new()
    {
        public MemoryMappedFile MemoryMappedFile { get; set; }
        public string MutexName { get; set; }
        public int MemoryMappedFileSize { get; set; }
        public bool IsNeedReadLock { get; set; } = false;

        public MemoryMapFileHelper(string mutexName, string memoryName, int memoryMappedFileSize = 10 * 1024 * 1024, bool isNeedReadLock = false)
        {
            MutexName = mutexName;
            MemoryMappedFileSize = memoryMappedFileSize;
            MemoryMappedFile = MemoryMappedFile.CreateOrOpen(memoryName, memoryMappedFileSize, MemoryMappedFileAccess.ReadWrite);
            IsNeedReadLock = isNeedReadLock;
        }

        public static MemoryMapFileHelper<List<MyRectangular>> GetHelper(bool isNeedReadLock = false)
        {
            return new MemoryMapFileHelper<List<MyRectangular>>("ToastNotification_Mutex", "ToastNotification_MemoryName", 10 * 1024 * 1024, isNeedReadLock);
        }

        public void Write(T entity)
        {
            //用于多进程之间对同一块内存进行读写的锁
            using (Mutex mutex = new Mutex(false, MutexName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                byte[] buf = new byte[MemoryMappedFileSize];
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, entity);
                    buf = ms.ToArray();
                }
                using (var view = MemoryMappedFile.CreateViewAccessor(0, MemoryMappedFileSize, MemoryMappedFileAccess.ReadWrite))
                {
                    view.WriteArray(0, buf, 0, buf.Count());
                }
            }
        }

        public T Read()
        {
            if (IsNeedReadLock)
            {
                using (Mutex mutex = new Mutex(false, MutexName))
                {
                    return ReadFun();
                }
            }
            else
            {
                return ReadFun();
            }
        }

        T ReadFun()
        {
            BinaryFormatter bf = new BinaryFormatter();
            T result;
            byte[] buf = new byte[MemoryMappedFileSize];
            using (var view = MemoryMappedFile.CreateViewAccessor(0, MemoryMappedFileSize, MemoryMappedFileAccess.ReadWrite))
            {
                view.ReadArray(0, buf, 0, MemoryMappedFileSize);
            }
            if (buf.All(singleByte => singleByte == 0))
            {
                return null;
            }
            using (var ms = new MemoryStream(buf))
            {
                result = bf.Deserialize(ms) as T;
            }
            return result;
        }

        public void Append()
        {

        }

    }
}
