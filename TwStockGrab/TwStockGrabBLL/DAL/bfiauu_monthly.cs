//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwStockGrabBLL.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class bfiauu_monthly
    {
        public decimal uid { get; set; }
        public int deal_year { get; set; }
        public int deal_month { get; set; }
        public string trade_type { get; set; }
        public Nullable<long> deal_cnt { get; set; }
        public Nullable<long> deal_stock_cnt { get; set; }
        public Nullable<decimal> deal_stock_rate { get; set; }
        public Nullable<decimal> deal_money { get; set; }
        public Nullable<decimal> deal_money_rate { get; set; }
        public string title { get; set; }
        public System.DateTime create_at { get; set; }
        public System.DateTime update_at { get; set; }
    }
}
