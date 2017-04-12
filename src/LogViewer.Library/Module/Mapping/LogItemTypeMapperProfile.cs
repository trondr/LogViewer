using AutoMapper;
using LogViewer.Library.Module.Model;
using LogViewer.Library.Module.Services;
using LogViewer.Library.Module.ViewModels;

namespace LogViewer.Library.Module.Mapping
{
    public class LogItemTypeMapperProfile: Profile
    {
        private readonly ILogLevelViewModelProvider _logLevelViewModelProvider;
        private readonly ILoggerViewModelProvider _loggerViewModelProvider;

        public LogItemTypeMapperProfile(ILogLevelViewModelProvider logLevelViewModelProvider,ILoggerViewModelProvider loggerViewModelProvider)
        {
            _logLevelViewModelProvider = logLevelViewModelProvider;
            _loggerViewModelProvider = loggerViewModelProvider;
        }

        protected override void Configure()
        {
            CreateMap<LogItem, LogItemViewModel>()
                .ForMember(model => model.Time, expression => expression.MapFrom(item => item.Time))
                .ForMember(model => model.LogLevel, expression => expression.MapFrom(item => _logLevelViewModelProvider.GetLevel(item.LogLevel.ToString())))
                .ForMember(model => model.Logger,expression => expression.MapFrom(item => _loggerViewModelProvider.GetLogger(item.Logger)))
                .ForMember(model => model.ThreadId, expression => expression.MapFrom(item => item.ThreadId))
                .ForMember(model => model.Message, expression => expression.MapFrom(item => item.Message))
                .ForMember(model => model.ExceptionString, expression => expression.MapFrom(item => item.ExceptionString))
                .ForMember(model => model.IsVisible, expression => expression.Ignore())
                .ForMember(model => model.SourceCode, expression => expression.Ignore()
                );

        }
    }
}
