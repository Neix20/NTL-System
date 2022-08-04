using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NtlSystem.Models
{
    public class mCustomerMsg
    {
        public mCustomerMsg(int notification, TNtlCustomer customer)
        {
            this.notification = notification;
            this.customer = customer;
        }

        public int notification { get; set; }
        public TNtlCustomer customer { get; set; }
    }
}