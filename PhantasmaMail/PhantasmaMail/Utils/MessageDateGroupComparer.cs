using System.Collections.Generic;
using Syncfusion.DataSource;
using Syncfusion.DataSource.Extensions;

namespace PhantasmaMail.Utils
{
    public class MessageDateGroupComparer : IComparer<GroupResult>, ISortDirection
    {
        public MessageDateGroupComparer()
        {
            SortDirection = ListSortDirection.Ascending;
        }

        public int Compare(GroupResult x, GroupResult y)
        {
            int groupX;
            int groupY;

            groupX = x.Count;
            groupY = y.Count;

            // Objects are compared and return the SortDirection
            if (groupX.CompareTo(groupY) > 0)
                return SortDirection == ListSortDirection.Ascending ? 1 : -1;
            if (groupX.CompareTo(groupY) == -1)
                return SortDirection == ListSortDirection.Ascending ? -1 : 1;
            return 0;
        }

        public ListSortDirection SortDirection { get; set; }
    }
}