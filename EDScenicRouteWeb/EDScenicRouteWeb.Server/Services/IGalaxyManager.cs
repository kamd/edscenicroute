﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteWeb.Server.Services
{
    public interface IGalaxyManager
    {
        EDScenicRouteCore.GalaxyAgent GetAgent();
    }
}
