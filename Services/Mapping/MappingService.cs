using AutoMapper;
using DocumentHelper.Services.Mapping.Profiles;
using Microsoft.Extensions.Logging;

namespace DocumentHelper.Services.Mapping
{
    public static class MappingService
    {
        private static readonly IMapper _mapper;

        static MappingService()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DocumentMappingProfile>();
            }, loggerFactory);

            _mapper = configuration.CreateMapper();
        }

        public static IMapper Mapper => _mapper;

    }
}
