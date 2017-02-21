﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinExWebApp20272532.ViewModels
{
    public class ShipmentsReportViewModel
    {
        public virtual ShipmentsSearchViewModel Shipment { get; set; }
        public virtual IEnumerable<ShipmentsListViewModel> Shipments { get; set; }
    }
}