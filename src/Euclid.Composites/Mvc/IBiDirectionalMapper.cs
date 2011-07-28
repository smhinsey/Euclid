using System;

namespace Euclid.Composites.Mvc
{
    public interface IBiDirectionalMapper<T, TPrime>
    {
        Type Left { get; }
        Type Right { get; }

        TPrime Map(T mappingSource);
        T Map(TPrime mappingSource);
    }
}