namespace Uncas.Core.Data
{
    /// <summary>
    /// Represents paging of data.
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagingInfo"/> class.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        public PagingInfo(int pageSize)
        {
            PageSize = pageSize;
        }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; private set; }
    }
}