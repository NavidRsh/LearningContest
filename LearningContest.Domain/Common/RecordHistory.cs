using CommonInfrastructure.Attribute;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Domain.Common
{
    public abstract class RecordHistory
    {
        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        [EntityConfigAttribute(Dto = ConfigItemState.Exist)]
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// شناسه ایجاد کننده
        /// </summary>
        [EntityConfigAttribute(Dto = ConfigItemState.Exist)]
        public int CreatedById { get; set; }
        /// <summary>
        /// ایجاد کننده
        /// </summary>
        //public Entities.User CreatedBy { get; set; }
        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        [EntityConfigAttribute(Dto = ConfigItemState.Exist)]
        public DateTime? ModifyDateTime { get; set; }
        /// <summary>
        /// شناسه ویرایش کننده
        /// </summary>
        [EntityConfigAttribute(Dto = ConfigItemState.Exist)]
        public int? ModifiedById { get; set; }
        /// <summary>
        /// ویرایش کننده
        /// </summary>
        //public Entities.User ModifiedBy { get; set; }

        
    }

   
}
