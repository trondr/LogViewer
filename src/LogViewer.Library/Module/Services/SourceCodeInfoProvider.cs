using System.Collections.Generic;
using System.IO;
using LogViewer.Library.Infrastructure.LifeStyles;

namespace LogViewer.Library.Module.Services
{
    [Singleton]
    public class SourceCodeInfoProvider : ISourceCodeInfoProvider
    {
        readonly Dictionary<string,SourceCodeInfo> _sourceCodeInfos = new Dictionary<string, SourceCodeInfo>();
        
        public SourceCodeInfo GetSourceCode(string fileName)
        {
            SourceCodeInfo sourceCodeInfo;
            if (_sourceCodeInfos.ContainsKey(fileName))
            {
                sourceCodeInfo = _sourceCodeInfos[fileName];
            }
            else
            {
                sourceCodeInfo = new SourceCodeInfo()
                {
                    Code = GetCode(fileName)
                };
                _sourceCodeInfos.Add(fileName, sourceCodeInfo);
            }
            return sourceCodeInfo;
        }

        private string GetCode(string fileName)
        {
            string code;
            if (File.Exists(fileName))
            {
                using (var sr = new StreamReader(fileName))
                {
                    code = sr.ReadToEnd();
                }
            }
            else
            {
                code = "//File does not exist: " + fileName;
            }
            return code;
        }
    }
}