using System;
using System.Collections;
using System.Collections.Generic;

namespace LearningContest.Domain.DapperPager
{
    public class PagerData<T> : IEnumerable<T>
    {
        private readonly IEnumerable dataSource;
        private readonly int totalRecords;
        private readonly int totalPages;
        private readonly int currentPageNo;
        private readonly int pageSize;
        private readonly bool isReport;
        public PagerData(IEnumerable dataSource, int totalRecords, int totalPages, int currentPageNo, int pageSize, bool isReport = false)
        {
            this.totalRecords = totalRecords;
            this.totalPages = totalPages;
            this.dataSource = dataSource;
            this.currentPageNo = currentPageNo;
            this.pageSize = pageSize;
            this.isReport = isReport;
        }

        public bool IsReport
        {
            get
            {
                return isReport;
            }
        }

        public int TotalRecordCount
        {
            get
            {
                return totalRecords;
            }
        }

        public int TotalPagesCount
        {
            get
            {
                return totalPages;
            }
        }

        public int CurrentPageNo
        {
            get
            {
                return currentPageNo > 0 ? currentPageNo : 1;
            }
        }


        public int PageSize
        {
            get
            {
                return pageSize > 0 && pageSize <= 50 ? pageSize : 10;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (dataSource == null)
                yield break;

            foreach (T row in dataSource)
            {
                yield return row;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
