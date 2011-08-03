//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Euclid.Composites.Maps
//{
//    public class MapperRegistry
//    {
//        private readonly List<IMapper> _mappers = new List<IMapper>();

//        public void Add<TSource, TDestination>(IMapper<TSource, TDestination> mapper)
//        {
//            _mappers.Add(mapper);
//        }

//        public IMapper<TSource, TDestination> Get<TSource, TDestination>()
//        {
//            var mapper = _mappers.Cast<IMapper<TSource, TDestination>>()
//                            .Where(m => 
//                                   m.Source == typeof (TSource) && 
//                                   m.Destination == typeof (TDestination))
//                            .FirstOrDefault();

//            if (mapper == null)
//            {
//                throw new MapperNotFoundException(typeof (TSource), typeof (TDestination));
//            }

//            return mapper;
//        }

//        public TDestination GetInstanceFromMap<TDestination>(Type source)
//        {
//            var mapper = _mappers
//                            .Where(m =>
//                                   m.Source == source &&
//                                   m.Destination == typeof(TDestination))
//                            .FirstOrDefault();

//            if (mapper == null)
//            {
//                throw new MapperNotFoundException(source, typeof(TDestination));
//            }

//            return (TDestination)mapper.Map(source);
//        }
//    }
//}