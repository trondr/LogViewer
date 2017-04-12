using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Library.Module.Services
{
    public interface ISourceCodeInfoProvider
    {
        SourceCodeInfo GetSourceCode(string fileName);
    }
}
