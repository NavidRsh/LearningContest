using System;
using System.Collections.Generic;

namespace LearningContest.Domain.DapperPager
{
    public class PagerModel
    {
        private int totalRecordCount = 0;
        private int pageSize = 0;
        private int currentPageNo = 1;
        private string sortBy = "";
        private bool isReport = false;
        private string filters = "";
        public PagerModel() { }

        public PagerModel(int pageNo, int pageSize)
        {
            Page = pageNo;
            PageSize = pageSize;
        }

        public int PageSize
        {
            get
            {
                return pageSize > 0 ? pageSize : 10;
            }
            set
            {
                pageSize = value <= 100 ? value : 100;
            }
        }

        public bool IsReport
        {
            get
            {
                return isReport || PageSize < 1;
            }
            //set
            //{
            //    isReport = value;
            //}
        }

        public string Filters
        {
            get { return ""; }
            set { filters = value; }
        }

        public string Sorts
        {
            get { return sortBy.ToLower(); }
            set { sortBy = value; }
        }

        public int Page
        {
            get
            {
                return currentPageNo;
            }
            set
            {
                currentPageNo = value > 0 ? value : 1;
            }
        }

        public int TotalRecordCount
        {
            get
            {
                return totalRecordCount;
            }
        }

        public int TotalPageCount
        {
            get
            {
                int count = totalRecordCount / PageSize;
                if (TotalRecordCount > 0 && TotalRecordCount % PageSize > 0)
                {
                    count++;
                }
                return count;
            }
        }


    }
}
