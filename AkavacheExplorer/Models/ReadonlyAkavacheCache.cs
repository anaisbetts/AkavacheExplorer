using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ReactiveUI;

namespace Akavache.Models
{
    public class ReadonlyFileSystemProvider : IFilesystemProvider, IEnableLogger
    {
        IFilesystemProvider _inner;

        public ReadonlyFileSystemProvider(IFilesystemProvider inner = null)
        {
            _inner = inner ?? new SimpleFilesystemProvider();
        }

        public IObservable<Stream> SafeOpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share, IScheduler scheduler)
        {
            if (mode == FileMode.Create || 
                mode == FileMode.CreateNew ||
                (mode == FileMode.OpenOrCreate && !File.Exists(path)))
            {
                return Observable.Throw<Stream>(new Exception("Read only mode"));
            }
            return _inner.SafeOpenFileAsync(path, mode, FileAccess.Read, share, scheduler);
        }

        public void CreateRecursive(string path)
        {
            _inner.CreateRecursive(path);
        }

        public void Delete(string path)
        {
            this.Log().Info("Tried to delete {0} but we're in read-only mode", path);
        }
    }

    public class ReadonlyBlobCache : PersistentBlobCache
    {
        public ReadonlyBlobCache(string cacheDirectory, IScheduler scheduler = null)
            : base(cacheDirectory, new ReadonlyFileSystemProvider(), scheduler)
        {
        }
    }

    public class ReadonlyEncryptedBlobCache : EncryptedBlobCache
    {
        public ReadonlyEncryptedBlobCache(string cacheDirectory, IScheduler scheduler = null)
            : base(cacheDirectory, new ReadonlyFileSystemProvider(), scheduler)
        {
        }
    }
}