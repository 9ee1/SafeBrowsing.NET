using Gee.Common.Guards;
using System;

namespace Gee.External.Browsing.Services {
    /// <summary>
    ///     Threat List Synchronization Completed Event Arguments.
    /// </summary>
    public sealed class ThreatListSynchronizationCompletedEventArgs : EventArgs {
        /// <summary>
        ///     Get Synchronization Completion Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the synchronization operation for the
        ///     <see cref="SynchronizedThreatList" /> completed.
        /// </remarks>
        public DateTime SynchronizationCompletionDate { get; }

        /// <summary>
        ///     Get Synchronization Start Date.
        /// </summary>
        /// <remarks>
        ///     Represents the date, in Coordinated Universal Time (UTC), the synchronization operation for the
        ///     <see cref="SynchronizedThreatList" /> started.
        /// </remarks>
        public DateTime SynchronizationStartDate { get; }

        /// <summary>
        ///     Get Synchronized Threat List.
        /// </summary>
        /// <remarks>
        ///     Represents the <see cref="ThreatList" /> that was synchronized successfully.
        /// </remarks>
        public ThreatList SynchronizedThreatList { get; }

        /// <summary>
        ///     Create a Threat List Synchronization Completed Event Arguments.
        /// </summary>
        /// <param name="synchronizedThreatList">
        ///     The <see cref="ThreatList" /> that was synchronized successfully.
        /// </param>
        /// <param name="synchronizationStartDate">
        ///     The date, in Coordinated Universal Time (UTC), the synchronization operation for
        ///     <paramref name="synchronizedThreatList" /> started. If the date is not expressed in UTC, it is
        ///     converted to it.
        /// </param>
        /// <param name="synchronizationCompletionDate">
        ///     The date, in Coordinated Universal Time (UTC), the synchronization operation for
        ///     <paramref name="synchronizedThreatList" /> completed. If the date is not expressed in UTC, it is
        ///     converted to it.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="synchronizedThreatList" /> is a null reference.
        /// </exception>
        internal ThreatListSynchronizationCompletedEventArgs(ThreatList synchronizedThreatList, DateTime synchronizationStartDate, DateTime synchronizationCompletionDate) {
            Guard.ThrowIf(nameof(synchronizedThreatList), synchronizedThreatList).Null();

            this.SynchronizationCompletionDate = synchronizationCompletionDate;
            this.SynchronizationStartDate = synchronizationStartDate;
            this.SynchronizedThreatList = synchronizedThreatList;
        }
    }
}