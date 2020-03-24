using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Danliris.Service.Inventory.Lib.Models.GarmentLeftoverWarehouse.Stock;

namespace Com.Danliris.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Stock
{
    public interface IGarmentLeftoverWarehouseStockHistoryService
    {
        Task<int> StockIn(GarmentLeftoverWarehouseStock Stock);
        Task<int> StockIn(int StockId, double Quantity);
        Task<int> StockOut(int StockId, double Quantity);
        List<GarmentLeftoverWarehouseStockHistory> ReadAll();
    }
}
