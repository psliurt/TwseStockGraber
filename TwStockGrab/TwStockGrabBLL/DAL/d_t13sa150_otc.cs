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
    
    public partial class d_t13sa150_otc
    {
        public decimal Uid { get; set; }
        public System.DateTime data_date { get; set; }
        public string stock_no { get; set; }
        public string stock_name { get; set; }
        public Nullable<long> issue_cnt { get; set; }
        public Nullable<long> remain_investable { get; set; }
        public Nullable<long> total_hold_cnt { get; set; }
        public Nullable<decimal> remain_invest_percent { get; set; }
        public Nullable<decimal> total_hold_percent { get; set; }
        public Nullable<decimal> invest_ceil_limit { get; set; }
        public Nullable<decimal> china_invest_ceil_limit { get; set; }
        public string change_reason { get; set; }
        public Nullable<System.DateTime> last_comp_change { get; set; }
        public string title { get; set; }
        public System.DateTime create_at { get; set; }
        public System.DateTime update_at { get; set; }
    }
}