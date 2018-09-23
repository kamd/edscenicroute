﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.Data
{
    interface IGalaxyStore
    {
        IQueryable<GalacticSystem> Systems { get;  }
        IQueryable<GalacticPOI> POIs { get;  }
        Task<GalacticSystem> ResolvePlaceByName(string name);
        Task<GalacticSystem> ResolveSystemByName(string name);
        void Save();
        Task UpdateFromLocalFiles(CancellationToken cancellationToken);
    }
}
