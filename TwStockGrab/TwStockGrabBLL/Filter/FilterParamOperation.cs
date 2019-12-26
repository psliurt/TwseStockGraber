using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwStockGrabBLL.DAL;
namespace TwStockGrabBLL.Filter
{
    public class FilterParamOperation
    {
        public void SaveParams(FilterParam paramObj)
        {
            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                string code = Kit.GetSN();
                ctx.Set<p_filter_stg>().Add(new p_filter_stg
                {
                    create_at = DateTime.Now,
                    update_at = DateTime.Now,
                    stg_code = code,
                    stg_class_name = paramObj.FilterClassName,
                    stg_name = paramObj.StrategyName,
                    stg_p1 = paramObj.Param1Value,
                    stg_p10 = paramObj.Param10Value,
                    stg_p2 = paramObj.Param2Value,
                    stg_p3 = paramObj.Param3Value,
                    stg_p4 = paramObj.Param4Value,
                    stg_p5 = paramObj.Param5Value,
                    stg_p6 = paramObj.Param6Value,
                    stg_p7 = paramObj.Param7Value,
                    stg_p8 = paramObj.Param8Value,
                    stg_p9 = paramObj.Param9Value
                });

                ctx.Set<p_filter_stg_describe>().Add(new p_filter_stg_describe
                {
                    create_at = DateTime.Now,
                    stg_code = code,
                    stg_note = paramObj.StrategyNote,
                    stg_param_note = paramObj.ParamNote,
                    update_at = DateTime.Now
                });

                ctx.SaveChanges();
            }
        }

        public FilterParam GetParam(string code)
        {
            FilterParam fParam = null;
            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                p_filter_stg stg = ctx.Set<p_filter_stg>().AsNoTracking().Where(x => x.stg_code == code).FirstOrDefault();

                fParam = new FilterParam
                {
                    Param10Value = stg.stg_p10,
                    Param1Value = stg.stg_p1,
                    Param2Value = stg.stg_p2,
                    Param3Value = stg.stg_p3,
                    Param4Value = stg.stg_p4,
                    Param5Value = stg.stg_p5,
                    Param6Value = stg.stg_p6,
                    Param7Value = stg.stg_p7,
                    Param8Value = stg.stg_p8,
                    Param9Value = stg.stg_p9,
                    StrategyCode = stg.stg_code,
                    StrategyName = stg.stg_name,
                    FilterClassName = stg.stg_class_name
                };

                p_filter_stg_describe stgNote = ctx.Set<p_filter_stg_describe>().AsNoTracking().Where(x => x.stg_code == code).FirstOrDefault();

                fParam.ParamNote = stgNote.stg_param_note;
                fParam.StrategyNote = stgNote.stg_note;                
            }
            return fParam;
        }

        public List<FilterParam> GetParamList()
        {
            List<FilterParam> paramList = new List<FilterParam>();
            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                var stgList = from s in ctx.Set<p_filter_stg>().AsNoTracking()
                              join d in ctx.Set<p_filter_stg_describe>().AsNoTracking()
                              on s.stg_code equals d.stg_code
                              select new
                              {
                                  s.stg_code,
                                  s.stg_name,
                                  s.stg_p1,
                                  s.stg_p2,
                                  s.stg_p3,
                                  s.stg_p4,
                                  s.stg_p5,
                                  s.stg_p6,
                                  s.stg_p7,
                                  s.stg_p8,
                                  s.stg_p9,
                                  s.stg_p10,
                                  d.stg_note,
                                  d.stg_param_note
                              };

                foreach (var stg in stgList)
                {
                    //p_filter_stg_describe stgNote = ctx.Set<p_filter_stg_describe>().AsNoTracking().Where(x => x.stg_code == stg.stg_code).FirstOrDefault();

                    paramList.Add(new FilterParam
                    {
                        Param10Value = stg.stg_p10,
                        Param1Value = stg.stg_p1,
                        Param2Value = stg.stg_p2,
                        Param3Value = stg.stg_p3,
                        Param4Value = stg.stg_p4,
                        Param5Value = stg.stg_p5,
                        Param6Value = stg.stg_p6,
                        Param7Value = stg.stg_p7,
                        Param8Value = stg.stg_p8,
                        Param9Value = stg.stg_p9,
                        StrategyCode = stg.stg_code,
                        StrategyName = stg.stg_name,
                        ParamNote = stg.stg_param_note,
                        StrategyNote = stg.stg_note
                    });
                }                
            }
            return paramList;
        }

        public void UpdateParam(FilterParam paramObj)
        {
            using (TwStockDataContext ctx = new TwStockDataContext())
            {
                string code = paramObj.StrategyCode;
                p_filter_stg stg = ctx.Set<p_filter_stg>().Where(x => x.stg_code == code).FirstOrDefault();
                p_filter_stg_describe stgNote = ctx.Set<p_filter_stg_describe>().Where(x => x.stg_code == code).FirstOrDefault();

                stg.update_at = DateTime.Now;                
                stg.stg_name = paramObj.StrategyName;
                stg.stg_p1 = paramObj.Param1Value;
                stg.stg_p10 = paramObj.Param10Value;
                stg.stg_p2 = paramObj.Param2Value;
                stg.stg_p3 = paramObj.Param3Value;
                stg.stg_p4 = paramObj.Param4Value;
                stg.stg_p5 = paramObj.Param5Value;
                stg.stg_p6 = paramObj.Param6Value;
                stg.stg_p7 = paramObj.Param7Value;
                stg.stg_p8 = paramObj.Param8Value;
                stg.stg_p9 = paramObj.Param9Value;
                stg.stg_class_name = paramObj.FilterClassName;

                stgNote.stg_note = paramObj.StrategyNote;
                stgNote.stg_param_note = paramObj.ParamNote;
                stgNote.update_at = DateTime.Now;

                ctx.Entry<p_filter_stg>(stg).State = System.Data.Entity.EntityState.Modified;
                ctx.Entry<p_filter_stg_describe>(stgNote).State = System.Data.Entity.EntityState.Modified;

                ctx.SaveChanges();
            }
        }
    }
}
