﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;

namespace SinExWebApp20272532.ViewModels
{
    public class ShipmentsReportViewModel
    {
        public virtual ShipmentsSearchViewModel Shipment { get; set; }
        public virtual IPagedList<ShipmentsListViewModel> Shipments { get; set; }
    }
}