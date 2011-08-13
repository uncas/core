namespace Uncas.Core
{
    using System;

    /// <summary>
    /// Handles time in the system.
    /// </summary>
    /// <remarks>
    /// See http://ayende.com/blog/3408/dealing-with-time-in-tests.
    /// </remarks>
    public static class SystemTime
    {
        private static Func<DateTime> _now = () => DateTime.Now;

        /// <summary>
        /// Gets or sets the 'now' of the system.
        /// </summary>
        /// <value>
        /// The now of the system.
        /// </value>
        public static Func<DateTime> Now
        {
            get { return _now; }

            set { _now = value; }
        }
    }
}