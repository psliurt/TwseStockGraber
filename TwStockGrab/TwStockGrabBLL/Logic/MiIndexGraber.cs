using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
using TwStockGrabBLL.Logic.Rsp.Json;

namespace TwStockGrabBLL.Logic
{
    /// <summary>
    /// 交易資訊->盤後資訊->每日收盤行情->"全部"選項
    /// mi_index_price_idx_twse
    /// mi_index_price_idx_cross
    /// mi_index_price_idx_twcomp
    /// mi_index_return_idx_twse
    /// mi_index_return_idx_cross
    /// mi_index_return_idx_twcomp
    /// mi_index_market_stat
    /// mi_index_up_down_stat
    /// mi_index_all
    /// 本資訊自民國93年2月11日起提供。
    /// </summary>
    public class MiIndexGraber :Graber
    {
        public MiIndexGraber() : base()
        {
            this._graberClassName = typeof(MiIndexGraber).Name;
            this._graberFrequency = 1;
        }

        public override void DoJob(DateTime dataDate)
        {   
            string responseContent = GetWebContent(dataDate, "ALL");
            MI_INDEX_Rsp rsp = JsonConvert.DeserializeObject<MI_INDEX_Rsp>(responseContent);
            DateTime firstStyleDataStart = new DateTime(2004, 2, 11);
            DateTime secondStyleDataStart = new DateTime(2009, 1, 5);
            DateTime thirdStyleDataStart = new DateTime(2011, 8, 1);

            if (dataDate >= firstStyleDataStart && dataDate < secondStyleDataStart)
            {
                if (rsp.data8 == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate);
                    Sleep();
                }
            }
            else if (dataDate >= secondStyleDataStart && dataDate < thirdStyleDataStart)
            {
                if (rsp.data8 == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate);
                    Sleep();
                }
            }
            else if (dataDate >= thirdStyleDataStart)
            {
                if (rsp.data9 == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate);
                    Sleep();
                }
            }
            else
            {
                if (rsp.data9 == null)
                {
                    Sleep();
                }
                else
                {
                    SaveToDatabase(rsp, dataDate);
                    Sleep();
                }
            }            
        }

        

        private void SaveToDatabase(MI_INDEX_Rsp rsp, DateTime grabeDate)
        {
            DateTime dataDate = GetDateFromAdDateString(rsp.date);

            List<mi_index_price_idx_twse> tmpAddPriceIdxTwseList = new List<mi_index_price_idx_twse>();
            List<mi_index_price_idx_twse> tmpPriceIdxTwseDataList = null;

            List<mi_index_price_idx_cross> tmpAddPriceIdxCrossList = new List<mi_index_price_idx_cross>();
            List<mi_index_price_idx_cross> tmpPriceIdxCrossDataList = null;

            List<mi_index_price_idx_twcomp> tmpAddPriceIdxTwCompList = new List<mi_index_price_idx_twcomp>();
            List<mi_index_price_idx_twcomp> tmpPriceIdxTwCompDataList = null;

            List<mi_index_return_idx_twse> tmpAddReturnIdxTwseList = new List<mi_index_return_idx_twse>();
            List<mi_index_return_idx_twse> tmpReturnIdxTwseDataList = null;

            List<mi_index_return_idx_cross> tmpAddReturnIdxCrossList = new List<mi_index_return_idx_cross>();
            List<mi_index_return_idx_cross> tmpReturnIdxCrossDataList = null;

            List<mi_index_return_idx_twcomp> tmpAddReturnIdxTwCompList = new List<mi_index_return_idx_twcomp>();
            List<mi_index_return_idx_twcomp> tmpReturnIdxTwCompDataList = null;

            List<mi_index_market_stat> tmpAddMarketStatisticList = new List<mi_index_market_stat>();
            List<mi_index_market_stat> tmpMarketStatisticDataList = null;

            List<mi_index_up_down_stat> tmpAddUpDownStatisticList = new List<mi_index_up_down_stat>();
            List<mi_index_up_down_stat> tmpUpDownStatisticDataList = null;

            List<mi_index_all> tmpAddAllIndexList = new List<mi_index_all>();
            List<mi_index_all> tmpAllIndexDataList = null;

            using (TwStockDataContext context = new TwStockDataContext())
            {
                tmpPriceIdxTwseDataList = context.Set<mi_index_price_idx_twse>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                tmpPriceIdxCrossDataList = context.Set<mi_index_price_idx_cross>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                tmpPriceIdxTwCompDataList = context.Set<mi_index_price_idx_twcomp>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();

                tmpReturnIdxTwseDataList = context.Set<mi_index_return_idx_twse>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                tmpReturnIdxCrossDataList = context.Set<mi_index_return_idx_cross>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                tmpReturnIdxTwCompDataList = context.Set<mi_index_return_idx_twcomp>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();

                tmpMarketStatisticDataList = context.Set<mi_index_market_stat>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();
                tmpUpDownStatisticDataList = context.Set<mi_index_up_down_stat>().AsNoTracking().Where(x => x.data_date == dataDate).ToList();

                tmpAllIndexDataList = context.Set<mi_index_all>().AsNoTracking().Where(x => x.data_date == dataDate && x.select_type == "ALL").ToList();

            }

            DateTime firstStyleDataStart = new DateTime(2004, 2, 11);
            DateTime secondStyleDataStart = new DateTime(2009, 1, 5);
            DateTime thirdStyleDataStart = new DateTime(2011, 8, 1);

            if (dataDate >= firstStyleDataStart && dataDate < secondStyleDataStart)
            {
                foreach (var data in rsp.data7)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_market_stat obj = tmpMarketStatisticDataList.Where(x => x.data_date == dataDate && x.deal_stat_item == itemName).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddMarketStatisticList.Add(new mi_index_market_stat
                        {
                            data_date = dataDate,
                            deal_stat_item = itemName,
                            deal_money = ToDecimalQ(data.ElementAt(1)),
                            deal_stock_cnt = ToLongQ(data.ElementAt(2)),
                            deal_cnt = ToLongQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle7)
                        });
                    }
                }

                foreach (var data in rsp.data8)
                {
                    string stockNo = data.ElementAt(0).Trim();

                    mi_index_all obj = tmpAllIndexDataList.Where(x => x.data_date == dataDate && x.select_type == "ALL" && x.stock_no == stockNo).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddAllIndexList.Add(new mi_index_all
                        {
                            data_date = grabeDate,
                            select_type = "ALL",
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            deal_stock_num = ToIntQ(data.ElementAt(2)),
                            deal_trade_num = ToIntQ(data.ElementAt(3)),
                            deal_money = ToDecimalQ(data.ElementAt(4)),
                            open_price = ToDecimalQ(data.ElementAt(5)),
                            high_price = ToDecimalQ(data.ElementAt(6)),
                            low_price = ToDecimalQ(data.ElementAt(7)),
                            close_price = ToDecimalQ(data.ElementAt(8)),
                            up_down = SignToByteQ(data.ElementAt(9)),
                            up_down_price = ToDecimalQ(data.ElementAt(10)),
                            last_show_buy_price = ToDecimalQ(data.ElementAt(11)),
                            last_show_buy_qty = ToIntQ(data.ElementAt(12)),
                            last_show_sell_price = ToDecimalQ(data.ElementAt(13)),
                            last_show_sell_qty = ToIntQ(data.ElementAt(14)),
                            eps = ToDecimalQ(data.ElementAt(15)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle8)
                        });
                    }
                }
            }
            else if (dataDate >= secondStyleDataStart && dataDate < thirdStyleDataStart)
            {
                foreach (var data in rsp.data1)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_twse obj = tmpPriceIdxTwseDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxTwseList.Add(new mi_index_price_idx_twse
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(0)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(1), data.ElementAt(2)),
                            up_down_percent = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle1)
                        });
                    }
                }

                foreach (var data in rsp.data2)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_cross obj = tmpPriceIdxCrossDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxCrossList.Add(new mi_index_price_idx_cross
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(0)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(1), data.ElementAt(2)),
                            up_down_percent = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle2)
                        });
                    }
                }

                foreach (var data in rsp.data3)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_twcomp obj = tmpPriceIdxTwCompDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxTwCompList.Add(new mi_index_price_idx_twcomp
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(0)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(1), data.ElementAt(2)),
                            up_down_percent = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle3)
                        });
                    }
                }

                foreach (var data in rsp.data4)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_twse obj = tmpReturnIdxTwseDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxTwseList.Add(new mi_index_return_idx_twse
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(0)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(1), data.ElementAt(2)),
                            up_down_percent = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle4)
                        });
                    }
                }

                foreach (var data in rsp.data5)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_cross obj = tmpReturnIdxCrossDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxCrossList.Add(new mi_index_return_idx_cross
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(0)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(1), data.ElementAt(2)),
                            up_down_percent = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle5)
                        });
                    }
                }

                foreach (var data in rsp.data6)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_twcomp obj = tmpReturnIdxTwCompDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxTwCompList.Add(new mi_index_return_idx_twcomp
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(0)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(1), data.ElementAt(2)),
                            up_down_percent = ToDecimalQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle6)
                        });
                    }
                }

                foreach (var data in rsp.data7)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_market_stat obj = tmpMarketStatisticDataList.Where(x => x.data_date == dataDate && x.deal_stat_item == itemName).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddMarketStatisticList.Add(new mi_index_market_stat
                        {
                            data_date = dataDate,
                            deal_stat_item = itemName,
                            deal_money = ToDecimalQ(data.ElementAt(1)),
                            deal_stock_cnt = ToLongQ(data.ElementAt(2)),
                            deal_cnt = ToLongQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle7)
                        });
                    }
                }

                foreach (var data in rsp.data8)
                {
                    string stockNo = data.ElementAt(0).Trim();

                    mi_index_all obj = tmpAllIndexDataList.Where(x => x.data_date == dataDate && x.select_type == "ALL" && x.stock_no == stockNo).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddAllIndexList.Add(new mi_index_all
                        {
                            data_date = grabeDate,
                            select_type = "ALL",
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            deal_stock_num = ToIntQ(data.ElementAt(2)),
                            deal_trade_num = ToIntQ(data.ElementAt(3)),
                            deal_money = ToDecimalQ(data.ElementAt(4)),
                            open_price = ToDecimalQ(data.ElementAt(5)),
                            high_price = ToDecimalQ(data.ElementAt(6)),
                            low_price = ToDecimalQ(data.ElementAt(7)),
                            close_price = ToDecimalQ(data.ElementAt(8)),
                            up_down = SignToByteQ(data.ElementAt(9)),
                            up_down_price = ToDecimalQ(data.ElementAt(10)),
                            last_show_buy_price = ToDecimalQ(data.ElementAt(11)),
                            last_show_buy_qty = ToIntQ(data.ElementAt(12)),
                            last_show_sell_price = ToDecimalQ(data.ElementAt(13)),
                            last_show_sell_qty = ToIntQ(data.ElementAt(14)),
                            eps = ToDecimalQ(data.ElementAt(15)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle8)
                        });
                    }
                }
            }
            else if (dataDate >= thirdStyleDataStart)
            {
                foreach (var data in rsp.data1)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_twse obj = tmpPriceIdxTwseDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxTwseList.Add(new mi_index_price_idx_twse
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle1)
                        });
                    }
                }

                foreach (var data in rsp.data2)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_cross obj = tmpPriceIdxCrossDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxCrossList.Add(new mi_index_price_idx_cross
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle2)
                        });
                    }
                }

                foreach (var data in rsp.data3)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_twcomp obj = tmpPriceIdxTwCompDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxTwCompList.Add(new mi_index_price_idx_twcomp
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle3)
                        });
                    }
                }

                foreach (var data in rsp.data4)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_twse obj = tmpReturnIdxTwseDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxTwseList.Add(new mi_index_return_idx_twse
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle4)
                        });
                    }
                }

                foreach (var data in rsp.data5)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_cross obj = tmpReturnIdxCrossDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxCrossList.Add(new mi_index_return_idx_cross
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle5)
                        });
                    }
                }

                foreach (var data in rsp.data6)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_twcomp obj = tmpReturnIdxTwCompDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxTwCompList.Add(new mi_index_return_idx_twcomp
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle6)
                        });
                    }
                }

                foreach (var data in rsp.data7)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_market_stat obj = tmpMarketStatisticDataList.Where(x => x.data_date == dataDate && x.deal_stat_item == itemName).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddMarketStatisticList.Add(new mi_index_market_stat
                        {
                            data_date = dataDate,
                            deal_stat_item = itemName,
                            deal_money = ToDecimalQ(data.ElementAt(1)),
                            deal_stock_cnt = ToLongQ(data.ElementAt(2)),
                            deal_cnt = ToLongQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle7)
                        });
                    }
                }

                foreach (var data in rsp.data8)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_up_down_stat obj = tmpUpDownStatisticDataList.Where(x => x.data_date == dataDate && x.class_item == itemName).FirstOrDefault();
                    if (obj == null)
                    {
                        int allMarket = 0;
                        int MarketAtLimit = 0;
                        GetDataAndLimit(data.ElementAt(1), ref allMarket, ref MarketAtLimit);

                        int allStock = 0;
                        int StockAtLimit = 0;
                        GetDataAndLimit(data.ElementAt(2), ref allStock, ref StockAtLimit);

                        tmpAddUpDownStatisticList.Add(new mi_index_up_down_stat
                        {
                            data_date = dataDate,
                            class_item = itemName,
                            all_market = allMarket,
                            market_at_limit = MarketAtLimit,
                            all_stock = allStock,
                            stock_at_limit = StockAtLimit,
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle8)
                        });
                    }
                }


                foreach (var data in rsp.data9)
                {
                    string stockNo = data.ElementAt(0).Trim();

                    mi_index_all obj = tmpAllIndexDataList.Where(x => x.data_date == dataDate && x.select_type == "ALL" && x.stock_no == stockNo).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddAllIndexList.Add(new mi_index_all
                        {
                            data_date = grabeDate,
                            select_type = "ALL",
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            deal_stock_num = ToIntQ(data.ElementAt(2)),
                            deal_trade_num = ToIntQ(data.ElementAt(3)),
                            deal_money = ToDecimalQ(data.ElementAt(4)),
                            open_price = ToDecimalQ(data.ElementAt(5)),
                            high_price = ToDecimalQ(data.ElementAt(6)),
                            low_price = ToDecimalQ(data.ElementAt(7)),
                            close_price = ToDecimalQ(data.ElementAt(8)),
                            up_down = SignToByteQ(data.ElementAt(9)),
                            up_down_price = ToDecimalQ(data.ElementAt(10)),
                            last_show_buy_price = ToDecimalQ(data.ElementAt(11)),
                            last_show_buy_qty = ToIntQ(data.ElementAt(12)),
                            last_show_sell_price = ToDecimalQ(data.ElementAt(13)),
                            last_show_sell_qty = ToIntQ(data.ElementAt(14)),
                            eps = ToDecimalQ(data.ElementAt(15)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle9)
                        });
                    }
                }
            }
            else
            {
                foreach (var data in rsp.data1)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_twse obj = tmpPriceIdxTwseDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxTwseList.Add(new mi_index_price_idx_twse
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle1)
                        });
                    }
                }

                foreach (var data in rsp.data2)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_cross obj = tmpPriceIdxCrossDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxCrossList.Add(new mi_index_price_idx_cross
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle2)
                        });
                    }
                }

                foreach (var data in rsp.data3)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_price_idx_twcomp obj = tmpPriceIdxTwCompDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddPriceIdxTwCompList.Add(new mi_index_price_idx_twcomp
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle3)
                        });
                    }
                }

                foreach (var data in rsp.data4)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_twse obj = tmpReturnIdxTwseDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxTwseList.Add(new mi_index_return_idx_twse
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle4)
                        });
                    }
                }

                foreach (var data in rsp.data5)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_cross obj = tmpReturnIdxCrossDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxCrossList.Add(new mi_index_return_idx_cross
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle5)
                        });
                    }
                }

                foreach (var data in rsp.data6)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_return_idx_twcomp obj = tmpReturnIdxTwCompDataList.Where(x => x.index_name == itemName).FirstOrDefault();

                    if (obj == null)
                    {
                        tmpAddReturnIdxTwCompList.Add(new mi_index_return_idx_twcomp
                        {
                            data_date = dataDate,
                            index_name = itemName,
                            close_index_val = ToDecimalQ(data.ElementAt(1)),
                            up_down_point = ToUpDownDecimal(data.ElementAt(2), data.ElementAt(3)),
                            up_down_percent = ToDecimalQ(data.ElementAt(4)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle6)
                        });
                    }
                }

                foreach (var data in rsp.data7)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_market_stat obj = tmpMarketStatisticDataList.Where(x => x.data_date == dataDate && x.deal_stat_item == itemName).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddMarketStatisticList.Add(new mi_index_market_stat
                        {
                            data_date = dataDate,
                            deal_stat_item = itemName,
                            deal_money = ToDecimalQ(data.ElementAt(1)),
                            deal_stock_cnt = ToLongQ(data.ElementAt(2)),
                            deal_cnt = ToLongQ(data.ElementAt(3)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle7)
                        });
                    }
                }

                foreach (var data in rsp.data8)
                {
                    string itemName = data.ElementAt(0).Trim();
                    mi_index_up_down_stat obj = tmpUpDownStatisticDataList.Where(x => x.data_date == dataDate && x.class_item == itemName).FirstOrDefault();
                    if (obj == null)
                    {
                        int allMarket = 0;
                        int MarketAtLimit = 0;
                        GetDataAndLimit(data.ElementAt(1), ref allMarket, ref MarketAtLimit);

                        int allStock = 0;
                        int StockAtLimit = 0;
                        GetDataAndLimit(data.ElementAt(2), ref allStock, ref StockAtLimit);

                        tmpAddUpDownStatisticList.Add(new mi_index_up_down_stat
                        {
                            data_date = dataDate,
                            class_item = itemName,
                            all_market = allMarket,
                            market_at_limit = MarketAtLimit,
                            all_stock = allStock,
                            stock_at_limit = StockAtLimit,
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle8)
                        });
                    }
                }


                foreach (var data in rsp.data9)
                {
                    string stockNo = data.ElementAt(0).Trim();

                    mi_index_all obj = tmpAllIndexDataList.Where(x => x.data_date == dataDate && x.select_type == "ALL" && x.stock_no == stockNo).FirstOrDefault();
                    if (obj == null)
                    {
                        tmpAddAllIndexList.Add(new mi_index_all
                        {
                            data_date = grabeDate,
                            select_type = "ALL",
                            stock_no = stockNo,
                            stock_name = data.ElementAt(1).Trim(),
                            deal_stock_num = ToIntQ(data.ElementAt(2)),
                            deal_trade_num = ToIntQ(data.ElementAt(3)),
                            deal_money = ToDecimalQ(data.ElementAt(4)),
                            open_price = ToDecimalQ(data.ElementAt(5)),
                            high_price = ToDecimalQ(data.ElementAt(6)),
                            low_price = ToDecimalQ(data.ElementAt(7)),
                            close_price = ToDecimalQ(data.ElementAt(8)),
                            up_down = SignToByteQ(data.ElementAt(9)),
                            up_down_price = ToDecimalQ(data.ElementAt(10)),
                            last_show_buy_price = ToDecimalQ(data.ElementAt(11)),
                            last_show_buy_qty = ToIntQ(data.ElementAt(12)),
                            last_show_sell_price = ToDecimalQ(data.ElementAt(13)),
                            last_show_sell_qty = ToIntQ(data.ElementAt(14)),
                            eps = ToDecimalQ(data.ElementAt(15)),
                            create_at = DateTime.Now,
                            update_at = DateTime.Now,
                            title = string.Format("{0}", rsp.subtitle9)
                        });
                    }
                }
            }          

            using (TwStockDataContext context = new TwStockDataContext())
            {
                context.mi_index_price_idx_twse.AddRange(tmpAddPriceIdxTwseList);
                context.mi_index_price_idx_cross.AddRange(tmpAddPriceIdxCrossList);
                context.mi_index_price_idx_twcomp.AddRange(tmpAddPriceIdxTwCompList);

                context.mi_index_return_idx_twse.AddRange(tmpAddReturnIdxTwseList);
                context.mi_index_return_idx_cross.AddRange(tmpAddReturnIdxCrossList);
                context.mi_index_return_idx_twcomp.AddRange(tmpAddReturnIdxTwCompList);

                context.mi_index_market_stat.AddRange(tmpAddMarketStatisticList);
                context.mi_index_up_down_stat.AddRange(tmpAddUpDownStatisticList);

                context.mi_index_all.AddRange(tmpAddAllIndexList);

                context.SaveChanges();
            }
        }

        private string GetWebContent(DateTime date, string type)
        {
            string paramResponse = "json";
            string paramDate = GetyyyyMMddDateString(date);
            string paramType = type;
            string paramUnderLine = GetTimeStamp();
        
            string url = string.Format("https://www.twse.com.tw/exchangeReport/MI_INDEX?response={0}&date={1}&type={2}&_={3}",
                paramResponse, paramDate, paramType, paramUnderLine);

            return GetHttpResponse(url);
        }        

        private void GetDataAndLimit(string data, ref int allMarket, ref int limit)
        {
            if (string.IsNullOrEmpty(data))
            {
                allMarket = 0;
                limit = 0;
                return;
            }

            int leftIndex = data.IndexOf('(');
            int rightIndex = data.IndexOf(')');

            if (leftIndex == -1 || rightIndex == -1)
            {
                allMarket = Convert.ToInt32(data.Replace(",",""));
                limit = 0;
                return;
            }

            string intStringWithComma = data.Substring(0, leftIndex);            

            allMarket = Convert.ToInt32(intStringWithComma.Replace(",", ""));
            limit = Convert.ToInt32(data.Substring(leftIndex + 1, rightIndex - (leftIndex + 1)));
        }
    }
}
