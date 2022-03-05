using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LearningContest.Domain.Enums
{
    public enum CollectionTypeEnum
    {
        //نامشخص
        none = 0,
        //نقد
        cash = 1,
        //اعتباری
        LongLasting = 2
    }
    public enum InvoiceWareHouseEnum
    {
        BeforeWareHouseCheck = 0,
        AfterWareHouseCheck = 1

    }

    public enum CustomerRequestAcceptanceStatus
    {
        [Description("تایید نشده")]
        Created = 1,
        [Description("تایید شده")]
        Accept = 2,
        [Description("رد شده")]
        NotAccept = 3,
        [Description("تحویل به کارپینو")]
        ToCarpino = 4

    }

    public enum CustomerRequestPaymentStatus
    {
        [Description("بدون پرداخت آنلاین")]
        NotOnlinePayment = 1,
        [Description("معلق")]
        Pending = 2,
        [Description("پرداخت شده")]
        Paid = 3,
        [Description("پرداخت ناموفق")]
        Failed = 4
    }
}
