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
    
    public partial class mi_margin
    {
        public decimal uid { get; set; }
        public System.DateTime data_date { get; set; }
        public string select_type { get; set; }
        public string stock_no { get; set; }
        public string stock_name { get; set; }
        public Nullable<decimal> finance_buy_in { get; set; }
        public Nullable<decimal> finance_sell_out { get; set; }
        public Nullable<decimal> finance_cash_back { get; set; }
        public Nullable<decimal> finance_yesterday_balance { get; set; }
        public Nullable<decimal> finance_today_balance { get; set; }
        public Nullable<decimal> finance_ceiling { get; set; }
        public Nullable<decimal> margin_buy_in { get; set; }
        public Nullable<decimal> margin_sell_out { get; set; }
        public Nullable<decimal> margin_cash_back { get; set; }
        public Nullable<decimal> margin_yesterday_balance { get; set; }
        public Nullable<decimal> margin_today_balance { get; set; }
        public Nullable<decimal> margin_ceiling { get; set; }
        public Nullable<long> offset { get; set; }
        public string note { get; set; }
        public string title { get; set; }
        public System.DateTime create_at { get; set; }
        public System.DateTime update_at { get; set; }
    }
}
