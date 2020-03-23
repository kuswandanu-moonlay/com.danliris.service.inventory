using Com.Danliris.Service.Inventory.Lib.Enums;
using Com.Danliris.Service.Inventory.Lib.Models.GarmentLeftoverWarehouse.Stock;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Stock
{
    public class GarmentLeftoverWarehouseStockService : IGarmentLeftoverWarehouseStockService
    {
        private const string UserAgent = "GarmentLeftoverWarehouseStockService";

        private InventoryDbContext DbContext;
        private DbSet<GarmentLeftoverWarehouseStock> DbSet;

        private readonly IServiceProvider ServiceProvider;
        private readonly IIdentityService IdentityService;

        public GarmentLeftoverWarehouseStockService(InventoryDbContext dbContext, IServiceProvider serviceProvider)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<GarmentLeftoverWarehouseStock>();

            ServiceProvider = serviceProvider;
            IdentityService = (IIdentityService)serviceProvider.GetService(typeof(IIdentityService));
        }

        public async Task<int> StockIn(GarmentLeftoverWarehouseStock stock)
        {
            try
            {
                int Affected = 0;

                var Query = DbSet.Where(w => w.ReferenceType == stock.ReferenceType && w.UnitId == stock.UnitId);

                switch (stock.ReferenceType)
                {
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.RO:
                        Query = Query.Where(w => w.RONo == stock.RONo);
                        break;
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.PO:
                        Query = Query.Where(w => w.PONo == stock.PONo);
                        break;
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.KG:
                        Query = Query.Where(w => w.KG == stock.KG);
                        break;
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.PRODUCT:
                        Query = Query.Where(w => w.ProductId == stock.ProductId && w.UomId == stock.UomId);
                        break;
                }

                var existingStock = Query.SingleOrDefault();
                if (existingStock == null)
                {
                    stock.FlagForCreate(IdentityService.Username, UserAgent);
                    stock.FlagForUpdate(IdentityService.Username, UserAgent);

                    stock.Histories = new List<GarmentLeftoverWarehouseStockHistory>();
                    GarmentLeftoverWarehouseStockHistory stockHistory = new GarmentLeftoverWarehouseStockHistory
                    {
                        StockType = GarmentLeftoverWarehouseStockTypeEnum.IN,
                        BeforeQuantity = 0,
                        Quantity = stock.Quantity,
                        AfterQuantity = stock.Quantity
                    };
                    stockHistory.FlagForCreate(IdentityService.Username, UserAgent);
                    stockHistory.FlagForUpdate(IdentityService.Username, UserAgent);
                    stock.Histories.Add(stockHistory);

                    DbSet.Add(stock);
                }
                else
                {
                    existingStock.Quantity += stock.Quantity;
                    existingStock.FlagForUpdate(IdentityService.Username, UserAgent);
                }

                Affected = await DbContext.SaveChangesAsync();
                return Affected;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> StockOut(GarmentLeftoverWarehouseStock stock)
        {
            try
            {
                int Affected = 0;

                var Query = DbSet.Where(w => w.ReferenceType == stock.ReferenceType && w.UnitId == stock.UnitId);

                switch (stock.ReferenceType)
                {
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.RO:
                        Query = Query.Where(w => w.RONo == stock.RONo);
                        break;
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.PO:
                        Query = Query.Where(w => w.PONo == stock.PONo);
                        break;
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.KG:
                        Query = Query.Where(w => w.KG == stock.KG);
                        break;
                    case GarmentLeftoverWarehouseStockReferenceTypeEnum.PRODUCT:
                        Query = Query.Where(w => w.ProductId == stock.ProductId && w.UomId == stock.UomId);
                        break;
                }

                var existingData = Query.Single();
                existingData.Quantity -= stock.Quantity;
                existingData.FlagForUpdate(IdentityService.Username, UserAgent);

                Affected = await DbContext.SaveChangesAsync();
                return Affected;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
