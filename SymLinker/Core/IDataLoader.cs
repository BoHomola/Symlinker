using SymLinker.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymLinker.Core
{
    public interface IDataLoader<TData> where TData : IDataEntity
    {
        public TData LoadData();
        public void SaveData(TData data);
    }
}
