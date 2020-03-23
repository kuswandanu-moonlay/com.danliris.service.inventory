using Com.Danliris.Service.Inventory.Lib.Models.GarmentLeftoverWarehouse.Stock;
using Com.Danliris.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Stock;
using Com.Danliris.Service.Inventory.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Inventory.WebApi.Controllers.v1.GarmentLeftoverWarehouse.Stock
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment/leftover-warehouse-stocks")]
    public class StockHistoryController : Controller
    {
        IGarmentLeftoverWarehouseStockHistoryService service;

        public StockHistoryController(IGarmentLeftoverWarehouseStockHistoryService service)
        {
            this.service = service;
        }

        [HttpGet]
        public virtual IActionResult Get()
        {
            try
            {
                List<GarmentLeftoverWarehouseStockHistory> result = service.ReadAll();

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, e.Message);
            }
        }
    }
}
