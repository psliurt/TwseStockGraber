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
    
    public partial class d_3itrdsum_weekly
    {
        public decimal Uid { get; set; }
        public System.DateTime data_date { get; set; }
        public string item_name { get; set; }
        public Nullable<decimal> buy_in_money { get; set; }
        public Nullable<decimal> sell_out_money { get; set; }
        public Nullable<decimal> diff_money { get; set; }
        public string title { get; set; }
        public System.DateTime create_at { get; set; }
        public System.DateTime update_at { get; set; }
    }
}
