﻿using Com.Danliris.Service.Inventory.Lib.Enums;
using Com.Moonlay.Models;

namespace Com.Danliris.Service.Inventory.Lib.Models.GarmentLeftoverWarehouse.Stock
{
    public class GarmentLeftoverWarehouseStockHistory : StandardEntity
    {
        public int StockId { get; set; }
        public GarmentLeftoverWarehouseStock Stock { get; set; }

        public GarmentLeftoverWarehouseStockTypeEnum StockType { get; set; }

        public double BeforeQuantity { get; set; }
        public double Quantity { get; set; }
        public double AfterQuantity { get; set; }
    }
}
