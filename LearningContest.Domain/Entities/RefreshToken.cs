using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        /// <summary>
        /// رفرش توکن
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// زمان انقضای رفرش توکن
        /// </summary>
        public DateTime Expires { get; set; }
        ///// <summary>
        ///// نوع اپلیکیشن : وب، موبایل و ... 
        ///// </summary>
        public int? SoftwareTypeId { get; set; }        
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }
        //public virtual User User { get; set; }
        /// <summary>
        /// آی پی
        /// </summary>
        public string RemoteIpAddress { get; set; }
        /// <summary>
        /// زمان ایجاد
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// زمان ویرایش 
        /// </summary>
        public DateTime Modified { get; set; }

        
    }
}
