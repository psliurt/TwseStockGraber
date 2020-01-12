using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TwStockGrabBLL.Logic;
using System.Threading;
using TwStockGrabBLL.Logic.DeskGraber;
using TwStockGrabBLL.Filter;
using TwStockGrabBLL.Filter.AfterMarket;
using TwStockGrabBLL.Filter.AfterMarket.ResultData;
using TwStockGrabBLL.Mode;

namespace TwStockGrab
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FilterParamOperation op = new FilterParamOperation();
            List<FilterParam> ds = op.GetParamList();
            if (ds != null)
            {
                FilterStrategyCmb.DataSource = ds;
                FilterStrategyCmb.DisplayMember = "StrategyName";
                FilterStrategyCmb.ValueMember = "StrategyCode";
                NoneRdo.Checked = true;
                FilterStrategyCmb.SelectedIndex = 0;
            }


            //Graber g = new StockItemGraber();
            //g.DoJob(DateTime.Today);


            //MiIndex01To20Graber g = new MiIndex01To20Graber();
            //DateTime d = new DateTime(2019, 4, 1);
            //do
            //{
            //    g.DoJob(d);
            //    d = d.AddDays(1);
            //    Sleep();
            //} while (d < DateTime.Today);



            //MiIndexEtfGraber g2 = new MiIndexEtfGraber();
            //DateTime d2 = new DateTime(2019, 4, 1);

            //do
            //{
            //    g2.DoJob(d2);
            //    d2 = d2.AddDays(1);
            //    Sleep();
            //} while (d2 < DateTime.Today);

            //DateTime d3 = new DateTime(2019, 1, 6);

            //FmtqikGraber g3 = new FmtqikGraber();
            //do
            //{
            //    g3.DoJob(d3);
            //    d3 = d3.AddDays(7);
            //    Sleep();
            //} while (d3 < DateTime.Today);

            //MiIndexTop20Graber g4 = new MiIndexTop20Graber();
            //DateTime d4 = new DateTime(2019, 1, 1);
            //do
            //{
            //    g4.DoJob(d4);
            //    d4 = d4.AddDays(1);
            //    Sleep();
            //} while (d4 < DateTime.Today);

            //Mi5MinsGraber g5 = new Mi5MinsGraber();
            //DateTime d5 = new DateTime(2019, 1, 1);
            //do
            //{
            //    g5.DoJob(d5);
            //    d5 = d5.AddDays(1);
            //    Sleep();
            //} while (d5 < DateTime.Today);


            //BfiamuGraber g6 = new BfiamuGraber();
            //DateTime d6 = new DateTime(2019, 6, 1);

            //do
            //{
            //    g6.DoJob(d6);
            //    d6 = d6.AddDays(1);
            //    Sleep();
            //} while (d6 < DateTime.Today);

            //TwtasuGraber g7 = new TwtasuGraber();
            //DateTime d7 = new DateTime(2019, 7, 1);

            //do
            //{
            //    g7.DoJob(d7);
            //    d7 = d7.AddDays(1);
            //    Sleep();
            //} while (d7 < DateTime.Today);

            //Twt44uGraber g8 = new Twt44uGraber();
            //DateTime d8 = new DateTime(2019, 5, 1);

            //do
            //{
            //    g8.DoJob(d8);
            //    d8 = d8.AddDays(1);
            //    Sleep();
            //} while (d8 < DateTime.Today);

            //Twt43uGraber g9 = new Twt43uGraber();
            //DateTime d9 = new DateTime(2019, 7, 1);

            //do
            //{
            //    g9.DoJob(d9);
            //    d9 = d9.AddDays(1);
            //    Sleep();
            //} while (d9 < DateTime.Today);

            //Twt38uGraber g10 = new Twt38uGraber();
            //DateTime d10 = new DateTime(2019, 6, 25);

            //do
            //{
            //    g10.DoJob(d10);
            //    d10 = d10.AddDays(1);
            //    Sleep();
            //} while (d10 <= DateTime.Today);

            //MiQfiisSort20Graber g11 = new MiQfiisSort20Graber();
            //DateTime d11 = new DateTime(2019, 6, 1);

            //do
            //{
            //    g11.DoJob(d11);
            //    d11 = d11.AddDays(1);
            //    Sleep();
            //} while (d11 <= DateTime.Today);

            //MiQfiisCatGraber g12 = new MiQfiisCatGraber();
            //DateTime d12 = new DateTime(2019, 6, 1);

            //do
            //{
            //    g12.DoJob(d12);
            //    d12 = d12.AddDays(1);
            //    Sleep();
            //} while (d12 <= DateTime.Today);


            //MiQfiisGraber g13 = new MiQfiisGraber();
            //DateTime d13 = new DateTime(2019, 7, 7);

            //do
            //{
            //    g13.DoJob(d13);
            //    d13 = d13.AddDays(1);
            //    Sleep();
            //} while (d13 <= DateTime.Today);

            //Bfi82uGraber g14 = new Bfi82uGraber();
            //DateTime d14 = new DateTime(2019, 1, 1);

            //do
            //{
            //    g14.DoJob(d14);
            //    d14 = d14.AddDays(1);
            //    Sleep();
            //} while (d14 <= DateTime.Today);

            //T86Graber g15 = new T86Graber();
            //DateTime d15 = new DateTime(2019, 7, 1);

            //do
            //{
            //    g15.DoJob(d15);
            //    d15 = d15.AddDays(1);
            //    Sleep();
            //} while (d15 <= DateTime.Today);

            //Twt54uGraber g16 = new Twt54uGraber();
            //DateTime d16 = new DateTime(2019, 7, 1);

            //do
            //{
            //    g16.DoJob(d16);
            //    d16 = d16.AddDays(1);
            //    Sleep();
            //} while (d16 <= DateTime.Today);

            //Twt47uGraber g17 = new Twt47uGraber();
            //DateTime d17 = new DateTime(2019, 7, 1);

            //do
            //{
            //    g17.DoJob(d17);
            //    d17 = d17.AddDays(1);
            //    Sleep();
            //} while (d17 <= DateTime.Today);

            //Bft41uGraber g18 = new Bft41uGraber();
            //DateTime d18 = new DateTime(2019, 7, 1);

            //do
            //{
            //    g18.DoJob(d18);
            //    d18 = d18.AddDays(1);
            //    Sleep();
            //} while (d18 <= DateTime.Today);

            //BfiauuStockDailyGraber g19 = new BfiauuStockDailyGraber();
            //DateTime d19 = new DateTime(2005, 1, 1);

            //do
            //{
            //    g19.DoJob(d19);
            //    d19 = d19.AddYears(1);
            //    Sleep();
            //} while (d19 <= DateTime.Today);

            //StockItemGraber g20 = new StockItemGraber();
            //DateTime d20 = new DateTime(2019, 10, 1);

            //do
            //{
            //    g20.DoJob(d20);
            //    d20 = d20.AddMonths(-1);
            //    Sleep();
            //} while (d20 <= DateTime.Today);

            //StockDayGraber g21 = new StockDayGraber();
            //DateTime d21 = new DateTime(2010, 1, 4);

            //do
            //{
            //    g21.DoJob(d21);
            //    d21 = d21.AddMonths(1);
            //    Sleep();
            //} while (d21 <= DateTime.Today);

            //StockDayAvgGraber g22 = new StockDayAvgGraber();
            //DateTime d22 = new DateTime(1999, 1, 5);

            //do
            //{
            //    g22.DoJob(d22);
            //    d22 = d22.AddMonths(1);
            //    Sleep();
            //} while (d22 <= DateTime.Today);

            //FmsrfkGraber g23 = new FmsrfkGraber();
            //DateTime d23 = new DateTime(1992, 1, 1);

            //do
            //{
            //    g23.DoJob(d23);
            //    d23 = d23.AddYears(1);
            //    Sleep();
            //} while (d23 <= DateTime.Today);

            //FmnptkGraber g24 = new FmnptkGraber();
            //DateTime d24 = new DateTime(1991, 1, 1);

            //do
            //{
            //    g24.DoJob(d24);
            //    d24 = d24.AddYears(1);
            //    Sleep();
            //} while (d24 <= DateTime.Today);

            //Twt84uGraber g25 = new Twt84uGraber();
            //DateTime d25 = new DateTime(2005, 12, 19);

            //do
            //{
            //    g25.DoJob(d25);
            //    d25 = d25.AddDays(1);
            //    Sleep();
            //} while (d25 <= DateTime.Today);

            //Twtb4uGraber g26 = new Twtb4uGraber();
            //DateTime d26 = new DateTime(2014, 1, 6);

            //do
            //{
            //    g26.DoJob(d26);
            //    d26 = d26.AddDays(1);
            //    Sleep();
            //} while (d26 <= DateTime.Today);

            //StockFirstGraber g27 = new StockFirstGraber();
            //DateTime d27 = new DateTime(2014, 2, 11);

            //do
            //{
            //    g27.DoJob(d27);
            //    d27 = d27.AddDays(1);
            //    Sleep();
            //} while (d27 <= DateTime.Today);

            //BwibbuDailyGraber g28 = new BwibbuDailyGraber();
            //DateTime d28 = new DateTime(2005, 9, 2);

            //do
            //{
            //    g28.DoJob(d28);
            //    d28 = d28.AddDays(1);
            //    Sleep();
            //} while (d28 <= DateTime.Today);

            //BfiauuDailyGraber g29 = new BfiauuDailyGraber();
            //DateTime d29 = new DateTime(2005, 4, 1);

            //do
            //{
            //    g29.DoJob(d29);
            //    d29 = d29.AddMonths(1);
            //    Sleep();
            //} while (d29 <= DateTime.Today);

            //BfiauuMonthlyGraber g30 = new BfiauuMonthlyGraber();
            //DateTime d30 = new DateTime(2005, 4, 1);

            //do
            //{
            //    g30.DoJob(d30);
            //    d30 = d30.AddYears(1);
            //    Sleep();
            //} while (d30 <= DateTime.Today);

            //BfiauuYearlyGraber g31 = new BfiauuYearlyGraber();
            //DateTime d31 = new DateTime(2019, 9, 16);

            //do
            //{
            //    g31.DoJob(d31);
            //    d31 = d31.AddDays(1);
            //    Sleep();
            //} while (d31 <= DateTime.Today);

            //BfiauuSingleGraber g32 = new BfiauuSingleGraber();
            ////DateTime d32 = new DateTime(2005, 4, 4);
            //DateTime d32 = new DateTime(2005, 8, 25);

            //do
            //{
            //    g32.DoJob(d32);
            //    d32 = d32.AddDays(1);
            //    Sleep();
            //} while (d32 <= DateTime.Today);

            //Twta1uGraber g33 = new Twta1uGraber();
            ////DateTime d33 = new DateTime(2006, 10, 2);
            //DateTime d33 = new DateTime(2007, 3, 17);

            //do
            //{
            //    g33.DoJob(d33);
            //    d33 = d33.AddDays(1);
            //    Sleep();
            //} while (d33 <= DateTime.Today);


            //Twt93uGraber g34 = new Twt93uGraber();            
            //DateTime d34 = new DateTime(2005, 7, 1);

            //do
            //{
            //    g34.DoJob(d34);
            //    d34 = d34.AddDays(1);
            //    Sleep();
            //} while (d34 <= DateTime.Today);

            //Twt92uGraber g35 = new Twt92uGraber();
            //DateTime d35 = new DateTime(2013, 9, 23);

            //do
            //{
            //    g35.DoJob(d35);
            //    d35 = d35.AddDays(1);
            //    Sleep();
            //} while (d35 <= DateTime.Today);

            //Twtbau2Graber g36 = new Twtbau2Graber();
            //DateTime d36 = new DateTime(2014, 6, 30);

            //do
            //{
            //    g36.DoJob(d36);
            //    d36 = d36.AddDays(1);
            //    Sleep();
            //} while (d36 <= DateTime.Today);

            //Bfi84u2Graber g37 = new Bfi84u2Graber();
            //DateTime d37 = new DateTime(2015, 3, 12);

            //do
            //{
            //    g37.DoJob(d37);
            //    d37 = d37.AddDays(1);
            //    Sleep();
            //} while (d37 <= DateTime.Today);

            //MiMarginGraber g38 = new MiMarginGraber();
            //DateTime d38 = new DateTime(2001, 1, 1);

            //do
            //{
            //    g38.DoJob(d38);
            //    d38 = d38.AddDays(1);
            //    Sleep();
            //} while (d38 <= DateTime.Today);

            //MiIndexTradeStatisticGraber g39 = new MiIndexTradeStatisticGraber();
            //DateTime d39 = new DateTime(2012, 4, 2);

            //do
            //{
            //    g39.DoJob(d39);
            //    d39 = d39.AddDays(1);
            //    Sleep();
            //} while (d39 <= DateTime.Today);

            //MiIndexGraber g40 = new MiIndexGraber();
            //DateTime d40 = new DateTime(2004, 2, 11);

            //do
            //{
            //    g40.DoJob(d40);
            //    d40 = d40.AddDays(1);
            //    Sleep();
            //} while (d40 <= DateTime.Today);

            //StockItemGraber g41 = new StockItemGraber();
            //DateTime d41 = new DateTime(2019, 9, 30);

            //do
            //{
            //    g41.DoJob(d41);
            //    d41 = d41.AddDays(1);
            //    Sleep();
            //} while (d41 <= DateTime.Today);




        }

        private void Sleep()
        {
            Random r = new Random();
            int rnd = 0;
            do
            {
                rnd = r.Next(5000);
            } while (rnd < 3000);
            Thread.Sleep(rnd);
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            //D3itrdsumDailyGraber g1 = new D3itrdsumDailyGraber();
            //DateTime d1 = new DateTime(2017, 1, 3);
            //do
            //{            
            //    g1.DoJob(d1);
            //    d1 = d1.AddDays(1);
            //    Sleep();
            //} while (d1 <= DateTime.Today);

            //MessageBox.Show("OK");

            //D3itradeHedgeDailyGraber g2 = new D3itradeHedgeDailyGraber();
            ////DateTime d2 = new DateTime(2014, 12, 1);
            //DateTime d2 = new DateTime(2014, 12, 24);
            //do
            //{
            //    g2.DoJob(d2);
            //    d2 = d2.AddDays(1);
            //    Sleep();
            //} while (d2 <= DateTime.Today);

            //DdealtrHedgeDailyGraber g3 = new DdealtrHedgeDailyGraber();            
            //DateTime d3 = new DateTime(2014, 12, 1);
            //do
            //{
            //    g3.DoJob(d3);
            //    d3 = d3.AddDays(1);
            //    Sleep();
            //} while (d3 <= DateTime.Today);

            //DMarginBalGraber g4 = new DMarginBalGraber();
            //DateTime d4 = new DateTime(2007, 1, 2);
            //do
            //{
            //    g4.DoJob(d4);
            //    d4 = d4.AddDays(1);
            //    Sleep();
            //} while (d4 <= DateTime.Today);

            //DStkQuoteGraber g5 = new DStkQuoteGraber();
            //DateTime d5 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g5.DoJob(d5);
            //    d5 = d5.AddDays(1);
            //    Sleep();
            //} while (d5 <= DateTime.Today);

            //DIndexSummaryGraber g6 = new DIndexSummaryGraber();
            //DateTime d6 = new DateTime(2016, 1, 4);
            //do
            //{
            //    g6.DoJob(d6);
            //    d6 = d6.AddDays(1);
            //    Sleep();
            //} while (d6 <= DateTime.Today);

            //DMarketHighlightGraber g7 = new DMarketHighlightGraber();
            //DateTime d7 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g7.DoJob(d7);
            //    d7 = d7.AddDays(1);
            //    Sleep();
            //} while (d7 <= DateTime.Today);

            //DMarketStatisticsDailyGraber g8 = new DMarketStatisticsDailyGraber();
            //DateTime d8 = new DateTime(2009, 1, 5);
            //do
            //{
            //    g8.DoJob(d8);
            //    d8 = d8.AddDays(1);
            //    Sleep();
            //} while (d8 <= DateTime.Today);

            //DStkWn1430Graber g9 = new DStkWn1430Graber();
            //DateTime d9 = new DateTime(2007, 7, 10);
            //do
            //{
            //    g9.DoJob(d9);
            //    d9 = d9.AddDays(1);
            //    Sleep();
            //} while (d9 <= DateTime.Today);

            //DStkWn1430SummaryGraber g10 = new DStkWn1430SummaryGraber();
            //DateTime d10 = new DateTime(2012, 5, 29);
            //do
            //{
            //    g10.DoJob(d10);
            //    d10 = d10.AddDays(1);
            //    Sleep();
            //} while (d10 <= DateTime.Today);

            //DSitctrDailyGraber g11 = new DSitctrDailyGraber();
            //DateTime d11 = new DateTime(2009, 2, 2);
            //do
            //{
            //    g11.DoJob(d11);
            //    d11 = d11.AddDays(1);
            //    Sleep();
            //} while (d11 <= DateTime.Today);

            //DForgtrDailyGraber g12 = new DForgtrDailyGraber();
            //DateTime d12 = new DateTime(2018, 1, 15);
            //do
            //{
            //    g12.DoJob(d12);
            //    d12 = d12.AddDays(1);
            //    Sleep();
            //} while (d12 <= DateTime.Today);

            //DQfiisectGraber g13 = new DQfiisectGraber();
            //DateTime d13 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g13.DoJob(d13);
            //    d13 = d13.AddDays(1);
            //    Sleep();
            //} while (d13 <= DateTime.Today);

            //DQfiiGraber g14 = new DQfiiGraber();
            //DateTime d14 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g14.DoJob(d14);
            //    d14 = d14.AddDays(1);
            //    Sleep();
            //} while (d14 <= DateTime.Today);

            //DBlockDayGraber g15 = new DBlockDayGraber();
            //DateTime d15 = new DateTime(2007, 1, 8);
            //do
            //{
            //    g15.DoJob(d15);
            //    d15 = d15.AddDays(1);
            //    Sleep();
            //} while (d15 <= DateTime.Today);

            //DMktGraber g16 = new DMktGraber();
            //DateTime d16 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g16.DoJob(d16);
            //    d16 = d16.AddDays(1);
            //    Sleep();
            //} while (d16 <= DateTime.Today);

            //DMktGraber g17 = new DMktGraber();
            //DateTime d17 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g17.DoJob(d17);
            //    d17 = d17.AddDays(1);
            //    Sleep();
            //} while (d17 <= DateTime.Today);

            //DRtBrkGraber g18 = new DRtBrkGraber();
            //DateTime d18 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g18.DoJob(d18);
            //    d18 = d18.AddDays(1);
            //    Sleep();
            //} while (d18 <= DateTime.Today);

            //DSectrGraber g19 = new DSectrGraber();
            //DateTime d19 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g19.DoJob(d19);
            //    d19 = d19.AddDays(1);
            //    Sleep();
            //} while (d19 <= DateTime.Today);

            //DPeraGraber g20 = new DPeraGraber();
            //DateTime d20 = new DateTime(2007, 1, 2);
            //do
            //{
            //    g20.DoJob(d20);
            //    d20 = d20.AddDays(1);
            //    Sleep();
            //} while (d20 <= DateTime.Today);

            //DShtsellGraber g21 = new DShtsellGraber();
            //DateTime d21 = new DateTime(2008, 10, 1);
            //do
            //{
            //    g21.DoJob(d21);
            //    d21 = d21.AddDays(1);
            //    Sleep();
            //} while (d21 <= DateTime.Today);

            //DPeratioPeraGraber g22 = new DPeratioPeraGraber();
            //DateTime d22 = new DateTime(2007, 1, 2);
            //do
            //{
            //    g22.DoJob(d22);
            //    d22 = d22.AddDays(1);
            //    Sleep();
            //} while (d22 <= DateTime.Today);

            //DTrnDailyGraber g23 = new DTrnDailyGraber();
            //DateTime d23 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g23.DoJob(d23);
            //    d23 = d23.AddDays(1);
            //    Sleep();
            //} while (d23 <= DateTime.Today);

            //DVolRankDailyGraber g24 = new DVolRankDailyGraber();
            //DateTime d24 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g24.DoJob(d24);
            //    d24 = d24.AddDays(1);
            //    Sleep();
            //} while (d24 <= DateTime.Today);

            //DAmtRankDailyGraber g25 = new DAmtRankDailyGraber();
            //DateTime d25 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g25.DoJob(d25);
            //    d25 = d25.AddDays(1);
            //    Sleep();
            //} while (d25 <= DateTime.Today);

            //DStkAvgDailyGraber g26 = new DStkAvgDailyGraber();
            //DateTime d26 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g26.DoJob(d26);
            //    d26 = d26.AddDays(1);
            //    Sleep();
            //} while (d26 <= DateTime.Today);

            //DAvgAmtDailyGraber g27 = new DAvgAmtDailyGraber();
            //DateTime d27 = new DateTime(2007, 1, 2);
            //do
            //{
            //    g27.DoJob(d27);
            //    d27 = d27.AddDays(1);
            //    Sleep();
            //} while (d27 <= DateTime.Today);

            //DRtRallyDailyGraber g28 = new DRtRallyDailyGraber();
            //DateTime d28 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g28.DoJob(d28);
            //    d28 = d28.AddDays(1);
            //    Sleep();
            //} while (d28 <= DateTime.Today);

            //DRtDeclinedDailyGraber g29 = new DRtDeclinedDailyGraber();
            //DateTime d29 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g29.DoJob(d29);
            //    d29 = d29.AddDays(1);
            //    Sleep();
            //} while (d29 <= DateTime.Today);

            //DMarginSblGraber g30 = new DMarginSblGraber();
            //DateTime d30 = new DateTime(2012, 10, 2);
            //do
            //{
            //    g30.DoJob(d30);
            //    d30 = d30.AddDays(1);
            //    Sleep();
            //} while (d30 <= DateTime.Today);

            //DMgUsedDailyGraber g31 = new DMgUsedDailyGraber();
            //DateTime d31 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g31.DoJob(d31);
            //    d31 = d31.AddDays(1);
            //    Sleep();
            //} while (d31 <= DateTime.Today);

            //DOddDailyGraber g32 = new DOddDailyGraber();
            //DateTime d32 = new DateTime(2007, 4, 1);
            //do
            //{
            //    g32.DoJob(d32);
            //    d32 = d32.AddMonths(1);
            //    Sleep();
            //} while (d32 <= DateTime.Today);

            //DBlockMthMonthlyGraber g33 = new DBlockMthMonthlyGraber();
            //DateTime d33 = new DateTime(2007, 1, 1);
            //do
            //{
            //    g33.DoJob(d33);
            //    d33 = d33.AddYears(1);
            //    Sleep();
            //} while (d33 <= DateTime.Today);

            //DBlockYrYearlyGraber g34 = new DBlockYrYearlyGraber();
            //DateTime d34 = new DateTime(2007, 1, 1);
            //do
            //{
            //    g34.DoJob(d34);
            //    d34 = d34.AddYears(1);
            //    Sleep();
            //} while (d34 <= DateTime.Today);

            //DLendGraber g35 = new DLendGraber();
            //DateTime d35 = new DateTime(2007, 1, 1);
            //do
            //{
            //    g35.DoJob(d35);
            //    d35 = d35.AddMonths(1);
            //    Sleep();
            //} while (d35 <= DateTime.Today);

            //DMgratioGraber g36 = new DMgratioGraber();
            //DateTime d36 = new DateTime(2007, 1, 1);
            //do
            //{
            //    g36.DoJob(d36);
            //    d36 = d36.AddMonths(1);
            //    Sleep();
            //} while (d36 <= DateTime.Today);

            //DMarginRankDailyGraber g37 = new DMarginRankDailyGraber();
            //DateTime d37 = new DateTime(2007, 4, 23);
            //do
            //{
            //    g37.DoJob(d37);
            //    d37 = d37.AddDays(1);
            //    Sleep();
            //} while (d37 <= DateTime.Today);

            //DMarginRankWeeklyGraber g38 = new DMarginRankWeeklyGraber();
            //DateTime d38 = new DateTime(2007, 4, 28);
            //DateTime thisWeekLast = new DateTime(2020, 1, 4);
            //do
            //{
            //    g38.DoJob(d38);
            //    d38 = d38.AddDays(7);
            //    Sleep();
            //} while (d38 <= thisWeekLast);

            //DMarginRankMonthlyGraber g39 = new DMarginRankMonthlyGraber();
            //DateTime d39 = new DateTime(2007, 4, 1);
            //DateTime thisMonthFirst = new DateTime(2020, 1, 1);
            //do
            //{
            //    g39.DoJob(d39);
            //    d39 = d39.AddMonths(1);
            //    Sleep();
            //} while (d39 < thisMonthFirst);

            //DMgUsedWeeklyGraber g40 = new DMgUsedWeeklyGraber();
            //DateTime d40 = new DateTime(2007, 4, 28);
            //DateTime thisWeekStart = new DateTime(2019, 12, 30);
            //do
            //{
            //    g40.DoJob(d40);
            //    d40 = d40.AddDays(7);
            //    Sleep();
            //} while (d40 < thisWeekStart);

            //DCeilOrdGraber g41 = new DCeilOrdGraber();
            //DateTime d41 = new DateTime(2007, 4, 23);

            //do
            //{
            //    g41.DoJob(d41);
            //    d41 = d41.AddDays(1);
            //    Sleep();
            //} while (d41 <= DateTime.Today);

            //DTrnMonthlyGraber g42 = new DTrnMonthlyGraber();
            //DateTime d42 = new DateTime(2007, 4, 1);

            //do
            //{
            //    g42.DoJob(d42);
            //    d42 = d42.AddMonths(1);
            //    Sleep();
            //} while (d42 <= DateTime.Today);

            //DTrnYearlyGraber g43 = new DTrnYearlyGraber();
            //DateTime d43 = new DateTime(2007, 1, 1);


            //do
            //{
            //    g43.DoJob(d43);
            //    d43 = d43.AddYears(1);
            //    Sleep();
            //} while (d43 <= DateTime.Today);

            //DVolRankWeeklyGraber g44 = new DVolRankWeeklyGraber();
            //DateTime d44 = new DateTime(2007, 4, 23);

            //do
            //{
            //    g44.DoJob(d44);
            //    d44 = d44.AddDays(7);
            //    Sleep();
            //} while (d44 <= DateTime.Today);

            //DVolRankMonthlyGraber g45 = new DVolRankMonthlyGraber();
            //DateTime d45 = new DateTime(2007, 4, 1);

            //do
            //{
            //    g45.DoJob(d45);
            //    d45 = d45.AddMonths(1);
            //    Sleep();
            //} while (d45 <= DateTime.Today);

            //DVolRankYearlyGraber g46 = new DVolRankYearlyGraber();
            //DateTime d46 = new DateTime(2007, 1, 1);

            //do
            //{
            //    g46.DoJob(d46);
            //    d46 = d46.AddYears(1);
            //    Sleep();
            //} while (d46 <= DateTime.Today);

            //DAmtRankMonthlyGraber g47 = new DAmtRankMonthlyGraber();
            //DateTime d47 = new DateTime(2007, 1, 1);

            //do
            //{
            //    g47.DoJob(d47);
            //    d47 = d47.AddMonths(1);
            //    Sleep();
            //} while (d47 <= DateTime.Today);

            //DAmtRankYearlyGraber g48 = new DAmtRankYearlyGraber();
            //DateTime d48 = new DateTime(2007, 1, 1);

            //do
            //{
            //    g48.DoJob(d48);
            //    d48 = d48.AddYears(1);
            //    Sleep();
            //} while (d48 <= DateTime.Today);

            //DStkAvgYearlyGraber g49 = new DStkAvgYearlyGraber();
            //DateTime d49 = new DateTime(2007, 1, 1);

            //do
            //{
            //    g49.DoJob(d49);
            //    d49 = d49.AddYears(1);
            //    Sleep();
            //} while (d49 <= DateTime.Today);

            //DStkAvgMonthlyGraber g50 = new DStkAvgMonthlyGraber();
            //DateTime d50 = new DateTime(2007, 1, 1);

            //do
            //{
            //    g50.DoJob(d50);
            //    d50 = d50.AddMonths(1);
            //    Sleep();
            //} while (d50 <= DateTime.Today);

            //DRtRallyMonthlyGraber g51 = new DRtRallyMonthlyGraber();
            //DateTime d51 = new DateTime(2007, 1, 1);

            //do
            //{
            //    g51.DoJob(d51);
            //    d51 = d51.AddMonths(1);
            //    Sleep();
            //} while (d51 <= DateTime.Today);

            //DRtDeclinedMonthlyGraber g52 = new DRtDeclinedMonthlyGraber();
            //DateTime d52 = new DateTime(2007, 1, 1);

            //do
            //{
            //    g52.DoJob(d52);
            //    d52 = d52.AddMonths(1);
            //    Sleep();
            //} while (d52 <= DateTime.Today);

            //DRtRallyWeeklyGraber g53 = new DRtRallyWeeklyGraber();
            //DateTime d53 = new DateTime(2007, 4, 23);

            //do
            //{
            //    g53.DoJob(d53);
            //    d53 = d53.AddDays(7);
            //    Sleep();
            //} while (d53 <= DateTime.Today);

            //DRtDeclinedWeeklyGraber g54 = new DRtDeclinedWeeklyGraber();
            //DateTime d54 = new DateTime(2007, 4, 23);

            //do
            //{
            //    g54.DoJob(d54);
            //    d54 = d54.AddDays(7);
            //    Sleep();
            //} while (d54 <= DateTime.Today);

            //DWkqGraber g55 = new DWkqGraber();
            //DateTime d55 = new DateTime(2007, 4, 23);

            //do
            //{
            //    g55.DoJob(d55);
            //    d55 = d55.AddDays(7);
            //    Sleep();
            //} while (d55 <= DateTime.Today);

            //D3itrdsumWeeklyGraber g56 = new D3itrdsumWeeklyGraber();
            //DateTime d56 = new DateTime(2017, 1, 2);

            //do
            //{
            //    g56.DoJob(d56);
            //    d56 = d56.AddDays(7);
            //    Sleep();
            //} while (d56 <= DateTime.Today);

            //D3itrdsumMonthlyGraber g57 = new D3itrdsumMonthlyGraber();
            //DateTime d57 = new DateTime(2017, 1, 1);

            //do
            //{
            //    g57.DoJob(d57);
            //    d57 = d57.AddMonths(1);
            //    Sleep();
            //} while (d57 <= DateTime.Today);

            //D3itrdsumYearlyGraber g58 = new D3itrdsumYearlyGraber();
            //DateTime d58 = new DateTime(2017, 1, 1);

            //do
            //{
            //    g58.DoJob(d58);
            //    d58 = d58.AddYears(1);
            //    Sleep();
            //} while (d58 <= DateTime.Today);


            //D3itradeHedgeWeeklyGraber g59 = new D3itradeHedgeWeeklyGraber();
            //DateTime d59 = new DateTime(2014, 12, 1);
            //do
            //{
            //    g59.DoJob(d59);
            //    d59 = d59.AddDays(7);
            //    Sleep();
            //} while (d59 <= DateTime.Today);


            //D3itradeHedgeMonthlyGraber g60 = new D3itradeHedgeMonthlyGraber();
            //DateTime d60 = new DateTime(2014, 12, 1);
            //do
            //{
            //    g60.DoJob(d60);
            //    d60 = d60.AddMonths(1);
            //    Sleep();
            //} while (d60 <= DateTime.Today);

            //D3itradeHedgeYearlyGraber g61 = new D3itradeHedgeYearlyGraber();
            //DateTime d61 = new DateTime(2015, 1, 1);

            //do
            //{
            //    g61.DoJob(d61);
            //    d61 = d61.AddYears(1);
            //    Sleep();
            //} while (d61 <= DateTime.Today);

            DForgtrWeeklyGraber g62 = new DForgtrWeeklyGraber();
            DateTime d62 = new DateTime(2014, 1, 6);
            do
            {
                g62.DoJob(d62);
                d62 = d62.AddDays(7);
                Sleep();
            } while (d62 <= DateTime.Today);


            DForgtrMonthlyGraber g63 = new DForgtrMonthlyGraber();
            DateTime d63 = new DateTime(2007, 4, 1);
            do
            {
                g63.DoJob(d63);
                d63 = d63.AddMonths(1);
                Sleep();
            } while (d63 <= DateTime.Today);


            DForgtrYearlyGraber g64 = new DForgtrYearlyGraber();
            DateTime d64 = new DateTime(2007, 1, 1);
            do
            {
                g64.DoJob(d64);
                d64 = d64.AddYears(1);
                Sleep();
            } while (d64 <= DateTime.Today);



            MessageBox.Show("OK");           

        }

        private void _miIndexBtn_Click(object sender, EventArgs e)
        {
            //if (SingleDateRdo.Checked)
            //{
            //    Graber g1 = new MiIndexGraber();
            //    g1.DoJob(DataDatePicker.Value.Date);

            //    Graber g2 = new MiIndexTradeStatisticGraber();
            //    g2.DoJob(DataDatePicker.Value.Date);
            //}
            //else if (PeriodRdo.Checked)
            //{
            //    DateTime from = FromDatePicker.Value.Date;
            //    DateTime to = ToDatePicker.Value.Date;

            //    Graber g1 = new MiIndexGraber();
            //    Graber g2 = new MiIndexTradeStatisticGraber();

            //    do
            //    {
            //        g1.DoJob(from);
            //        g2.DoJob(from);
            //        from = from.AddDays(1);
            //        Sleep();

            //    } while (from <= to);
            //}
            //else
            //{
            //    DateTime backFrom = FromBackDatePicker.Value.Date;
            //    DateTime stop = new DateTime(2000, 1, 1);

            //    Graber g1 = new MiIndexGraber();
            //    Graber g2 = new MiIndexTradeStatisticGraber();

            //    do
            //    {
            //        g1.DoJob(backFrom);
            //        g2.DoJob(backFrom);
            //        backFrom = backFrom.AddDays(-1);
            //        Sleep();

            //    } while (backFrom >= stop);

            //}   

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);
            List<Graber> gbrList = new List<Graber>();
            gbrList.Add(new MiIndexGraber());
            gbrList.Add(new MiIndexTradeStatisticGraber());
            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            gbrList);

            MessageBox.Show("MI_INDEX Complete!");
        }

        private void _fmtqikBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new FmtqikGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);
            
            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new FmtqikGraber());

            MessageBox.Show("FMTQIK Complete!");
        }

        private void _stockFirstBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new StockFirstGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new StockFirstGraber());

            MessageBox.Show("STOCK_FIRST Complete!");
        }

        private void _miIndex20Btn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new MiIndexTop20Graber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new MiIndexTop20Graber());

            MessageBox.Show("MI_INDEX20 Complete!");
        }

        private void _mi5MinsBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Mi5MinsGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Mi5MinsGraber());

            MessageBox.Show("MI_5MINS Complete!");
        }

        private void _bfiamuBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new BfiamuGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new BfiamuGraber());

            MessageBox.Show("BFIAMU Complete!");
        }

        private void _stockDayBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new StockDayGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new StockDayGraber());

            MessageBox.Show("BFIAMU Complete!");
        }

        private void _twtasuBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new TwtasuGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new TwtasuGraber());

            MessageBox.Show("TWTASU Complete!");
        }

        private void _stockDayAvgBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new StockDayAvgGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new StockDayAvgGraber());


            MessageBox.Show("STOCK_DAY_AVG Complete!");
        }

        private void _fmsrfkBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new FmsrfkGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new FmsrfkGraber());

            MessageBox.Show("FMSRFK Complete!");
        }

        private void _fmnptkBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new FmnptkGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new FmnptkGraber());

            MessageBox.Show("FMNPTK Complete!");
        }

        private void _bft41uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Bft41uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Bft41uGraber());

            MessageBox.Show("BFT41U Complete!");
        }

        private void _bwibbuDailyBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new BwibbuDailyGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new BwibbuDailyGraber());

            MessageBox.Show("BWIBBU_d Complete!");
        }

        private void _twt84uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt84uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt84uGraber());

            MessageBox.Show("TWT84U Complete!");
        }

        private void _twtb4uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twtb4uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twtb4uGraber());

            MessageBox.Show("TWTB4U Complete!");
        }

        private void _twtbau2Btn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twtbau2Graber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twtbau2Graber());

            MessageBox.Show("TWTBAU2 Complete!");
        }

        private void _miMargnBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new MiMarginGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new MiMarginGraber());

            MessageBox.Show("MI_MARGN Complete!");
        }

        private void _bfi84u2Btn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Bfi84u2Graber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Bfi84u2Graber());

            MessageBox.Show("BFI84U2 Complete!");
        }

        private void _twt92uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt92uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt92uGraber());

            MessageBox.Show("TWT92U Complete!");
        }

        private void _twt93uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt93uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt93uGraber());

            MessageBox.Show("TWT93U Complete!");
        }

        private void _twt96uBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TWT96U 尚無此功能!");
        }

        private void _twta1uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twta1uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twta1uGraber());

            MessageBox.Show("TWTA1U Complete!");
        }

        private void _bfi82uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Bfi82uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Bfi82uGraber());

            MessageBox.Show("BFI82U Complete!");
        }

        private void _t86Btn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new T86Graber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new T86Graber());

            MessageBox.Show("T86 Complete!");
        }

        private void _twt54uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt54uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt54uGraber());

            MessageBox.Show("TWT54U Complete!");
        }

        private void _twt47uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt47uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt47uGraber());

            MessageBox.Show("TWT47U Complete!");
        }

        private void _twt44uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt44uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt44uGraber());

            MessageBox.Show("TWT44U Complete!");
        }

        private void _twt43uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt43uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt43uGraber());

            MessageBox.Show("TWT43U Complete!");
        }

        private void _twt38uBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new Twt38uGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new Twt38uGraber());

            MessageBox.Show("TWT38U Complete!");
        }

        private void _miQfiisBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new MiQfiisGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new MiQfiisGraber());

            MessageBox.Show("MI_QFIIS Complete!");
        }

        private void _miQfiisSort20Btn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new MiQfiisSort20Graber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new MiQfiisSort20Graber());

            MessageBox.Show("MI_QFIIS_sort_20 Complete!");
        }

        private void _miQfiisCatBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new MiQfiisCatGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new MiQfiisCatGraber());

            MessageBox.Show("MI_QFIIS_cat Complete!");
        }

        private void _bfiauuSingleBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new BfiauuSingleGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new BfiauuSingleGraber());

            MessageBox.Show("BFIAUU Complete!");
        }

        private void _bfiauuSdBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new BfiauuStockDailyGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new BfiauuStockDailyGraber());

            MessageBox.Show("BFIAUU_sd Complete!");
        }

        private void _bfiauuDailyBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new BfiauuDailyGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new BfiauuDailyGraber());

            MessageBox.Show("BFIAUU_d Complete!");
        }

        private void _bfiauuMonthlyBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new BfiauuMonthlyGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new BfiauuMonthlyGraber());

            MessageBox.Show("BFIAUU_m Complete!");
        }

        private void _bfiauuYearlyBtn_Click(object sender, EventArgs e)
        {
            //Graber g1 = new BfiauuYearlyGraber();
            //g1.DoJob(DataDatePicker.Value.Date);

            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            new BfiauuYearlyGraber());

            MessageBox.Show("BFIAUU_y Complete!");
        }

        private void MarketSingleDataBtn_Click(object sender, EventArgs e)
        {
            ExecuterType et = GetExecuteType();
            IModeExecute executer = ModeFactory.GetExecuter(et);

            List<Graber> grbList = new List<Graber>();
            grbList.Add(new MiIndexGraber());
            grbList.Add(new MiMarginGraber());
            grbList.Add(new T86Graber());
            grbList.Add(new Twt93uGraber());
            grbList.Add(new Twt38uGraber());
            grbList.Add(new MiQfiisCatGraber());
            grbList.Add(new MiQfiisGraber());
            grbList.Add(new MiQfiisSort20Graber());
            grbList.Add(new MiIndexTradeStatisticGraber());
            grbList.Add(new MiIndexTop20Graber());
            grbList.Add(new TwtasuGraber());

            executer.Execute(new DateParams
            {
                SingleDate = DataDatePicker.Value.Date,
                FromDate = FromDatePicker.Value.Date,
                ToDate = ToDatePicker.Value.Date,
                BackFromDate = FromBackDatePicker.Value.Date
            },
            grbList);            

            MessageBox.Show("OK");
        }

        private void DeskSingleDataBtn_Click(object sender, EventArgs e)
        {
            DMarginBalGraber g1 = new DMarginBalGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            DStkQuoteGraber g2 = new DStkQuoteGraber();
            g2.DoJob(DataDatePicker.Value.Date);

            D3itradeHedgeDailyGraber g3 = new D3itradeHedgeDailyGraber();
            g3.DoJob(DataDatePicker.Value.Date);

            DMarginSblGraber g4 = new DMarginSblGraber();
            g4.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("OK");
        }

        private void AfterMarketFilterBtn_Click(object sender, EventArgs e)
        {
        //    FilterTest test = new FilterTest();
        //    //test.FilterStock();
        //    test.FilterTest2();
            string code = FilterStrategyCmb.SelectedValue.ToString();
            AfterMarketFilter filter = FilterFactory.Create(code);
            List<FilterResultData> result = filter.ExecFilter();            
            AfterMarketFilterDataGV.DataSource = result;            
        }

        private void SaveFilterParamBtn_Click(object sender, EventArgs e)
        {
            FilterParamOperation op = new FilterParamOperation();

            if (AddRdo.Checked)
            {
                op.SaveParams(new FilterParam
                {
                    Param10Value = FilterParam10Txt.Text.Trim(),
                    Param1Value = FilterParam1Txt.Text.Trim(),
                    Param2Value = FilterParam2Txt.Text.Trim(),
                    Param3Value = FilterParam3Txt.Text.Trim(),
                    Param4Value = FilterParam4Txt.Text.Trim(),
                    Param5Value = FilterParam5Txt.Text.Trim(),
                    Param6Value = FilterParam6Txt.Text.Trim(),
                    Param7Value = FilterParam7Txt.Text.Trim(),
                    Param8Value = FilterParam8Txt.Text.Trim(),
                    Param9Value = FilterParam9Txt.Text.Trim(),
                    StrategyName = FilterStrategyNameTxt.Text.Trim(),
                    ParamNote = FilterParamNoteTxt.Text.Trim(),
                    StrategyNote = FilterStrategyNoteTxt.Text.Trim(),
                    FilterClassName = AfterMarketFilterClassTxt.Text.Trim()
                });
            }

            if (UpdateRdo.Checked)
            {
                string code = FilterStrategyCmb.SelectedValue.ToString();

                op.UpdateParam(new FilterParam
                {
                    StrategyCode = code,
                    Param10Value = FilterParam10Txt.Text.Trim(),
                    Param1Value = FilterParam1Txt.Text.Trim(),
                    Param2Value = FilterParam2Txt.Text.Trim(),
                    Param3Value = FilterParam3Txt.Text.Trim(),
                    Param4Value = FilterParam4Txt.Text.Trim(),
                    Param5Value = FilterParam5Txt.Text.Trim(),
                    Param6Value = FilterParam6Txt.Text.Trim(),
                    Param7Value = FilterParam7Txt.Text.Trim(),
                    Param8Value = FilterParam8Txt.Text.Trim(),
                    Param9Value = FilterParam9Txt.Text.Trim(),
                    StrategyName = FilterStrategyNameTxt.Text.Trim(),
                    ParamNote = FilterParamNoteTxt.Text.Trim(),
                    StrategyNote = FilterStrategyNoteTxt.Text.Trim(),
                    FilterClassName = AfterMarketFilterClassTxt.Text.Trim()
                });
            }


            List<FilterParam> paramList = op.GetParamList();

            FilterStrategyCmb.DataSource = paramList;
            FilterStrategyCmb.DisplayMember = "StrategyName";
            FilterStrategyCmb.ValueMember = "StrategyCode";
            NoneRdo.Checked = true;
        }

        private void FilterStrategyCmb_SelectedValueChanged(object sender, EventArgs e)
        {
            if (FilterStrategyCmb.SelectedValue == null)
            { return; }

            if (FilterStrategyCmb.SelectedValue.GetType() != typeof(string))
            { return; }

            string code = FilterStrategyCmb.SelectedValue.ToString();
            if (string.IsNullOrEmpty(code))
            {
                return;
            }
            FilterParamOperation op = new FilterParamOperation();
            FilterParam paramSet = op.GetParam(code);
            FilterParam10Txt.Text = paramSet.Param10Value;
            FilterParam1Txt.Text = paramSet.Param1Value;
            FilterParam2Txt.Text = paramSet.Param2Value;
            FilterParam3Txt.Text = paramSet.Param3Value;
            FilterParam4Txt.Text = paramSet.Param4Value;
            FilterParam5Txt.Text = paramSet.Param5Value;
            FilterParam6Txt.Text = paramSet.Param6Value;
            FilterParam7Txt.Text = paramSet.Param7Value;
            FilterParam8Txt.Text = paramSet.Param8Value;
            FilterParam9Txt.Text = paramSet.Param9Value;
            FilterStrategyNameTxt.Text = paramSet.StrategyName;
            FilterParamNoteTxt.Text = paramSet.ParamNote;
            FilterStrategyNoteTxt.Text = paramSet.StrategyNote;
            AfterMarketFilterClassTxt.Text = paramSet.FilterClassName;
        }

        private void UpdateRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (UpdateRdo.Checked)
            {
                AfterMarketFilterClassTxt.ReadOnly = false;
            }            
        }

        private void AddRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (AddRdo.Checked)
            {
                AfterMarketFilterClassTxt.ReadOnly = false;
            }
        }

        private void NoneRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (NoneRdo.Checked)
            {
                AfterMarketFilterClassTxt.ReadOnly = true;
            }
        }

        private void _DMarginSblBtn_Click(object sender, EventArgs e)
        {
            DMarginSblGraber g1 = new DMarginSblGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_margin_sbl Complete!");
        }

        private ExecuterType GetExecuteType()
        {
            if (SingleDateRdo.Checked)
            {
                return ExecuterType.SingleDay;
            }
            else if (PeriodRdo.Checked)
            {
                return ExecuterType.Period;
            }
            else
            {
                return ExecuterType.ReverseBack;
            }
        }

        private void _DBlockDayBtn_Click(object sender, EventArgs e)
        {
            DBlockDayGraber g1 = new DBlockDayGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_block_day Complete!");
        }

        private void _DStkQuoteBtn_Click(object sender, EventArgs e)
        {
            DStkQuoteGraber g1 = new DStkQuoteGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_stk_quote Complete!");
        }

        private void _DIndexSummaryBtn_Click(object sender, EventArgs e)
        {
            DIndexSummaryGraber g1 = new DIndexSummaryGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_index_summary Complete!");
        }

        private void _DMarketHighlightBtn_Click(object sender, EventArgs e)
        {
            DMarketHighlightGraber g1 = new DMarketHighlightGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_market_highlight Complete!");
        }

        private void _DMarketStatisticsDailyBtn_Click(object sender, EventArgs e)
        {
            DMarketStatisticsDailyGraber g1 = new DMarketStatisticsDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_market_statistics_daily Complete!");
        }

        private void _DStkWin1430Btn_Click(object sender, EventArgs e)
        {
            DStkWn1430Graber g1 = new DStkWn1430Graber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_stk_wn1430 Complete!");
        }

        private void _DShtsellBtn_Click(object sender, EventArgs e)
        {
            DShtsellGraber g1 = new DShtsellGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_shtsell Complete!");
        }

        private void _DRtBrkBtn_Click(object sender, EventArgs e)
        {
            DRtBrkGraber g1 = new DRtBrkGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_rt_brk Complete!");
        }

        private void _DRtRallyDailyBtn_Click(object sender, EventArgs e)
        {
            DRtRallyDailyGraber g1 = new DRtRallyDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_rt_rally_daily Complete!");
        }

        private void _DMktBtn_Click(object sender, EventArgs e)
        {
            DMktGraber g1 = new DMktGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_mkt Complete!");
        }

        private void _DTrnDailyBtn_Click(object sender, EventArgs e)
        {
            DTrnDailyGraber g1 = new DTrnDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_trn_daily Complete!");
        }

        private void _DVolRankDailyBtn_Click(object sender, EventArgs e)
        {
            DVolRankDailyGraber g1 = new DVolRankDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_vol_rank_daily Complete!");
        }

        private void _DAmtRankDailyBtn_Click(object sender, EventArgs e)
        {
            DAmtRankDailyGraber g1 = new DAmtRankDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_amt_rank_daily Complete!");
        }

        private void _DStkAvgDailyBtn_Click(object sender, EventArgs e)
        {
            DStkAvgDailyGraber g1 = new DStkAvgDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_stk_avg_daily Complete!");
        }

        private void _DAvgAmtDailyBtn_Click(object sender, EventArgs e)
        {
            DAvgAmtDailyGraber g1 = new DAvgAmtDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_avg_amt_daily Complete!");
        }

        private void _DRtDeclinedDailyBtn_Click(object sender, EventArgs e)
        {
            DRtDeclinedDailyGraber g1 = new DRtDeclinedDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_rt_declined_daily Complete!");
        }

        private void _DSectrBtn_Click(object sender, EventArgs e)
        {
            DSectrGraber g1 = new DSectrGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_sectr Complete!");
        }

        private void _DPeraBtn_Click(object sender, EventArgs e)
        {
            DPeraGraber g1 = new DPeraGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_pera Complete!");
        }

        private void _D3itrdsumDailyBtn_Click(object sender, EventArgs e)
        {
            D3itrdsumDailyGraber g1 = new D3itrdsumDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_3itrdsum_daily Complete!");
        }

        private void _D3itradeHedgeDailyBtn_Click(object sender, EventArgs e)
        {
            D3itradeHedgeDailyGraber g1 = new D3itradeHedgeDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_3itrade_hedge_daily Complete!");
        }

        private void _DDealtrHedgeDailyBtn_Click(object sender, EventArgs e)
        {
            DDealtrHedgeDailyGraber g1 = new DDealtrHedgeDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_dealtr_hedge_daily Complete!");
        }

        private void _DSitctrDailyBtn_Click(object sender, EventArgs e)
        {
            DSitctrDailyGraber g1 = new DSitctrDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_sitctr_daily Complete!");
        }

        private void _DForgtrDailyBtn_Click(object sender, EventArgs e)
        {
            DForgtrDailyGraber g1 = new DForgtrDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_forgtr_daily Complete!");
        }

        private void _DQfiiBtn_Click(object sender, EventArgs e)
        {
            DQfiiGraber g1 = new DQfiiGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_qfii Complete!");
        }

        private void _DQfiisectBtn_Click(object sender, EventArgs e)
        {
            DQfiisectGraber g1 = new DQfiisectGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_qfiisect Complete!");
        }

        private void _DoddDailyBtn_Click(object sender, EventArgs e)
        {
            DOddDailyGraber g1 = new DOddDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_odd_daily Complete!");
        }

        private void _DBlockMthMonthlyBtn_Click(object sender, EventArgs e)
        {
            DBlockMthMonthlyGraber g1 = new DBlockMthMonthlyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_block_mth_monthly Complete!");
        }

        private void _DBlockYrYearlyBtn_Click(object sender, EventArgs e)
        {
            DBlockYrYearlyGraber g1 = new DBlockYrYearlyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_block_yr_yearly Complete!");
        }

        private void _DMarginRankDailyBtn_Click(object sender, EventArgs e)
        {
            DMarginRankDailyGraber g1 = new DMarginRankDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_margin_rank_daily Complete!");
        }

        private void _DMarginRankWeeklyBtn_Click(object sender, EventArgs e)
        {
            DMarginRankWeeklyGraber g1 = new DMarginRankWeeklyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_margin_rank_weekly Complete!");
        }

        private void _DMarginRankMonthlyBtn_Click(object sender, EventArgs e)
        {
            DMarginRankMonthlyGraber g1 = new DMarginRankMonthlyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("d_margin_rank_monthly Complete!");
        }

        private void HolidayDataBtn_Click(object sender, EventArgs e)
        {
            HolidayDataFetcher fetcher = new HolidayDataFetcher();
            fetcher.Fetch();
        }

        private void ExportFilterResultBtn_Click(object sender, EventArgs e)
        {
            List<FilterResultData> resultDataList = (List<FilterResultData>)AfterMarketFilterDataGV.DataSource;
            if (Directory.Exists("./FilterResult") == false)
            {
                Directory.CreateDirectory("./FilterResult");
            }

            if (resultDataList.Any() == false)
            {
                MessageBox.Show("沒有資料，不須匯出");
                return;
            }

            string fileNameAndPath = string.Format("./FilterResult/{0}.txt", DateTime.Today.ToString("yyyy_MM_dd"));
            using (StreamWriter sw = new StreamWriter(fileNameAndPath, true))
            {
                sw.WriteLine("--------------------------------------------");
                sw.WriteLine("* 選股策略名稱:{0}", FilterStrategyNameTxt.Text);                
                sw.WriteLine("    Note:{0}", FilterStrategyNoteTxt.Text);
                sw.WriteLine("--------------------------------------------");
                foreach (var item in resultDataList)
                {
                    sw.WriteLine("{0}  {1}", item.StockNo, item.StockName);
                }
                sw.WriteLine("                                            ");
            }
            MessageBox.Show("資料匯出完成!");
        }
    }
}
