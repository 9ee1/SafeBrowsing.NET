using Gee.Common.Guards;
using Polly;
using System;
using System.Threading.Tasks;

namespace Gee.External.Browsing.Databases {
    /// <summary>
    ///     Base Resilient Database.
    /// </summary>
    public abstract class BaseResilientBrowsingDatabase {
        /// <summary>
        ///     Disposed Flag.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Resiliency Policy.
        /// </summary>
        private readonly IAsyncPolicy _resiliencyPolicy;

        /// <summary>
        ///     Retry Attempts.
        /// </summary>
        private readonly int _retryAttempts;

        /// <summary>
        ///     Create a Base Resilient Database.
        /// </summary>
        /// <param name="retryAttempts">
        ///     The number of attempts a failed operation should be retried.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="retryAttempts" /> is less than or equal to <c>0</c>.
        /// </exception>
        private protected BaseResilientBrowsingDatabase(int retryAttempts) {
            Guard.ThrowIf(nameof(retryAttempts), retryAttempts).LessThanOrEqualTo(0);

            this._disposed = false;
            this._retryAttempts = retryAttempts;
            // ...
            //
            // ...
            this._resiliencyPolicy = CreateResiliencyPolicy(this);

            // <summary>
            //      Create Resiliency Policy.
            // </summary>
            IAsyncPolicy CreateResiliencyPolicy(BaseResilientBrowsingDatabase @this) {
                var cPolicyBuilder = Policy.Handle<BrowsingDatabaseException>();
                var cPolicy = cPolicyBuilder.WaitAndRetryAsync(@this._retryAttempts, CreateRetryPolicyTimeout);
                return cPolicy;
            }

            // <summary>
            //      Create Retry Policy Timeout.
            // </summary>
            TimeSpan CreateRetryPolicyTimeout(int cRetryAttempt) {
                var cDelaySeconds = Math.Pow(2, cRetryAttempt);
                var cDelay = TimeSpan.FromSeconds(cDelaySeconds);
                return cDelay;
            }
        }

        /// <summary>
        ///     Destroy a Base Resilient Database.
        /// </summary>
        ~BaseResilientBrowsingDatabase() => this.Dispose(false);

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        public void Dispose() {
            if (!this._disposed) {
                this.Dispose(true);
                GC.SuppressFinalize(this);

                this._disposed = true;
            }
        }

        /// <summary>
        ///     Dispose Object.
        /// </summary>
        /// <param name="disposing">
        ///     A boolean true if the object is being disposed by a caller. A boolean false if the object is being
        ///     disposed by a finalizer.
        /// </param>
        private protected virtual void Dispose(bool disposing) {
            if (!this._disposed) {
                this._disposed = true;
            }
        }

        /// <summary>
        ///     Execute a Resiliency Policy Action.
        /// </summary>
        /// <param name="resiliencyPolicyAction">
        ///     An action for the resilience policy to execute.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        private protected async Task ExecuteResiliencyPolicyAsync(Func<Task> resiliencyPolicyAction) {
            try {
                var executeTask = this._resiliencyPolicy.ExecuteAsync(resiliencyPolicyAction);
                await executeTask.ConfigureAwait(false);
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Execute a Resiliency Policy Action.
        /// </summary>
        /// <typeparam name="T">
        ///     The return type of <paramref name="resiliencyPolicyAction" />.
        /// </typeparam>
        /// <param name="resiliencyPolicyAction">
        ///     An action for the resilience policy to execute.
        /// </param>
        /// <returns>
        ///     The return value of <paramref name="resiliencyPolicyAction" />.
        /// </returns>
        private protected async Task<T> ExecuteResiliencyPolicyAsync<T>(Func<Task<T>> resiliencyPolicyAction) {
            try {
                var executeTask = this._resiliencyPolicy.ExecuteAsync(resiliencyPolicyAction);
                var executeResult = await executeTask.ConfigureAwait(false);
                return executeResult;
            }
            catch (ObjectDisposedException) {
                this.Dispose();
                throw;
            }
        }
    }
}