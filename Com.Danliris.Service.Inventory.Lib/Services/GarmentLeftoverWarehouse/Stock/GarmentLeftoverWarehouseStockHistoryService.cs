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
    public class GarmentLeftoverWarehouseStockHistoryService : IGarmentLeftoverWarehouseStockHistoryService
    {
        private const string UserAgent = "GarmentLeftoverWarehouseStockHistoryService";

        private InventoryDbContext DbContext;
        private DbSet<GarmentLeftoverWarehouseStockHistory> DbSet;

        private readonly IServiceProvider ServiceProvider;
        private readonly IIdentityService IdentityService;

        public GarmentLeftoverWarehouseStockHistoryService(InventoryDbContext dbContext, IServiceProvider serviceProvider)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<GarmentLeftoverWarehouseStockHistory>();

            ServiceProvider = serviceProvider;
            IdentityService = (IIdentityService)serviceProvider.GetService(typeof(IIdentityService));
        }

        public async Task<int> StockIn(GarmentLeftoverWarehouseStock Stock)
        {
            try
            {
                int Affected = 0;

                double beforeQuantity = 0;

                var previousData = DbSet.Where(w => w.StockId == Stock.Id).OrderBy(o => o._CreatedUtc).LastOrDefault();
                if (previousData != null)
                {
                    beforeQuantity = previousData.AfterQuantity;
                }

                var newData = new GarmentLeftoverWarehouseStockHistory
                {
                    StockId = Stock.Id,
                    StockType = GarmentLeftoverWarehouseStockTypeEnum.IN,
                    BeforeQuantity = beforeQuantity,
                    Quantity = Stock.Quantity,
                    AfterQuantity = beforeQuantity + Stock.Quantity
                };
                newData.FlagForCreate(IdentityService.Username, UserAgent);
                newData.FlagForUpdate(IdentityService.Username, UserAgent);

                DbSet.Add(newData);

                Affected = await DbContext.SaveChangesAsync();
                return Affected;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> StockIn(int StockId, double Quantity)
        {
            try
            {
                int Affected = 0;

                double beforeQuantity = 0;

                var previousData = DbSet.Where(w => w.StockId == StockId).OrderBy(o => o._CreatedUtc).LastOrDefault();
                if (previousData != null)
                {
                    beforeQuantity = previousData.AfterQuantity;
                }

                var newData = new GarmentLeftoverWarehouseStockHistory
                {
                    StockId = StockId,
                    StockType = GarmentLeftoverWarehouseStockTypeEnum.IN,
                    BeforeQuantity = beforeQuantity,
                    Quantity = Quantity,
                    AfterQuantity = beforeQuantity + Quantity
                };
                newData.FlagForCreate(IdentityService.Username, UserAgent);
                newData.FlagForUpdate(IdentityService.Username, UserAgent);

                DbSet.Add(newData);

                Affected = await DbContext.SaveChangesAsync();
                return Affected;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> StockOut(int StockId, double Quantity)
        {
            try
            {
                int Affected = 0;

                double beforeQuantity = 0;

                var previousData = DbSet.Where(w => w.StockId == StockId).OrderBy(o => o._CreatedUtc).LastOrDefault();
                if (previousData != null)
                {
                    beforeQuantity = previousData.AfterQuantity;
                }

                var newData = new GarmentLeftoverWarehouseStockHistory
                {
                    StockId = StockId,
                    StockType = GarmentLeftoverWarehouseStockTypeEnum.OUT,
                    BeforeQuantity = beforeQuantity,
                    Quantity = -Quantity,
                    AfterQuantity = beforeQuantity - Quantity
                };
                newData.FlagForCreate(IdentityService.Username, UserAgent);
                newData.FlagForUpdate(IdentityService.Username, UserAgent);

                DbSet.Add(newData);

                Affected = await DbContext.SaveChangesAsync();
                return Affected;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<GarmentLeftoverWarehouseStockHistory> ReadAll()
        {
            var data = DbSet.ToList();
            return data;
        }
    }
}
