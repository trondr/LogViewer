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
        private readonly ISourceCodeInfoProvider _sourceCodeInfoProvider;

        public LogItemTypeMapperProfile(ILogLevelViewModelProvider logLevelViewModelProvider,
            ILoggerViewModelProvider loggerViewModelProvider, 
            ISourceCodeInfoProvider sourceCodeInfoProvider)
        {
            _logLevelViewModelProvider = logLevelViewModelProvider;
            _loggerViewModelProvider = loggerViewModelProvider;
            _sourceCodeInfoProvider = sourceCodeInfoProvider;
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
                .ForMember(model => model.SourceCode, expression => expression.MapFrom(item => _sourceCodeInfoProvider.GetSourceCode(item.SourceFileName)))
                .ForMember(model => model.SourceCodeDetails, expression => expression.MapFrom(item => GetSourceCodeDetails(item)))
                .ForMember(model => model.SourceCodeLine, expression => expression.MapFrom(item => item.SourceFileLineNr)
                );
        }

        private string GetSourceCodeDetails(LogItem item)
        {
            var sourceCodeDetails = $"File: '{item.SourceFileName}', Class: '{item.CallSiteClass}' Method: '{item.CallSiteMethod}' Line: '{item.SourceFileLineNr}'";
            return sourceCodeDetails;
        }
    }
}
