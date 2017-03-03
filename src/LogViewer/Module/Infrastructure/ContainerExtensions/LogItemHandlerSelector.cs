using System;
using System.Collections;
using System.Reflection;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using LogViewer.Library.Module.Services;

namespace LogViewer.Module.Infrastructure.ContainerExtensions
{
   public class LogItemHandlerSelector: DefaultTypedFactoryComponentSelector
    {
        protected override Func<IKernelInternal, IReleasePolicy, object> BuildFactoryComponent(MethodInfo method, string componentName, Type componentType, IDictionary additionalArguments)
        {            
            return (kernel, rp) => kernel.ResolveAll(componentType);            
        }

        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            return null;
        }

        protected override Type GetComponentType(MethodInfo method, object[] arguments)
        {            
            var message = arguments[0];            
            var handlerType = typeof(ILogItemHandler<>).MakeGenericType(message.GetType());

            //var handlerType2 = new TypedFactoryCoponentCollection(handlerType.MakeArrayType(), new Arguments(arguments));
            return handlerType;
        }
    }
}
