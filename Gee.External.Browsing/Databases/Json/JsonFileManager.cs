using Gee.Common;
using Gee.Common.Guards;
using Gee.Common.Security.Cryptography;
using Gee.Common.Text;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Gee.External.Browsing.Databases.Json {
    /// <summary>
    ///     JSON File Manager.
    /// </summary>
    internal sealed class JsonFileManager : IDisposable {
        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     File Lock.
        /// </summary>
        private readonly Mutex _fileLock;

        /// <summary>
        ///     File Path.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        ///     Create a JSON File Manager.
        /// </summary>
        /// <param name="filePath">
        ///     An absolute file path to a file.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="filePath" /> is a null reference.
        /// </exception>
        public JsonFileManager(string filePath) {
            Guard.ThrowIf(nameof(filePath), filePath).Null();

            this._disposed = false;
            this._filePath = filePath;
            // ...
            //
            // ...
            this._fileLock = CreateFileLock(this);

            // <summary>
            //      Create File Lock.
            // </summary>
            Mutex CreateFileLock(JsonFileManager @this) {
                var cFilePathHash = @this._filePath.AsciiEncode().Md5Hash().HexadecimalEncode();
                var cFileLock = new Mutex(false, $@"Global\{cFilePathHash}");
                return cFileLock;
            }
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                this._fileLock.Dispose();
                this._disposed = true;
            }
        }

        /// <summary>
        ///     Lock File.
        /// </summary>
        private void LockFile() {
            try {
                this._fileLock.WaitOne();
            }
            catch (AbandonedMutexException) {
                // ...
                //
                // We are using a global named mutex. An abandoned global named mutex might indicate that a process
                // has been terminated abruptly. We're only using the mutex to synchronize reads and writes to the
                // file so if it was abandoned by another process, we should be fine.
            }
        }

        /// <summary>
        ///     Read File.
        /// </summary>
        /// <returns>
        ///     A <see cref="FileModel" />.
        /// </returns>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if the file could not be read.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        public FileModel Read() {
            this.ThrowIfDisposed();
            try {
                this.LockFile();

                var fileContents = File.ReadAllText(this._filePath, Encoding.ASCII);
                var fileModel = JsonConvert.DeserializeObject<FileModel>(fileContents);
                return fileModel;
            }
            catch (Exception ex) {
                var detailMessage = $"A file ({this._filePath}) could not be read.";
                throw new BrowsingDatabaseException(detailMessage, ex);
            }
            finally {
                this.UnlockFile();
            }
        }

        /// <summary>
        ///     Throw an Exception if Object is Disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        private void ThrowIfDisposed() {
            if (this._disposed) {
                var detailMessage = $"An object ({nameof(JsonFileManager)}) is disposed.";
                throw new ObjectDisposedException(nameof(JsonFileManager), detailMessage);
            }
        }

        /// <summary>
        ///     Unlock File.
        /// </summary>
        private void UnlockFile() {
            this._fileLock.ReleaseMutex();
        }

        /// <summary>
        ///     Write to File.
        /// </summary>
        /// <param name="fileModel">
        ///     A <see cref="FileModel" />.
        /// </param>
        /// <exception cref="Gee.External.Browsing.Databases.BrowsingDatabaseException">
        ///     Thrown if the file could not be written.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="fileModel" /> is a null reference.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     Thrown if the object is disposed.
        /// </exception>
        public void Write(FileModel fileModel) {
            this.ThrowIfDisposed();
            Guard.ThrowIf(nameof(fileModel), fileModel).Null();

            try {
                this.LockFile();

                var fileContents = JsonConvert.SerializeObject(fileModel);
                File.WriteAllText(this._filePath, fileContents, Encoding.ASCII);
            }
            catch (Exception ex) {
                var detailMessage = $"A file ({this._filePath}) could not be written.";
                throw new BrowsingDatabaseException(detailMessage, ex);
            }
            finally {
                this.UnlockFile();
            }
        }
    }
}