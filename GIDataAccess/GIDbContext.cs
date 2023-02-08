using GIDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class GIDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public virtual DbSet<GetValueByColumn> GetValueByColumn { get; set; }
        public DbSet<im_GoodsIssue> IM_GoodsIssue { get; set; }
        public DbSet<im_GoodsIssueItemLocationSN> im_GoodsIssueItemLocationSN { get; set; }
        public DbSet<im_GoodsIssueItem> IM_GoodsIssueItem { get; set; }
        public DbSet<im_GoodsIssueItemLocation> IM_GoodsIssueItemLocation { get; set; }
        public DbSet<im_PlanGoodsIssue> IM_PlanGoodsIssue { get; set; }
        public DbSet<im_PlanGoodsIssueItem> IM_PlanGoodsIssueItem { get; set; }
        public DbSet<im_Pack> IM_Pack { get; set; }
        public DbSet<im_PackItem> IM_PackItem { get; set; }
        public DbSet<im_Task> IM_Task { get; set; }
        public DbSet<im_logTote> im_logTote { get; set; }
        public DbSet<im_TaskItem> IM_TaskItem { get; set; }
        public DbSet<ms_DocumentType> MS_DocumentType { get; set; }
        public DbSet<ms_DocumentTypeNumber> MS_DocumentTypeNumber { get; set; }
        public DbSet<Sy_AutoNumber> Sy_AutoNumber { get; set; }
        public DbSet<sy_Process> Sy_Process { get; set; }
        public DbSet<sy_ProcessStatus> Sy_ProcessStatus { get; set; }
        public DbSet<wm_TagOut> WM_TagOut { get; set; }
        public DbSet<wm_TagOutItem> WM_TagOutItem { get; set; }
        public DbSet<wm_TagOutPick> WM_TagOutPick { get; set; }

        public DbSet<__overall_round> __overall_round { get; set; }
        public DbSet<__overall_round_pick> __overall_round_pick { get; set; }
        public DbSet<__overall_route> __overall_route { get; set; }
        public DbSet<__overall_status> __overall_status { get; set; }
        public DbSet<__overall_status_Express> __overall_status_Express { get; set; }
        public DbSet<__overall_zone_pick> __overall_zone_pick { get; set; }
        public DbSet<__dashboard> __dashboard { get; set; }

        public virtual DbSet<View_GoodsIssue> View_GoodsIssue { get; set; }

        public DbSet<View_PLANWAVEV> View_PLANWAVEV { get; set; }

        public DbSet<View_PLANWAVEbyPLANGI> View_PLANWAVEbyPLANGI { get; set; }
        public DbSet<View_PLANWAVEbyPLANGIV2> View_PLANWAVEbyPLANGIV2 { get; set; }
        public DbSet<View_Task> View_Task { get; set; }
        public DbSet<View_TaskLabeling> View_TaskLabeling { get; set; }
        public DbSet<View_TaskInsertBinCard> View_TaskInsertBinCard { get; set; }
        public DbSet<im_Signatory_log> im_Signatory_log { get; set; }
        public DbSet<View_PrintOutGI> View_PrintOutGI { get; set; }
        public DbSet<View_PrintOutGI_PlanGI> View_PrintOutGI_PlanGI { get; set; }
        
        public DbSet<View_RPT_Picking_Tag_GI> View_RPT_Picking_Tag_GI { get; set; }
        public DbSet<View_RPT_Picking_Tag_PlanGI> View_RPT_Picking_Tag_PlanGI { get; set; }
        public DbSet<View_RPT_Picking_Tag_TaskItem> View_RPT_Picking_Tag_TaskItem { get; set; }
        public DbSet<View_RPT_Pick_GI> View_RPT_Pick_GI { get; set; }
        public DbSet<View_RPT_Pick_PlanGI> View_RPT_Pick_PlanGI { get; set; }

        public DbSet<View_RPT_Pick_TaskItem> View_RPT_Pick_TaskItem { get; set; }

        public DbSet<View_WaveCheckProductLot> View_WaveCheckProductLot { get; set; }
        public DbSet<View_GoodsIssueItemLocation_tag> View_GoodsIssueItemLocation_tag { get; set; }
        public DbSet<View_GoodsIssuecount_tag> View_GoodsIssuecount_tag { get; set; }
        public DbSet<View_GoodsIssuecount_tag_SelectTive> View_GoodsIssuecount_tag_SelectTive { get; set; }
        public DbSet<View_tagOut> View_tagOut { get; set; }
        public DbSet<View_TaskInsertBinCard_V2> View_TaskInsertBinCard_V2 { get; set; }
        public DbSet<View_GoodsIssuecount_tag_VC> View_GoodsIssuecount_tag_VC { get; set; }
        public DbSet<im_TruckLoad> im_TruckLoad { get; set; }
        public DbSet<im_TruckLoadItem> im_TruckLoadItem { get; set; }
        public DbSet<View_PrintTagOut> View_PrintTagOut { get; set; }
        public DbSet<View_TaskInsertBinCardWithLoad> View_TaskInsertBinCardWithLoad { get; set; }

        public DbSet<View_GoodsIssueCheckBOM> View_GoodsIssueCheckBOM { get; set; }
        public DbSet<View_GoodsIssueBOM> View_GoodsIssueBOM { get; set; }

        public DbSet<im_TaskItemTmp> im_TaskItemTmp { get; set; }

        public DbSet<View_GoodsIssueCheckNotBOM> View_GoodsIssueCheckNotBOM { get; set; }
        public DbSet<View_Taskitem_with_Truckload> View_Taskitem_with_Truckload { get; set; }
        public DbSet<View_Task_stg_toDock> View_Task_stg_toDock { get; set; }
        public DbSet<View_TaskAftersplit> View_TaskAftersplit { get; set; }
        public DbSet<View_RPT_PickingPlan> View_RPT_PickingPlan { get; set; }
        public DbSet<View_Get_location_DoclSTG> View_Get_location_DoclSTG { get; set; }
        public DbSet<View_ScanPickToDock> View_ScanPickToDock { get; set; }
        public DbSet<View_ScanPicKSTG_To_Dock> View_ScanPicKSTG_To_Dock { get; set; }
        public DbSet<View_Taskitem_Labeling> View_Taskitem_Labeling { get; set; }
        public DbSet<View_Taskitem_with_Truckload_PICKQTY> View_Taskitem_with_Truckload_PICKQTY { get; set; }
        public DbSet<View_Taskitem_MoveOnGround> View_Taskitem_MoveOnGround { get; set; }
        public DbSet<View_Taskitem_with_Truckload_PICKQTY_GETPLAN> View_Taskitem_with_Truckload_PICKQTY_GETPLAN { get; set; }
        public DbSet<CheckB4Wave> CheckB4Wave { get; set; }
        public DbSet<CheckWaveDip> CheckWaveDip { get; set; }
        public DbSet<CheckWaveDipbyWave> CheckWaveDipbyWave { get; set; }
        public DbSet<CheckWaveWCS> CheckWaveWCS { get; set; }
        public DbSet<CheckWaveWCS_status> CheckWaveWCS_status { get; set; }
        public DbSet<View_location_PP_waveEnd> View_location_PP_waveEnd { get; set; }
        public DbSet<RoundWave> RoundWave { get; set; }
        public DbSet<RoundWaveTimeAppointment> RoundWaveTimeAppointment { get; set; }
        public DbSet<CheckFOC> CheckFOC { get; set; }
        public DbSet<sp_Fixwave> sp_Fixwave { get; set; }
        public DbSet<sp_ChecktagByGi> sp_ChecktagByGi { get; set; }
        public DbSet<sp_ChecktagGi> sp_ChecktagGi { get; set; }
        public DbSet<sp_ChecktagTf> sp_ChecktagTf { get; set; }
        public DbSet<sp_UpdateBinBalance> sp_UpdateBinBalance { get; set; }
        public DbSet<sp_DeleteGoodsIssueitemLocation> sp_DeleteGoodsIssueitemLocation { get; set; }
        


        public DbSet<View_Checkstatus_truckload_order> View_Checkstatus_truckload_order { get; set; }
        public virtual DbSet<View_WaveBinBalanceViewModel_Ace> View_WaveBinBalanceViewModel_Ace { get; set; }
        public virtual DbSet<View_TaskInsertBinCard_wavewithoutrobot> View_TaskInsertBinCard_wavewithoutrobot { get; set; }
        public DbSet<log_Waveprocress> log_Waveprocress { get; set; }
        public DbSet<View_CutError_Stock> View_CutError_Stock { get; set; }
        public DbSet<View_Task_export> View_Task_export { get; set; }
        public DbSet<View_Task_moveSTG> View_Task_moveSTG { get; set; }
        public DbSet<View_TaskInsertBinCard_PickTiLight> View_TaskInsertBinCard_PickTiLight { get; set; }

        public DbSet<log_api_reponse> log_api_reponse { get; set; }
        public DbSet<log_api_request> log_api_request { get; set; }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }

            //optionsBuilder.UseSqlServer(@"Server=kascoit.ddns.net,22017;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=sa;password=K@sc0db12345;");

            //optionsBuilder.UseSqlServer(@"Server=10.0.177.33\SQLEXPRESS;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=cfrffmusr;password=ffmusr@cfr;");
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
