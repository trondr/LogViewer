using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Model;
using github.trondr.LogViewer.Library.ViewModels;

namespace github.trondr.LogViewer.Library.Services
{
    [Singleton]
    public class Mapper : IMapper
    {
        private readonly ILogLevelViewModelProvider _logLevelViewModelProvider;
        private readonly ILoggerViewModelProvider _loggerViewModelProvider;

        public Mapper(ILogLevelViewModelProvider logLevelViewModelProvider, ILoggerViewModelProvider loggerViewModelProvider)
        {
            _logLevelViewModelProvider = logLevelViewModelProvider;
            _loggerViewModelProvider = loggerViewModelProvider;
            //AutoMapper.Mapper.CreateMap<LogLevel, LogLevelViewModel>()
            //    .ForMember(model => model.Level, expression => expression.MapFrom(level => _logLevelViewModelProvider.GetLevel(level.ToString())));
            AutoMapper.Mapper.CreateMap<LogItem, LogItemViewModel>()
                .ForMember(model => model.Time, expression => expression.MapFrom(item => item.Time))
                .ForMember(model => model.LogLevel, expression => expression.MapFrom(item => _logLevelViewModelProvider.GetLevel(item.LogLevel.ToString())))
                .ForMember(model => model.Logger,expression => expression.MapFrom(item => _loggerViewModelProvider.GetLogger(item.Logger)))
                .ForMember(model => model.ThreadId, expression => expression.MapFrom(item => item.ThreadId))
                .ForMember(model => model.Message, expression => expression.MapFrom(item => item.Message));
        }

        public T Map<T>(object source)
        {
            return AutoMapper.Mapper.Map<T>(source);
        }
    }
}