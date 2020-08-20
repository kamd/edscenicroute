using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.Data
{
    public interface IGalaxyStore
    {
        IGalaxyStoreAgent GetAgent();
        void Save();
        Task UpdateFromLocalFiles(CancellationToken cancellationToken);
    }
}
