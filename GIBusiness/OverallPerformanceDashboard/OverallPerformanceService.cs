using Business.Library;
using Comone.Utils;
using DataAccess;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GIBusiness.OverallPerformanceDashboard
{
    public class OverallPerformanceService
    {
        public OverallPerformanceViewModel OverallPerformanceSearch(OverallPerformanceSearchViewModel data)
        {
            try
            {
                using (var context = new GIDbContext())
                {
                    //string pstring = "";
                    //if (!string.IsNullOrEmpty(data.OverallDate))
                    //    pstring += " and cast(DocDate as date) ='" + data.OverallDate + "'";

                    //var strwhere = new SqlParameter("@strwhere", pstring);

                    var queryResult = context.__overall_status.Where(c => c.DocDate == data.OverallDate.toDate()).ToList();
                    var queryResult2 = context.__overall_status_Express.Where(c => c.DocDate == data.OverallDate.toDate()).ToList();


                    var zone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("getZoneMaster"), new { }.sJson());
                    var round = utils.SendDataApi<List<RoundViewModel>>(new AppSettingConfig().GetUrl("getRoundMaster"), new { }.sJson());
                    var route = utils.SendDataApi<List<RouteViewModel>>(new AppSettingConfig().GetUrl("getRouteMaster"), new { }.sJson());

                    var _MS_Zone = zone.Where(c => c.zone_Name == "Ambient" || c.zone_Name == "Bulky" || c.zone_Name == "Chill" || c.zone_Name == "Frozen").OrderBy(o => o.zone_Name).ToList();
                    var _MS_Route = route.OrderBy(o => o.create_By).ToList();
                    var _MS_Round = round.OrderBy(o => o.create_By).ToList();

                    var result = new OverallPerformanceViewModel();
                    result.OverallDate = data.OverallDate;

                    var _OverallStatusList = new List<OverallStatusViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new OverallStatusViewModel();
                        resultItem.StatusName = item.StatusName;
                        resultItem.Qty = item.Qty.sParse<int>();

                        _OverallStatusList.Add(resultItem);
                    }
                    foreach (var item in queryResult2)
                    {
                        var resultItem = new OverallStatusViewModel();
                        resultItem.StatusName = "Ex_"+item.StatusName;
                        resultItem.Qty = item.Qty.sParse<int>();

                        _OverallStatusList.Add(resultItem);
                    }
                    result.OverallStatusViewModel = _OverallStatusList;

                    var _OrderByRoundList = new List<OrderByRoundViewModel>();
                    var queryResult3 = context.__overall_round.Where(c => c.DocDate == data.OverallDate.toDate()).ToList();
                    foreach (var item in _MS_Round)
                    {
                        var _queryResult3 = queryResult3.FirstOrDefault(c => c.Round_Name == item.round_Name);

                        var resultItem = new OrderByRoundViewModel();
                        resultItem.Round = item.round_Name;
                        resultItem.Order = 0;
                        resultItem.Done = 0;
                        resultItem.Canceled = 0;
                        resultItem.Remain = 0;

                        if (_queryResult3 != null)
                        {
                            resultItem.Order = _queryResult3.orderQty.sParse<int>();
                            resultItem.Done = _queryResult3.doneQty.sParse<int>();
                            resultItem.Canceled = _queryResult3.canceledQty.sParse<int>();
                            resultItem.Remain = _queryResult3.remainQty.sParse<int>();
                        }
                        
                        _OrderByRoundList.Add(resultItem);
                    }
                    result.OrderByRoundViewModel = _OrderByRoundList;

                    var _PickingByRoundList = new List<PickingByRoundViewModel>();
                    var queryResult4 = context.__overall_round_pick.Where(c => c.DocDate == data.OverallDate.toDate()).ToList();
                    foreach (var item in _MS_Round)
                    {
                        var _queryResult4 = queryResult4.FirstOrDefault(c => c.Round_Name == item.round_Name);

                        var resultItem = new PickingByRoundViewModel();
                        resultItem.Round = item.round_Name;
                        resultItem.PickQty = "0";
                        resultItem.Fulfilled = "0";
                        resultItem.UnFulfilled = "0";
                        resultItem.Remain = "0";

                        if (_queryResult4 != null)
                        {
                            //resultItem.PickQty = Convert.ToDouble(_queryResult4.PickQty).ToString("n2");
                            //resultItem.Fulfilled = Convert.ToDouble(_queryResult4.Fulfilled).ToString("n2");
                            //resultItem.UnFulfilled = Convert.ToDouble(_queryResult4.UnFulfilled).ToString("n2");
                            //resultItem.Remain = Convert.ToDouble(_queryResult4.Remain).ToString("n2");
                            resultItem.PickQty = _queryResult4.pickQty;
                            resultItem.Fulfilled = _queryResult4.fulfilled;
                            resultItem.UnFulfilled = _queryResult4.unFulfilled;
                            resultItem.Remain = _queryResult4.Remain;
                        }

                        _PickingByRoundList.Add(resultItem);
                    }
                    result.PickingByRoundViewModel = _PickingByRoundList;

                    var _PickingByZoneList = new List<PickingByZoneViewModel>();
                    var queryResult5 = context.__overall_zone_pick.Where(c => c.DocDate == data.OverallDate.toDate()).ToList();
                    foreach (var item in _MS_Zone)
                    {
                        var _queryResult5 = queryResult5.FirstOrDefault(c => c.Zone_Name == item.zone_Name);

                        var resultItem = new PickingByZoneViewModel();
                        resultItem.Zone = item.zone_Name;
                        resultItem.PickQty = "0";
                        resultItem.Fulfilled = "0";
                        resultItem.UnFulfilled = "0";
                        resultItem.Remain = "0";

                        if (_queryResult5 != null)
                        {
                            //resultItem.PickQty = Convert.ToDecimal(_queryResult5.PickQty).ToString("n2");
                            //resultItem.Fulfilled = Convert.ToDecimal(_queryResult5.Fulfilled).ToString("n2");
                            //resultItem.UnFulfilled = Convert.ToDecimal(_queryResult5.UnFulfilled).ToString("n2");
                            //resultItem.Remain = Convert.ToDecimal(_queryResult5.Remain).ToString("n2");
                            resultItem.PickQty = _queryResult5.pickQty;
                            resultItem.Fulfilled = _queryResult5.fulfilled;
                            resultItem.UnFulfilled = _queryResult5.unFulfilled;
                            resultItem.Remain = _queryResult5.Remain;
                        }

                        _PickingByZoneList.Add(resultItem);
                    }
                    result.PickingByZoneViewModel = _PickingByZoneList;

                    var _OrderByRouteList = new List<OrderByRouteViewModel>();
                    var queryResult6 = context.__overall_route.Where(c => c.DocDate == data.OverallDate.toDate()).ToList();
                    foreach (var item in _MS_Route)
                    {
                        var _queryResult6 = queryResult6.FirstOrDefault(c => c.Route_Name == item.route_Name);

                        var resultItem = new OrderByRouteViewModel();
                        resultItem.Route = item.route_Name;
                        resultItem.Order = 0;

                        if (_queryResult6 != null)
                        {
                            resultItem.Order = _queryResult6.orderQty.sParse<int>();
                        }

                        _OrderByRouteList.Add(resultItem);
                    }
                    result.OrderByRouteViewModel = _OrderByRouteList;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Overall_Insert()
        {
            using (var context = new GIDbContext())
            {
                try
                {
                    {
                        var commandText = "EXEC sp_View_Overall_Status_Insert_";
                        var rowsAffected = context.Database.ExecuteSqlCommand(commandText);
                        if (rowsAffected != 0)
                        {
                        }
                    }
                    {
                        var commandText = "EXEC sp_View_Overall_Round_Insert_";
                        var rowsAffected = context.Database.ExecuteSqlCommand(commandText);
                        if (rowsAffected != 0)
                        {
                        }
                    }
                    {
                        var commandText = "EXEC sp_View_Overall_Route_Insert_";
                        var rowsAffected = context.Database.ExecuteSqlCommand(commandText);
                        if (rowsAffected != 0)
                        {
                        }
                    }
                    {
                        var commandText = "EXEC sp_View_Overall_Round_Pick_Insert_";
                        var rowsAffected = context.Database.ExecuteSqlCommand(commandText);
                        if (rowsAffected != 0)
                        {
                        }
                    }
                    {
                        var commandText = "EXEC sp_View_Overall_Zone_Pick_Insert_";
                        var rowsAffected = context.Database.ExecuteSqlCommand(commandText);
                        if (rowsAffected != 0)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
