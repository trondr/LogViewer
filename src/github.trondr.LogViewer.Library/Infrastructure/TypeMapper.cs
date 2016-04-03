using System.Collections.Generic;
using AutoMapper;

namespace github.trondr.LogViewer.Library.Infrastructure
{
    /// <summary>
    /// Source: https://github.com/trondr/NCmdLiner.SolutionCreator/blob/master/src/NCmdLiner.SolutionCreator.Library/BootStrap/TypeMapper.cs
    /// </summary>
    [Singleton]
    public class TypeMapper : ITypeMapper
    {
        private readonly IEnumerable<Profile> _typeMapperProfiles;
        private IMapper _mapper;
        private object _synch = new object();

        public TypeMapper(IEnumerable<Profile> typeMapperProfiles)
        {
            _typeMapperProfiles = typeMapperProfiles;
        }

        private IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    lock (_synch)
                    {
                        if (_mapper == null)
                        {
                            _mapper = ConfigureAndCreateMapper();
                        }
                    }
                }
                return _mapper;
            }
        }
        
        public T Map<T>(object source)
        {
            return Mapper.Map<T>(source);
        }

        private IMapper ConfigureAndCreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(ConfigureTypeMappers);
            return mapperConfiguration.CreateMapper();
        }

        private void ConfigureTypeMappers(IMapperConfiguration mapperConfiguration)
        {
            foreach (var profile in _typeMapperProfiles)
            {
                mapperConfiguration.AddProfile(profile);
            }
        }
    }
}