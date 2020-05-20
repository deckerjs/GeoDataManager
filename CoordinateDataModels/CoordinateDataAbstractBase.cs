using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateDataModels
{
    public abstract class CoordinateDataAbstractBase<T>: CoordinateDataInfo
    {
        public T Data { get; set; }
    }

}
