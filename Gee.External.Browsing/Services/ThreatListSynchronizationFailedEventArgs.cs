using Gee.Common.Guards;
using System;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Threat List Synchronization Failed Event Arguments.
    /// </summary>
    public sealed class ThreatListSynchronizationFailedEventArgs : EventArgs {
        /// <summary>
        ///     Get Skipped Threat List.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatList" /> that failed to synchronize and was skipped.
        /// </remarks>
        public ThreatList SkippedThreatList { get; }

        /// <summary>
        ///     Get Synchronization Exception.
        /// </summary>
        /// <remarks>
        ///     Represents the exception that was thrown when the synchronization operation for
        ///     <see cref="SkippedThreatList" /> failed.
        /// </remarks>
        public Exception SynchronizationException { get; }

        /// <summary>
        ///     Get Synchronization Failure Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the synchronization operation
        ///     <see cref="SkippedThreatList" /> failed.
        /// </remarks>
        public DateTime SynchronizationFailureDate { get; }

        /// <summary>
        ///     Get Synchronization Start Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the synchronization operation
        ///     <see cref="SkippedThreatList" /> started.
        /// </remarks>
        public DateTime SynchronizationStartDate { get; }

        /// <summary>
        ///     Create a Threat List Synchronization Failed Event Arguments.
        /// </summary>
        /// <param name="skippedThreatList">
        ///     The <see cref="ThreatList" /> that failed to synchronize and was skipped.
        /// </param>
        /// <param name="synchronizationStartDate">
        ///     The date, in Coordinated Universal Time (UTC), the synchronization operation for
        ///     <paramref name="skippedThreatList" /> started. If the date is not expressed in UTC, it is converted to
        ///     it.
        /// </param>
        /// <param name="synchronizationFailureDate">
        ///     The date, in Coordinated Universal Time (UTC), the synchronization operation for
        ///     <paramref name="skippedThreatList" /> failed. If the date is not expressed in UTC, it is converted to
        ///     it.
        /// </param>
        /// <param name="synchronizationException">
        ///     The exception that was thrown when the synchronization operation for
        ///     <paramref name="skippedThreatList"/> failed. 
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="skippedThreatList" /> is a null reference, or if
        ///     <paramref name="synchronizationException" /> is a null reference.
        /// </exception>
        internal ThreatListSynchronizationFailedEventArgs(ThreatList skippedThreatList, DateTime synchronizationStartDate, DateTime synchronizationFailureDate, Exception synchronizationException) {
            Guard.ThrowIf(nameof(skippedThreatList), skippedThreatList).Null();
            Guard.ThrowIf(nameof(synchronizationException), synchronizationException).Null();

            this.SkippedThreatList = skippedThreatList;
            this.SynchronizationException = synchronizationException;
            this.SynchronizationFailureDate = synchronizationFailureDate.ToUniversalTime();
            this.SynchronizationStartDate = synchronizationStartDate.ToUniversalTime();
        }
    }
}