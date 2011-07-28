using System.Collections.Concurrent;
using System.Linq;

namespace Euclid.Composites.Mvc
{
    public class BiDirectionalMapperCollection
    {
        private static readonly BlockingCollection<IMapper> Mappers = new BlockingCollection<IMapper>();

        public void Add<T, TPrime>(IBiDirectionalMapper<T, TPrime> mapper)
        {
            Mappers.Add(mapper as IMapper);
        }

        public IBiDirectionalMapper<T, TPrime> Get<T, TPrime>()
        {
            return Mappers
                       .Cast<IBiDirectionalMapper<T, TPrime>>()
                       .Where(mapper =>
                              (mapper.Left == typeof (T) && mapper.Right == typeof (TPrime))
                              ||
                              (mapper.Left == typeof (TPrime) && mapper.Right == typeof (T)))
                       .FirstOrDefault();
        }

    }
}
