
namespace VirtualRoulette.Models.Enum
{/// <summary>
/// Enum for spin statuses
/// </summary>
    public enum SpinStatus : int
    {
        Default = InProgress,
        /// <summary>
        /// When spin is in progress
        /// </summary>
        InProgress = 1,
        /// <summary>
        /// When spin was in progress and already completed
        /// </summary>
        Completed = 2,
        /// <summary>
        /// When spim was canceled
        /// </summary>
        Canceled = -1
    }
}
