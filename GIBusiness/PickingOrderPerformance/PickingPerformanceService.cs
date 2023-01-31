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

namespace GIBusiness.PickingOrderPerformance
{
    public class PickingPerformanceService
    {
        public PickingViewModel PickingPerformanceSearchOld(PickingSearchViewModel data)
        {
            try
            {
                using (var context = new GIDbContext())
                {
                    string pstring = "";
                    if (!string.IsNullOrEmpty(data.PickDate))
                        pstring += " and DocDate = '" + data.PickDate + "'";
                    if (!string.IsNullOrEmpty(data.PickTime))
                        pstring += " and Round_Index = (select Round_Index from ms_Round where Round_Name = N'" + data.PickTime + "')";

                    string pstring2 = "";
                    if (!string.IsNullOrEmpty(data.PickDate))
                        pstring2 += " and DocDate = '" + data.PickDate + "'";
                    if (!string.IsNullOrEmpty(data.PickTime))
                        pstring2 += " and Round_Index = (select Round_Index from ms_Round where Round_Name = N'" + data.PickTime + "')";

                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var strwhere1 = new SqlParameter("@strwhere1", pstring2 + " and ISNULL(UDF_3, 'N') = 'N' and DocumentType_Id = 'Normal'");

                    var zone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("getZoneMaster"), new { }.sJson());
                    var round = utils.SendDataApi<List<RoundViewModel>>(new AppSettingConfig().GetUrl("getRoundMaster"), new { }.sJson());
                    var route = utils.SendDataApi<List<RouteViewModel>>(new AppSettingConfig().GetUrl("getRouteMaster"), new { }.sJson());

                    var __dashboard = context.__dashboard.AsQueryable();

                    if (!string.IsNullOrEmpty(data.PickDate))
                        __dashboard = __dashboard.Where(c => c.DocDate == data.PickDate.toDate());
                    if (!string.IsNullOrEmpty(data.PickTime))
                        __dashboard = __dashboard.Where(c => c.Round_Index == round.FirstOrDefault(f => f.round_Name == data.PickTime).round_Index);

                    var queryResult2 = __dashboard.ToList();

                    var _MS_Route = route.Where(c => c.route_Id != "0").OrderBy(o => o.create_By).ToList();
                    //var _MS_Zone = context.MS_Zone.Where(c => Convert.ToInt32(c.Zone_Id) < 4).OrderBy(c => c.Zone_Name).ToList();
                    var _MS_Zone = zone.Where(c => c.zone_Name == "Frozen" || c.zone_Name == "Chill" || c.zone_Name == "Ambient" || c.zone_Name == "BULKY").OrderBy(c => c.zone_Name).ToList();
                    //var _MS_Round = context.MS_Round.ToList();

                    var result = new PickingViewModel();
                    result.PickDate = data.PickDate;
                    result.PickTime = data.PickTime;
                    //result.OrderQty = queryResult.Sum(c => c.QtyOrder);

                    var _PickingRouteList = new List<PickingRouteViewModel>();
                    foreach (var item2 in _MS_Route)
                    {
                        //var _queryResult2_Filter = queryResult.Where(c => c.Route_Index == item2.Route_Index);
                        //var _queryResult2_QtyOrder = _queryResult2_Filter == null ? 0 : _queryResult2_Filter.Sum(c => c.QtyOrder);

                        var _PickingRoute = new PickingRouteViewModel();
                        _PickingRoute.Route = item2.route_Name;
                        //_PickingRoute.RouteOrderQty = _queryResult2_QtyOrder;

                        var _PickingRouteOrderList = new List<PickingRouteOrderViewModel>();
                        var _PickingRouteOrderList2 = new List<PickingRouteOrderViewModel>();

                        var _queryResult2 = queryResult2.Where(c => c.Route_Index == item2.route_Index).Select(c => new { c.PlanGoodsIssue_No, c.Overall_StatusCode, c.UDF_4 })
                                                .OrderBy(c => c.PlanGoodsIssue_No).ToList();
                        var _countOrderDetail = 0;
                        if (_queryResult2.Count > 0)
                        {
                            foreach (var item3 in _queryResult2.Distinct())
                            {
                                _countOrderDetail = _countOrderDetail + 1;

                                var _PickingRouteOrder = new PickingRouteOrderViewModel();
                                var _PickingRouteOrder2 = new PickingRouteOrderViewModel();

                                var _PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                                var _split = _PlanGoodsIssue_No.Split('-');
                                if (_split.Length > 1)
                                    _PlanGoodsIssue_No = _split[0] + "-" + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                                else
                                {
                                    var preOrderNo = "";
                                    if (item3.UDF_4 == "Y") //_PlanGoodsIssue_No.Substring(0, 1) == "R")
                                    {
                                        preOrderNo = _PlanGoodsIssue_No.Substring(0, 2);
                                    }

                                    _PlanGoodsIssue_No = preOrderNo + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                                }


                                var isExist = _PickingRouteOrderList.Any(an => an.PlanGoodsIssue_No == item3.PlanGoodsIssue_No);

                                if (!isExist)
                                {
                                    //if (_countOrderDetail <= 12)
                                    //{
                                    _PickingRouteOrder.PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                                    _PickingRouteOrder.Order = _PlanGoodsIssue_No;
                                    _PickingRouteOrder.OverallStatus = item3.Overall_StatusCode;
                                    //}
                                    //else
                                    //{
                                    //    _PickingRouteOrder2.Order = _PlanGoodsIssue_No;
                                    //    _PickingRouteOrder2.OverallStatus = item3.Overall_StatusCode;
                                    //}

                                    var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                                    var queryResult3_Filter = queryResult2.Where(c => c.Route_Index == item2.route_Index && c.PlanGoodsIssue_No == item3.PlanGoodsIssue_No && !(c.Pick_Status == null || c.Pick_Status == string.Empty))
                                                                .Select(c => new { c.Zone_Index, c.Doc_ZONE, c.Pick_StatusCode }).ToList();
                                    foreach (var item4 in _MS_Zone)
                                    {
                                        Guid zoneId;

                                        if (item4.zone_Name.ToUpper() == "AMBIENT")
                                        {
                                            zoneId = new Guid("8307ef2a-cc12-459f-ae0c-7e1ae17c3ab7");

                                            var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                            var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                            _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                        }
                                        else if (item4.zone_Name.ToUpper() == "CHILL")
                                        {
                                            zoneId = new Guid("24464F80-8E3B-411E-BBCF-1062ABD4398B");

                                            var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                            var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                            _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                        }
                                        else
                                        {

                                            var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index);
                                            var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;
                                            
                                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                            _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                        }




                                    }
                                    //if (_countOrderDetail <= 12)
                                    //{
                                    _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                                    _PickingRouteOrderList.Add(_PickingRouteOrder);
                                    //}
                                    //else
                                    //{
                                    //    _PickingRouteOrder2.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                                    //    _PickingRouteOrderList2.Add(_PickingRouteOrder2);
                                    //}

                                    _PickingRoute.RouteOrderQty = _PickingRouteOrderList.Count();
                                }
                            }
                        }
                        else
                        {
                            var _PickingRouteOrder = new PickingRouteOrderViewModel();
                            var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                            foreach (var item4 in _MS_Zone)
                            {
                                var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                _PickingRouteOrderDetail.PickingStatus = "-";

                                _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                            }
                            _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                            _PickingRouteOrderList.Add(_PickingRouteOrder);

                            _PickingRoute.RouteOrderQty = 0;
                        }

                        _PickingRoute.PickingRouteOrderViewModel = _PickingRouteOrderList;
                        _PickingRoute.PickingRouteOrderViewModel2 = _PickingRouteOrderList2;
                        _PickingRouteList.Add(_PickingRoute);


                    }

                    result.OrderQty = _PickingRouteList.Select(c => c.RouteOrderQty).Sum();

                    #region +++++ Express +++++

                    var _PickingRouteExpress = new PickingRouteViewModel();
                    _PickingRouteExpress.Route = "EXPRESS";
                    //_PickingRouteExpress.RouteOrderQty = _queryResult2_QtyOrderExpress;

                    var _PickingRouteOrderListExpress = new List<PickingRouteOrderViewModel>();
                    var _PickingRouteOrderListExpress2 = new List<PickingRouteOrderViewModel>();

                    var strwhereExpress = new SqlParameter("@strwhereExpress", " and DocDate = '" + data.PickDate + "' and UDF_3 = 'Y' and DocumentType_Id = 'Express'"); //ตัดเงื่อนไขรอบออก
                    var queryResultExpress = context.__dashboard.Where(c => c.DocDate == data.PickDate.toDate() && c.UDF_3 == "Y"  && c.DocumentType_Id == "Express").ToList();


                    var _queryResultExpress2 = queryResultExpress.Select(c => new { c.PlanGoodsIssue_No, c.Overall_StatusCode, c.UDF_4, c.Round_Name }).OrderBy(c => c.PlanGoodsIssue_No).ToList();
                    var _countOrderDetailExpress = 0;

                    _PickingRouteExpress.RouteOrderQty = _queryResultExpress2.Distinct().Count();

                    if (_queryResultExpress2.Count > 0)
                    {
                        foreach (var item3 in _queryResultExpress2.Distinct())
                        {
                            _countOrderDetailExpress = _countOrderDetailExpress + 1;

                            var _PickingRouteOrderExpress = new PickingRouteOrderViewModel();
                            var _PickingRouteOrderExpress2 = new PickingRouteOrderViewModel();

                            var _PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                            var _split = _PlanGoodsIssue_No.Split('-');
                            if (_split.Length > 1)
                                _PlanGoodsIssue_No = _split[0] + "-" + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                            else
                            {
                                var preOrderNo = "";
                                if (item3.UDF_4 == "Y") //_PlanGoodsIssue_No.Substring(0, 1) == "R")
                                {
                                    preOrderNo = _PlanGoodsIssue_No.Substring(0, 2);
                                }

                                _PlanGoodsIssue_No = preOrderNo + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                            }
                            //_PlanGoodsIssue_No = _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);

                            //if (_countOrderDetailExpress <= 12)
                            //{
                            _PickingRouteOrderExpress.PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                            _PickingRouteOrderExpress.Order = _PlanGoodsIssue_No;
                            _PickingRouteOrderExpress.OverallStatus = item3.Overall_StatusCode;
                            //_PickingRouteOrderExpress.udf_2 = item3.UDF_2;
                            //}
                            //else
                            //{
                            //    _PickingRouteOrderExpress2.Order = _PlanGoodsIssue_No;
                            //    _PickingRouteOrderExpress2.OverallStatus = item3.Overall_StatusCode;
                            //}

                            var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                            var queryResult3_Filter = queryResultExpress.Where(c => c.PlanGoodsIssue_No == item3.PlanGoodsIssue_No && !(c.Pick_Status == null || c.Pick_Status == string.Empty))
                                                        .Select(c => new { c.Zone_Index, c.Doc_ZONE, c.Pick_StatusCode });
                            foreach (var item4 in _MS_Zone)
                            {
                                Guid zoneId;

                                if (item4.zone_Name.ToUpper() == "AMBIENT")
                                {
                                    zoneId = new Guid("8307ef2a-cc12-459f-ae0c-7e1ae17c3ab7");

                                    var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                    var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                    var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                    //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                    Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                    var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                    _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                    _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                    _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                }
                                else if (item4.zone_Name.ToUpper() == "CHILL")
                                {
                                    zoneId = new Guid("24464F80-8E3B-411E-BBCF-1062ABD4398B");

                                    var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                    var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                    var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                    //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                    Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                    var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                    _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                    _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                    _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                }
                                else
                                {

                                    var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index);
                                    var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                    var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                    //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                    Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                    var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                    _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                    _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                    _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                }



                            }
                            //if (_countOrderDetailExpress <= 12)
                            //{
                            _PickingRouteOrderExpress.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                            _PickingRouteOrderListExpress.Add(_PickingRouteOrderExpress);
                            //}
                            //else
                            //{
                            //    _PickingRouteOrderExpress2.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                            //    _PickingRouteOrderListExpress2.Add(_PickingRouteOrderExpress2);
                            //}
                        }
                    }
                    else
                    {
                        var _PickingRouteOrder = new PickingRouteOrderViewModel();
                        var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                        foreach (var item4 in _MS_Zone)
                        {
                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                            _PickingRouteOrderDetail.PickingStatus = "-";

                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                        }
                        _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                        _PickingRouteOrderListExpress.Add(_PickingRouteOrder);
                    }


                    _PickingRouteExpress.PickingRouteOrderViewModel = _PickingRouteOrderListExpress;
                    _PickingRouteExpress.PickingRouteOrderViewModel2 = _PickingRouteOrderListExpress2;
                    _PickingRouteList.Add(_PickingRouteExpress);

                    #endregion

                    result.PickingRouteViewModel = _PickingRouteList;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PickingViewModel PickingPerformanceSearch(PickingSearchViewModel data)
        {
            try
            {
                using (var context = new GIDbContext())
                {
                    string pstring = "";
                    if (!string.IsNullOrEmpty(data.PickDate))
                        pstring += " and DocDate = '" + data.PickDate + "'";
                    if (!string.IsNullOrEmpty(data.PickTime))
                        pstring += " and Round_Index = (select Round_Index from ms_Round where Round_Name = N'" + data.PickTime + "')";

                    string pstring2 = "";
                    if (!string.IsNullOrEmpty(data.PickDate))
                        pstring2 += " and DocDate = '" + data.PickDate + "'";
                    if (!string.IsNullOrEmpty(data.PickTime))
                        pstring2 += " and Round_Index = (select Round_Index from ms_Round where Round_Name = N'" + data.PickTime + "')";

                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var strwhere1 = new SqlParameter("@strwhere1", pstring2 + " and ISNULL(UDF_3, 'N') = 'N' and DocumentType_Id IN ('Normal','JD')");

                    //var queryResult = context.View_PickingRoute.FromSql("sp_GetViewPickingRoute @strwhere", strwhere).ToList();


                    var zone = utils.SendDataApi<List<ZoneViewModel>>(new AppSettingConfig().GetUrl("getZoneMaster"), new { }.sJson());
                    var round = utils.SendDataApi<List<RoundViewModel>>(new AppSettingConfig().GetUrl("getRoundMaster"), new { }.sJson());
                    var route = utils.SendDataApi<List<RouteViewModel>>(new AppSettingConfig().GetUrl("getRouteMaster"), new { }.sJson());

                    var __dashboard = context.__dashboard.AsQueryable();

                    if (!string.IsNullOrEmpty(data.PickDate))
                        __dashboard = __dashboard.Where(c => c.DocDate == data.PickDate.toDate());
                    if (!string.IsNullOrEmpty(data.PickTime))
                        __dashboard = __dashboard.Where(c => c.Round_Index == round.FirstOrDefault(f => f.round_Name == data.PickTime).round_Index);

                    var queryResult2 = __dashboard.ToList();

                    var _MS_Route = route.Where(c => c.route_Id != "0").OrderBy(o => o.create_By).ToList();
                    //var _MS_Zone = context.MS_Zone.Where(c => Convert.ToInt32(c.Zone_Id) < 4).OrderBy(c => c.Zone_Name).ToList();
                    var _MS_Zone = zone.Where(c => c.zone_Name == "Frozen" || c.zone_Name == "Chill" || c.zone_Name == "Ambient" || c.zone_Name == "BULKY").OrderBy(c => c.zone_Name).ToList();
                    //var _MS_Round = context.MS_Round.ToList();

                    var result = new PickingViewModel();
                    result.PickDate = data.PickDate;
                    result.PickTime = data.PickTime;
                    //result.OrderQty = queryResult.Sum(c => c.QtyOrder);

                    var _PickingRouteList = new List<PickingRouteViewModel>();
                    foreach (var item2 in _MS_Route)
                    {
                        //var _queryResult2_Filter = queryResult.Where(c => c.Route_Index == item2.Route_Index);
                        //var _queryResult2_QtyOrder = _queryResult2_Filter == null ? 0 : _queryResult2_Filter.Sum(c => c.QtyOrder);

                        var _PickingRoute = new PickingRouteViewModel();
                        _PickingRoute.Route = item2.route_Name;
                        //_PickingRoute.RouteOrderQty = _queryResult2_QtyOrder;

                        var _PickingRouteOrderList = new List<PickingRouteOrderViewModel>();
                        var _PickingRouteOrderList2 = new List<PickingRouteOrderViewModel>();

                        var _queryResult22 = queryResult2.Where(c => c.Route_Index == item2.route_Index).Select(c => new { c.PlanGoodsIssue_No, c.Overall_StatusCode, c.UDF_4 })
                                             .OrderBy(c => c.PlanGoodsIssue_No).ToList();

                        var _queryResult2 = queryResult2.Where(c => c.Route_Index == item2.route_Index).Select(c => new { c.PlanGoodsIssue_No, c.Overall_StatusCode, c.UDF_4,c.DocumentType_Id })
                                                .OrderBy(c => c.PlanGoodsIssue_No).ToList();
                        var _countOrderDetail = 0;
                        if (_queryResult2.Count > 0)
                        {
                            foreach (var item3 in _queryResult2.Distinct())
                            {
                                _countOrderDetail = _countOrderDetail + 1;

                                var _PickingRouteOrder = new PickingRouteOrderViewModel();
                                var _PickingRouteOrder2 = new PickingRouteOrderViewModel();

                                var _PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                                var _split = _PlanGoodsIssue_No.Split('-');
                                if (_split.Length > 1)
                                    _PlanGoodsIssue_No = _split[0] + "-" + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                                else
                                {
                                    var preOrderNo = "";
                                    if (item3.UDF_4 == "Y") //_PlanGoodsIssue_No.Substring(0, 1) == "R")
                                    {
                                        preOrderNo = _PlanGoodsIssue_No.Substring(0, 2);
                                    }

                                    _PlanGoodsIssue_No = preOrderNo + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                                }


                                var isExist = _PickingRouteOrderList.Any(an => an.PlanGoodsIssue_No == item3.PlanGoodsIssue_No);

                                if (!isExist)
                                {
                                    //if (_countOrderDetail <= 12)
                                    //{
                                    _PickingRouteOrder.PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                                    _PickingRouteOrder.Order = _PlanGoodsIssue_No;
                                    _PickingRouteOrder.OverallStatus = item3.Overall_StatusCode;
                                    //_PickingRouteOrder.udf_2 = item3.UDF_2;
                                    _PickingRouteOrder.documentType_Id = item3.DocumentType_Id;
                                    //}
                                    //else
                                    //{
                                    //    _PickingRouteOrder2.Order = _PlanGoodsIssue_No;
                                    //    _PickingRouteOrder2.OverallStatus = item3.Overall_StatusCode;
                                    //}

                                    var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                                    var queryResult3_Filter = queryResult2.Where(c => c.Route_Index == item2.route_Index && c.PlanGoodsIssue_No == item3.PlanGoodsIssue_No && !(c.Pick_Status == null || c.Pick_Status == string.Empty))
                                                                .Select(c => new { c.Zone_Index, c.Doc_ZONE, c.Pick_StatusCode }).ToList();
                                    foreach (var item4 in _MS_Zone)
                                    {
                                        Guid zoneId;

                                        if (item4.zone_Name.ToUpper() == "AMBIENT")
                                        {
                                            zoneId = new Guid("8307ef2a-cc12-459f-ae0c-7e1ae17c3ab7");

                                            var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                            var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                            _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                        }
                                        else if (item4.zone_Name.ToUpper() == "CHILL")
                                        {
                                            zoneId = new Guid("24464F80-8E3B-411E-BBCF-1062ABD4398B");

                                            var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                            var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                            _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                        }
                                        else
                                        {

                                            var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index);
                                            var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                            _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                        }




                                    }
                                    //if (_countOrderDetail <= 12)
                                    //{
                                    _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                                    _PickingRouteOrderList.Add(_PickingRouteOrder);
                                    //}
                                    //else
                                    //{
                                    //    _PickingRouteOrder2.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                                    //    _PickingRouteOrderList2.Add(_PickingRouteOrder2);
                                    //}

                                    _PickingRoute.RouteOrderQty = _PickingRouteOrderList.Count();
                                }
                            }
                        }
                        else
                        {
                            var _PickingRouteOrder = new PickingRouteOrderViewModel();
                            var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                            foreach (var item4 in _MS_Zone)
                            {
                                var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                _PickingRouteOrderDetail.PickingStatus = "-";

                                _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                            }
                            _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                            _PickingRouteOrderList.Add(_PickingRouteOrder);

                            _PickingRoute.RouteOrderQty = 0;
                        }

                        _PickingRoute.PickingRouteOrderViewModel = _PickingRouteOrderList;
                        _PickingRoute.PickingRouteOrderViewModel2 = _PickingRouteOrderList2;
                        _PickingRouteList.Add(_PickingRoute);


                    }

                    result.OrderQty = _PickingRouteList.Select(c => c.RouteOrderQty).Sum();

                    #region +++++ Express +++++

                    var _PickingRouteExpress = new PickingRouteViewModel();
                    _PickingRouteExpress.Route = "EXPRESS";
                    //_PickingRouteExpress.RouteOrderQty = _queryResult2_QtyOrderExpress;

                    var _PickingRouteOrderListExpress = new List<PickingRouteOrderViewModel>();
                    var _PickingRouteOrderListExpress2 = new List<PickingRouteOrderViewModel>();

                    var strwhereExpress = new SqlParameter("@strwhereExpress", " and DocDate = '" + data.PickDate + "' and UDF_3 = 'Y' and DocumentType_Id = 'Express'"); //ตัดเงื่อนไขรอบออก
                    var queryResultExpress = context.__dashboard.Where(c => c.DocDate == data.PickDate.toDate() && c.UDF_3 == "Y" && c.DocumentType_Id == "Express").ToList();


                    var _queryResultExpress2 = queryResultExpress.Select(c => new { c.PlanGoodsIssue_No, c.Overall_StatusCode, c.UDF_4, c.Round_Name }).OrderBy(c => c.PlanGoodsIssue_No).ToList();
                    var _countOrderDetailExpress = 0;

                    _PickingRouteExpress.RouteOrderQty = _queryResultExpress2.Distinct().Count();

                    if (_queryResultExpress2.Count > 0)
                    {
                        foreach (var item3 in _queryResultExpress2.Distinct())
                        {
                            _countOrderDetailExpress = _countOrderDetailExpress + 1;

                            var _PickingRouteOrderExpress = new PickingRouteOrderViewModel();
                            var _PickingRouteOrderExpress2 = new PickingRouteOrderViewModel();

                            var _PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                            var _split = _PlanGoodsIssue_No.Split('-');
                            if (_split.Length > 1)
                                _PlanGoodsIssue_No = _split[0] + "-" + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                            else
                            {
                                var preOrderNo = "";
                                if (item3.UDF_4 == "Y") //_PlanGoodsIssue_No.Substring(0, 1) == "R")
                                {
                                    preOrderNo = _PlanGoodsIssue_No.Substring(0, 2);
                                }

                                _PlanGoodsIssue_No = preOrderNo + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
                            }
                            //_PlanGoodsIssue_No = _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);

                            //if (_countOrderDetailExpress <= 12)
                            //{
                            _PickingRouteOrderExpress.PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
                            _PickingRouteOrderExpress.Order = _PlanGoodsIssue_No;
                            _PickingRouteOrderExpress.OverallStatus = item3.Overall_StatusCode;
                            //_PickingRouteOrderExpress.udf_2 = item3.UDF_2;
                            //}
                            //else
                            //{
                            //    _PickingRouteOrderExpress2.Order = _PlanGoodsIssue_No;
                            //    _PickingRouteOrderExpress2.OverallStatus = item3.Overall_StatusCode;
                            //}

                            var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                            var queryResult3_Filter = queryResultExpress.Where(c => c.PlanGoodsIssue_No == item3.PlanGoodsIssue_No && !(c.Pick_Status == null || c.Pick_Status == string.Empty))
                                                        .Select(c => new { c.Zone_Index, c.Doc_ZONE, c.Pick_StatusCode });
                            foreach (var item4 in _MS_Zone)
                            {
                                Guid zoneId;

                                if (item4.zone_Name.ToUpper() == "AMBIENT")
                                {
                                    zoneId = new Guid("8307ef2a-cc12-459f-ae0c-7e1ae17c3ab7");

                                    var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                    var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                    var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                    //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                    Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                    var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                    _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                    _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                    _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                }
                                else if (item4.zone_Name.ToUpper() == "CHILL")
                                {
                                    zoneId = new Guid("24464F80-8E3B-411E-BBCF-1062ABD4398B");

                                    var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index || c.Zone_Index == zoneId);
                                    var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                    var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                    //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                    Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                    var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                    _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                    _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                    _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                }
                                else
                                {

                                    var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.zone_Index);
                                    var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

                                    var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                                    //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                                    Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                                    var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                                    _PickingRouteOrderDetail.Zone = Doc_ZONE;
                                    _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

                                    _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                                }



                            }
                            //if (_countOrderDetailExpress <= 12)
                            //{
                            _PickingRouteOrderExpress.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                            _PickingRouteOrderListExpress.Add(_PickingRouteOrderExpress);
                            //}
                            //else
                            //{
                            //    _PickingRouteOrderExpress2.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                            //    _PickingRouteOrderListExpress2.Add(_PickingRouteOrderExpress2);
                            //}
                        }
                    }
                    else
                    {
                        var _PickingRouteOrder = new PickingRouteOrderViewModel();
                        var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
                        foreach (var item4 in _MS_Zone)
                        {
                            var Doc_ZONE = item4.zone_Name.ToUpper().Trim();
                            //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
                            Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.zone_Id;

                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
                            _PickingRouteOrderDetail.Zone = Doc_ZONE;
                            _PickingRouteOrderDetail.PickingStatus = "-";

                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
                        }
                        _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
                        _PickingRouteOrderListExpress.Add(_PickingRouteOrder);
                    }


                    _PickingRouteExpress.PickingRouteOrderViewModel = _PickingRouteOrderListExpress;
                    _PickingRouteExpress.PickingRouteOrderViewModel2 = _PickingRouteOrderListExpress2;
                    _PickingRouteList.Add(_PickingRouteExpress);

                    #endregion

                    result.PickingRouteViewModel = _PickingRouteList;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public PickingViewModel PickingPerformanceSearch_from_view(PickingSearchViewModel data)
        //{
        //    try
        //    {
        //        using (var context = new GIDbContext())
        //        {
        //            var result = new PickingViewModel();
        //            result.PickDate = data.PickDate;
        //            result.PickTime = data.PickTime;
        //            //result.OrderQty = model.Count();
        //            var _PickingRouteList = new List<PickingRouteViewModel>();

        //            {
        //                #region "Normal"

        //                string pstring = "";
        //                if (!string.IsNullOrEmpty(data.PickDate))
        //                    pstring += " and [Date] = '" + data.PickDate + "' and Route_Name != 'EXPRESS'";
        //                if (!string.IsNullOrEmpty(data.PickTime))
        //                    pstring += " and Round_Index = (select Round_Index from ms_Round where Round_Name = N'" + data.PickTime + "')";
        //                var strwhere = new SqlParameter("@strwhere", pstring);

        //                var model = context.View_Dashboard.FromSql("sp_View_Dashboard @strwhere", strwhere).ToList();
        //                result.OrderQty = model.Count();

        //                {
        //                    // Group Route
        //                    foreach (var m in model.GroupBy(g => g.Route_Name))
        //                    {
        //                        var route = model.Where(c => c.Route_Name == m.Key);
        //                        var _PickingRoute = new PickingRouteViewModel();
        //                        _PickingRoute.Route = m.Key;
        //                        _PickingRoute.RouteOrderQty = route.Count();

        //                        var _PickingRouteOrderList = new List<PickingRouteOrderViewModel>();

        //                        // Filter Data By Route
        //                        foreach (var mm in model.Where(c => c.Route_Name == m.Key).OrderBy(o => o.PlanGoodsIssue_No).ToList())
        //                        {
        //                            _PickingRoute.Seq = mm.Seq;

        //                            var _PickingRouteOrder = new PickingRouteOrderViewModel();
        //                            var _PlanGoodsIssue_No = mm.PlanGoodsIssue_No;
        //                            var _split = _PlanGoodsIssue_No.Split('-');
        //                            if (_split.Length > 1)
        //                                _PlanGoodsIssue_No = _split[0] + "-" + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
        //                            else
        //                            {
        //                                var preOrderNo = "";
        //                                if (mm.PlanGoodsIssue_No.Substring(0, 1) == "R")
        //                                    preOrderNo = _PlanGoodsIssue_No.Substring(0, 2);

        //                                _PlanGoodsIssue_No = preOrderNo + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
        //                            }

        //                            _PickingRouteOrder.Order = _PlanGoodsIssue_No;
        //                            _PickingRouteOrder.OverallStatus = mm.Status;

        //                            var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
        //                            for (int i = 0; i < 4; i++)
        //                            {
        //                                var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                                switch (i)
        //                                {
        //                                    case 0:
        //                                        _PickingRouteOrderDetail.Zone = "A";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.A;
        //                                        break;
        //                                    case 1:
        //                                        _PickingRouteOrderDetail.Zone = "B";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.B;
        //                                        break;
        //                                    case 2:
        //                                        _PickingRouteOrderDetail.Zone = "C";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.C;
        //                                        break;
        //                                    case 3:
        //                                        _PickingRouteOrderDetail.Zone = "F";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.F;
        //                                        break;
        //                                    default:
        //                                        break;
        //                                }

        //                                _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                            }

        //                            _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
        //                            _PickingRouteOrderList.Add(_PickingRouteOrder);
        //                            _PickingRoute.PickingRouteOrderViewModel = _PickingRouteOrderList.OrderBy(o => o.Order).ToList();

        //                        }

        //                        _PickingRouteList.Add(_PickingRoute);
        //                    }
        //                }

        //                #endregion
        //            }

        //            {
        //                #region "EXPRESS"

        //                string pstring = "";
        //                if (!string.IsNullOrEmpty(data.PickDate))
        //                    pstring += " and [Date] = '" + data.PickDate + "' and Route_Name = 'EXPRESS'";

        //                var strwhere = new SqlParameter("@strwhere", pstring);

        //                var model = context.View_Dashboard.FromSql("sp_View_Dashboard @strwhere", strwhere).ToList();

        //                {
        //                    // Group Route
        //                    foreach (var m in model.GroupBy(g => g.Route_Name))
        //                    {
        //                        var route = model.Where(c => c.Route_Name == m.Key);
        //                        var _PickingRoute = new PickingRouteViewModel();
        //                        _PickingRoute.Route = m.Key;
        //                        _PickingRoute.RouteOrderQty = route.Count();

        //                        var _PickingRouteOrderList = new List<PickingRouteOrderViewModel>();

        //                        // Filter Data By Route
        //                        foreach (var mm in model.Where(c => c.Route_Name == m.Key))
        //                        {
        //                            _PickingRoute.Seq = mm.Seq;

        //                            var _PickingRouteOrder = new PickingRouteOrderViewModel();
        //                            var _PlanGoodsIssue_No = mm.PlanGoodsIssue_No;
        //                            var _split = _PlanGoodsIssue_No.Split('-');
        //                            if (_split.Length > 1)
        //                                _PlanGoodsIssue_No = _split[0] + "-" + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
        //                            else
        //                            {
        //                                var preOrderNo = "";
        //                                if (mm.PlanGoodsIssue_No.Substring(0, 1) == "R")
        //                                    preOrderNo = _PlanGoodsIssue_No.Substring(0, 2);

        //                                _PlanGoodsIssue_No = preOrderNo + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
        //                            }

        //                            _PickingRouteOrder.Order = _PlanGoodsIssue_No;
        //                            _PickingRouteOrder.OverallStatus = mm.Status;

        //                            var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
        //                            for (int i = 0; i < 4; i++)
        //                            {
        //                                var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                                switch (i)
        //                                {
        //                                    case 0:
        //                                        _PickingRouteOrderDetail.Zone = "A";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.A;
        //                                        break;
        //                                    case 1:
        //                                        _PickingRouteOrderDetail.Zone = "B";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.B;
        //                                        break;
        //                                    case 2:
        //                                        _PickingRouteOrderDetail.Zone = "C";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.C;
        //                                        break;
        //                                    case 3:
        //                                        _PickingRouteOrderDetail.Zone = "F";
        //                                        _PickingRouteOrderDetail.PickingStatus = mm.F;
        //                                        break;
        //                                    default:
        //                                        break;
        //                                }

        //                                _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                            }

        //                            _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
        //                            _PickingRouteOrderList.Add(_PickingRouteOrder);
        //                            _PickingRoute.PickingRouteOrderViewModel = _PickingRouteOrderList.OrderBy(o => o.Order).ToList();

        //                        }

        //                        _PickingRouteList.Add(_PickingRoute);
        //                    }
        //                }

        //                #endregion
        //            }

        //            if (_PickingRouteList.Count < 7)
        //            {
        //                var _MS_Route = context.MS_Route.Where(c => c.Route_Id != "0").OrderBy(o => o.Create_By).ToList();

        //                foreach (var m in _MS_Route)
        //                {
        //                    var isExist = _PickingRouteList.Any(an => an.Route == m.Route_Name);
        //                    if (!isExist)
        //                    {
        //                        var _PickingRoute = new PickingRouteViewModel();
        //                        _PickingRoute.Route = m.Route_Name;
        //                        _PickingRoute.RouteOrderQty = 0;
        //                        _PickingRoute.Seq = m.Create_By;

        //                        var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
        //                        var _PickingRouteOrder = new PickingRouteOrderViewModel();
        //                        var _PickingRouteOrderList = new List<PickingRouteOrderViewModel>();
        //                        for (int i = 0; i < 4; i++)
        //                        {
        //                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                            switch (i)
        //                            {
        //                                case 0:
        //                                    _PickingRouteOrderDetail.Zone = "A";
        //                                    break;
        //                                case 1:
        //                                    _PickingRouteOrderDetail.Zone = "B";
        //                                    break;
        //                                case 2:
        //                                    _PickingRouteOrderDetail.Zone = "C";
        //                                    break;
        //                                case 3:
        //                                    _PickingRouteOrderDetail.Zone = "F";
        //                                    break;
        //                                default:
        //                                    break;
        //                            }

        //                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                        }
        //                        _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
        //                        _PickingRouteOrderList.Add(_PickingRouteOrder);
        //                        _PickingRoute.PickingRouteOrderViewModel = _PickingRouteOrderList;

        //                        _PickingRouteList.Add(_PickingRoute);
        //                    }
        //                }

        //                {
        //                    var isExist = _PickingRouteList.Any(an => an.Route == "EXPRESS");
        //                    if (!isExist)
        //                    {
        //                        var _PickingRoute = new PickingRouteViewModel();
        //                        _PickingRoute.Route = "EXPRESS";
        //                        _PickingRoute.RouteOrderQty = 0;
        //                        _PickingRoute.Seq = "Z";

        //                        var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
        //                        var _PickingRouteOrder = new PickingRouteOrderViewModel();
        //                        var _PickingRouteOrderList = new List<PickingRouteOrderViewModel>();
        //                        for (int i = 0; i < 4; i++)
        //                        {
        //                            var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                            switch (i)
        //                            {
        //                                case 0:
        //                                    _PickingRouteOrderDetail.Zone = "A";
        //                                    break;
        //                                case 1:
        //                                    _PickingRouteOrderDetail.Zone = "B";
        //                                    break;
        //                                case 2:
        //                                    _PickingRouteOrderDetail.Zone = "C";
        //                                    break;
        //                                case 3:
        //                                    _PickingRouteOrderDetail.Zone = "F";
        //                                    break;
        //                                default:
        //                                    break;
        //                            }

        //                            _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                        }
        //                        _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
        //                        _PickingRouteOrderList.Add(_PickingRouteOrder);
        //                        _PickingRoute.PickingRouteOrderViewModel = _PickingRouteOrderList;

        //                        _PickingRouteList.Add(_PickingRoute);
        //                    }
        //                }
        //            }

        //            result.PickingRouteViewModel = _PickingRouteList.OrderBy(o => o.Seq).ToList();
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Dastboard_Insert()
        {
            using (var context = new GIDbContext())
            {
                context.Database.SetCommandTimeout(90);
                try
                {

                    var commandText = "EXEC sp_View_Dashboard_Insert_";
                    var rowsAffected = context.Database.ExecuteSqlCommand(commandText);
                    if (rowsAffected != 0)
                    {
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //public PickingViewModel PickingPerformanceExpressSearch(PickingSearchViewModel data)
        //{
        //    try
        //    {
        //        using (var context = new GIDbContext())
        //        {
        //            var _MS_Round = context.MS_Round.Where(c => Convert.ToInt32(c.Round_Id) >= 0).OrderBy(o => o.Create_By).ToList();
        //            //var _MS_Zone = context.MS_Zone.Where(c => Convert.ToInt32(c.Zone_Id) < 4).OrderBy(c => c.Zone_Name).ToList();
        //            var _MS_Zone = context.MS_Zone.Where(c => c.Zone_Name == "Frozen" || c.Zone_Name == "Chill" || c.Zone_Name == "Ambient" || c.Zone_Name == "BULKY").OrderBy(c => c.Zone_Name).ToList();
        //            //var _MS_Round = context.MS_Round.ToList();

        //            var result = new PickingViewModel();
        //            result.PickDate = data.PickDate;
        //            result.PickTime = data.PickTime;
        //            //result.OrderQty = queryResult.Sum(c => c.QtyOrder);

        //            var _PickingRouteList = new List<PickingRouteViewModel>();

        //            #region +++++ Express +++++

        //            //_PickingRouteExpress.RouteOrderQty = _queryResult2_QtyOrderExpress;


        //            var strwhereExpress = new SqlParameter("@strwhereExpress", " and DocDate = '" + data.PickDate + "' and UDF_3 = 'Y' and DocumentType_Id = 'Express'"); //ตัดเงื่อนไขรอบออก
        //            var queryResultExpress = context.View_PickingRouteOrderDetail_Status.FromSql("sp_View_Dashboard_ @strwhereExpress", strwhereExpress).ToList();


        //            foreach (var item in _MS_Round)
        //            {
        //                var _queryResultExpress2 = queryResultExpress.Where(w => w.Round_Index == item.Round_Index).Select(c => new { c.PlanGoodsIssue_No, c.Overall_StatusCode, c.UDF_4,c.UDF_2 }).OrderBy(c => c.PlanGoodsIssue_No).ToList();
        //                var _countOrderDetailExpress = 0;

        //                var _PickingRouteOrderListExpress = new List<PickingRouteOrderViewModel>();
        //                var _PickingRouteExpress = new PickingRouteViewModel();
        //                _PickingRouteExpress.Route = item.Round_Name;
        //                _PickingRouteExpress.RouteTime = this.setRoundTime(item.Round_Id);

        //                _PickingRouteExpress.RouteOrderQty = _queryResultExpress2.Distinct().Count();

        //                if (_queryResultExpress2.Count > 0)
        //                {
        //                    foreach (var item3 in _queryResultExpress2.Distinct())
        //                    {
        //                        _countOrderDetailExpress = _countOrderDetailExpress + 1;

        //                        var _PickingRouteOrderExpress = new PickingRouteOrderViewModel();

        //                        var _PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
        //                        var _split = _PlanGoodsIssue_No.Split('-');
        //                        if (_split.Length > 1)
        //                            _PlanGoodsIssue_No = _split[0] + "-" + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
        //                        else
        //                        {
        //                            var preOrderNo = "";
        //                            if (item3.UDF_4 == "Y") //_PlanGoodsIssue_No.Substring(0, 1) == "R")
        //                            {
        //                                preOrderNo = _PlanGoodsIssue_No.Substring(0, 2);
        //                            }

        //                            _PlanGoodsIssue_No = preOrderNo + _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);
        //                        }
        //                        //_PlanGoodsIssue_No = _PlanGoodsIssue_No.Substring(_PlanGoodsIssue_No.Length - 4);

        //                        //if (_countOrderDetailExpress <= 12)
        //                        //{
        //                        _PickingRouteOrderExpress.PlanGoodsIssue_No = item3.PlanGoodsIssue_No;
        //                        _PickingRouteOrderExpress.Order = _PlanGoodsIssue_No;
        //                        _PickingRouteOrderExpress.OverallStatus = item3.Overall_StatusCode;
        //                        _PickingRouteOrderExpress.udf_2 = item3.UDF_2;
        //                        //}
        //                        //else
        //                        //{
        //                        //    _PickingRouteOrderExpress2.Order = _PlanGoodsIssue_No;
        //                        //    _PickingRouteOrderExpress2.OverallStatus = item3.Overall_StatusCode;
        //                        //}

        //                        var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
        //                        var queryResult3_Filter = queryResultExpress.Where(c => c.PlanGoodsIssue_No == item3.PlanGoodsIssue_No && !(c.Pick_Status == null || c.Pick_Status == string.Empty))
        //                                                    .Select(c => new { c.Zone_Index, c.Doc_Zone, c.Pick_StatusCode });
        //                        foreach (var item4 in _MS_Zone)
        //                        {
        //                            Guid zoneId;

        //                            if (item4.Zone_Name.ToUpper() == "AMBIENT")
        //                            {
        //                                zoneId = new Guid("8307ef2a-cc12-459f-ae0c-7e1ae17c3ab7");

        //                                var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.Zone_Index || c.Zone_Index == zoneId);
        //                                var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

        //                                var Doc_ZONE = item4.Zone_Name.ToUpper().Trim();
        //                                //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
        //                                Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.Zone_Id;

        //                                var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                                _PickingRouteOrderDetail.Zone = Doc_ZONE;
        //                                _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

        //                                _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                            }
        //                            else if (item4.Zone_Name.ToUpper() == "CHILL")
        //                            {
        //                                zoneId = new Guid("24464F80-8E3B-411E-BBCF-1062ABD4398B");

        //                                var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.Zone_Index || c.Zone_Index == zoneId);
        //                                var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

        //                                var Doc_ZONE = item4.Zone_Name.ToUpper().Trim();
        //                                //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
        //                                Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.Zone_Id;

        //                                var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                                _PickingRouteOrderDetail.Zone = Doc_ZONE;
        //                                _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

        //                                _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                            }
        //                            else
        //                            {

        //                                var _queryResult3_Filter = queryResult3_Filter.FirstOrDefault(c => c.Zone_Index == item4.Zone_Index);
        //                                var _queryResult3_PickingStatus = _queryResult3_Filter == null ? "-" : _queryResult3_Filter.Pick_StatusCode;

        //                                var Doc_ZONE = item4.Zone_Name.ToUpper().Trim();
        //                                //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
        //                                Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.Zone_Id;

        //                                var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                                _PickingRouteOrderDetail.Zone = Doc_ZONE;
        //                                _PickingRouteOrderDetail.PickingStatus = _queryResult3_PickingStatus;

        //                                _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                            }



        //                        }
        //                        //if (_countOrderDetailExpress <= 12)
        //                        //{
        //                        _PickingRouteOrderExpress.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
        //                        _PickingRouteOrderListExpress.Add(_PickingRouteOrderExpress);
        //                        //}
        //                        //else
        //                        //{
        //                        //    _PickingRouteOrderExpress2.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
        //                        //    _PickingRouteOrderListExpress2.Add(_PickingRouteOrderExpress2);
        //                        //}
        //                    }
        //                }
        //                else
        //                {
        //                    var _PickingRouteOrder = new PickingRouteOrderViewModel();
        //                    var _PickingRouteOrderDetailList = new List<PickingRouteOrderDetailViewModel>();
        //                    foreach (var item4 in _MS_Zone)
        //                    {
        //                        var Doc_ZONE = item4.Zone_Name.ToUpper().Trim();
        //                        //Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "CHILLED" ? "B" : Doc_ZONE == "TEMP" ? "C" : item4.Zone_Id;
        //                        Doc_ZONE = Doc_ZONE == "AMBIENT" ? "A" : Doc_ZONE == "BULKY" ? "B" : Doc_ZONE == "CHILL" ? "C" : Doc_ZONE == "FROZEN" ? "F" : item4.Zone_Id;

        //                        var _PickingRouteOrderDetail = new PickingRouteOrderDetailViewModel();
        //                        _PickingRouteOrderDetail.Zone = Doc_ZONE;
        //                        _PickingRouteOrderDetail.PickingStatus = "-";

        //                        _PickingRouteOrderDetailList.Add(_PickingRouteOrderDetail);
        //                    }
        //                    _PickingRouteOrder.PickingRouteOrderDetailViewModel = _PickingRouteOrderDetailList;
        //                    _PickingRouteOrderListExpress.Add(_PickingRouteOrder);
        //                }


        //                _PickingRouteExpress.PickingRouteOrderViewModel = _PickingRouteOrderListExpress;
        //                _PickingRouteList.Add(_PickingRouteExpress);

        //                result.PickingRouteViewModel = _PickingRouteList;
        //            }
        //            #endregion
        //            result.OrderQty = _PickingRouteList.Select(c => c.RouteOrderQty).Sum();


        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public string setRoundTime(string id)
        {
            switch (id)
            {
                case "1":
                    return "10:00 - 11:00";
                case "2":
                    return "11:00 - 12:00";
                case "3":
                    return "12:00 - 13:00";
                case "4":
                    return "13:00 - 14:00";
                case "5":
                    return "14:00 - 15:00";
                case "6":
                    return "15:00 - 16:00";
                case "7":
                    return "16:00 - 17:00";
                case "8":
                    return "17:00 - 18:00";
                case "9":
                    return "18:00 - 19:00";
                case "10":
                    return "19:00 - 20:00";
                case "11":
                    return "20:00 - 21:00";
                case "12":
                    return "21:00 - 22:00";
                default:
                    return "";
            }
        }

        public List<RoundViewModel> FilterRound()
        {
            try
            {
                using (var context = new GIDbContext())
                {
                    var round = utils.SendDataApi<List<RoundViewModel>>(new AppSettingConfig().GetUrl("getRoundMaster"), new { }.sJson());

                    return round;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
