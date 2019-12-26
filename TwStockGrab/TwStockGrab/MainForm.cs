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

            DMgUsedDailyGraber g31 = new DMgUsedDailyGraber();
            DateTime d31 = new DateTime(2007, 4, 23);
            do
            {
                g31.DoJob(d31);
                d31 = d31.AddDays(1);
                Sleep();
            } while (d31 <= DateTime.Today);


            MessageBox.Show("OK");
            

        }

        private void _miIndexBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new MiIndexGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            Graber g2 = new MiIndexTradeStatisticGraber();
            g2.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("MI_INDEX Complete!");
        }

        private void _fmtqikBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new FmtqikGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("FMTQIK Complete!");
        }

        private void _stockFirstBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new StockFirstGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("STOCK_FIRST Complete!");
        }

        private void _miIndex20Btn_Click(object sender, EventArgs e)
        {
            Graber g1 = new MiIndexTop20Graber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("MI_INDEX20 Complete!");
        }

        private void _mi5MinsBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Mi5MinsGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("MI_5MINS Complete!");
        }

        private void _bfiamuBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new BfiamuGraber();
            g1.DoJob(DataDatePicker.Value.Date);
            MessageBox.Show("BFIAMU Complete!");
        }

        private void _stockDayBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new StockDayGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFIAMU Complete!");
        }

        private void _twtasuBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new TwtasuGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWTASU Complete!");
        }

        private void _stockDayAvgBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new StockDayAvgGraber();
            g1.DoJob(DataDatePicker.Value.Date);
            MessageBox.Show("STOCK_DAY_AVG Complete!");
        }

        private void _fmsrfkBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new FmsrfkGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("FMSRFK Complete!");
        }

        private void _fmnptkBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new FmnptkGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("FMNPTK Complete!");
        }

        private void _bft41uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Bft41uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFT41U Complete!");
        }

        private void _bwibbuDailyBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new BwibbuDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BWIBBU_d Complete!");
        }

        private void _twt84uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt84uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT84U Complete!");
        }

        private void _twtb4uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twtb4uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWTB4U Complete!");
        }

        private void _twtbau2Btn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twtbau2Graber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWTBAU2 Complete!");
        }

        private void _miMargnBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new MiMarginGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("MI_MARGN Complete!");
        }

        private void _bfi84u2Btn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Bfi84u2Graber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFI84U2 Complete!");
        }

        private void _twt92uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt92uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT92U Complete!");
        }

        private void _twt93uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt93uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT93U Complete!");
        }

        private void _twt96uBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TWT96U 尚無此功能!");
        }

        private void _twta1uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twta1uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWTA1U Complete!");
        }

        private void _bfi82uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Bfi82uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFI82U Complete!");
        }

        private void _t86Btn_Click(object sender, EventArgs e)
        {
            Graber g1 = new T86Graber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("T86 Complete!");
        }

        private void _twt54uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt54uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT54U Complete!");
        }

        private void _twt47uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt47uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT47U Complete!");
        }

        private void _twt44uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt44uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT44U Complete!");
        }

        private void _twt43uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt43uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT43U Complete!");
        }

        private void _twt38uBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new Twt38uGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("TWT38U Complete!");
        }

        private void _miQfiisBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new MiQfiisGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("MI_QFIIS Complete!");
        }

        private void _miQfiisSort20Btn_Click(object sender, EventArgs e)
        {
            Graber g1 = new MiQfiisSort20Graber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("MI_QFIIS_sort_20 Complete!");
        }

        private void _miQfiisCatBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new MiQfiisCatGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("MI_QFIIS_cat Complete!");
        }

        private void _bfiauuSingleBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new BfiauuSingleGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFIAUU Complete!");
        }

        private void _bfiauuSdBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new BfiauuStockDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFIAUU_sd Complete!");
        }

        private void _bfiauuDailyBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new BfiauuDailyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFIAUU_d Complete!");
        }

        private void _bfiauuMonthlyBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new BfiauuMonthlyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFIAUU_m Complete!");
        }

        private void _bfiauuYearlyBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new BfiauuYearlyGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            MessageBox.Show("BFIAUU_y Complete!");
        }

        private void MarketSingleDataBtn_Click(object sender, EventArgs e)
        {
            Graber g1 = new MiIndexGraber();
            g1.DoJob(DataDatePicker.Value.Date);

            Graber g2 = new MiMarginGraber();
            g2.DoJob(DataDatePicker.Value.Date);

            Graber g3 = new T86Graber();
            g3.DoJob(DataDatePicker.Value.Date);

            Graber g4 = new Twt38uGraber();
            g4.DoJob(DataDatePicker.Value.Date);

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
    }
}
