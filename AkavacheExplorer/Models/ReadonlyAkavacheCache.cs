using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ReactiveUI;
using System.Reactive;

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

        public IObservable<Unit> CreateRecursive(string path)
        {
            return _inner.CreateRecursive(path);
        }

        public IObservable<Unit> Delete(string path)
        {
            this.Log().Info("Tried to delete {0} but we're in read-only mode", path);
            return Observable.Return(Unit.Default);
        }

        public string GetDefaultLocalMachineCacheDirectory()
        {
            return _inner.GetDefaultLocalMachineCacheDirectory();
        }

        public string GetDefaultRoamingCacheDirectory()
        {
            return _inner.GetDefaultRoamingCacheDirectory();
        }

        public string GetDefaultSecretCacheDirectory()
        {
            return _inner.GetDefaultSecretCacheDirectory();
        }
    }

    public class BeginningOfTimeScheduler : IScheduler
    {
        IScheduler _inner;

        public BeginningOfTimeScheduler(IScheduler inner)
        {
            _inner = inner;
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            return _inner.Schedule(state, action);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _inner.Schedule(state, dueTime, action);
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return _inner.Schedule(state, dueTime, action);
        }

        public DateTimeOffset Now { get { return DateTimeOffset.MinValue; } }
    }

    public class ReadonlyBlobCache : PersistentBlobCache
    {
        public ReadonlyBlobCache(string cacheDirectory, IScheduler scheduler = null)
            : base(cacheDirectory, new ReadonlyFileSystemProvider(), new BeginningOfTimeScheduler(scheduler ?? RxApp.TaskpoolScheduler))
        {
        }
    }

    public class ReadonlyEncryptedBlobCache : EncryptedBlobCache
    {
        public ReadonlyEncryptedBlobCache(string cacheDirectory, IScheduler scheduler = null)
            : base(cacheDirectory, new ReadonlyFileSystemProvider(), new BeginningOfTimeScheduler(scheduler ?? RxApp.TaskpoolScheduler))
        {
        }
    }
}