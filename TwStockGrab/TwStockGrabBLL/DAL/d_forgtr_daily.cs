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
    
    public partial class d_forgtr_daily
    {
        public decimal Uid { get; set; }
        public System.DateTime data_date { get; set; }
        public short buy_sell_type { get; set; }
        public int rank { get; set; }
        public string stock_no { get; set; }
        public string stock_name { get; set; }
        public Nullable<int> buy_in { get; set; }
        public Nullable<int> sell_out { get; set; }
        public Nullable<int> diff { get; set; }
        public Nullable<int> self_buy_in { get; set; }
        public Nullable<int> self_sell_out { get; set; }
        public Nullable<int> self_diff { get; set; }
        public Nullable<int> total_buy_in { get; set; }
        public Nullable<int> total_sell_out { get; set; }
        public Nullable<decimal> total_diff { get; set; }
        public string title { get; set; }
        public System.DateTime create_at { get; set; }
        public System.DateTime update_at { get; set; }
    }
}
