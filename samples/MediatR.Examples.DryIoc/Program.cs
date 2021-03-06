using System;
using System.Reflection;
using System.Threading.Tasks;
using DryIoc;
using MediatR.Pipeline;

namespace MediatR.Examples.DryIoc
{
    class Program
    {
        static Task Main()
        {
            var mediator = BuildMediator();

            return Runner.Run(mediator, Console.Out, "DryIoc");
        }

        private static IMediator BuildMediator()
        {
            var container = new Container();

            container.RegisterDelegate<SingleInstanceFactory>(r => serviceType => r.Resolve(serviceType));
            container.RegisterDelegate<MultiInstanceFactory>(r => serviceType => r.ResolveMany(serviceType));
            container.RegisterInstance(Console.Out);

            //Pipeline works out of the box here

            container.RegisterMany(new[] { typeof(IMediator).GetAssembly(), typeof(Ping).GetAssembly() }, type => type.GetTypeInfo().IsInterface); 

            return container.Resolve<IMediator>();
        }
    }
}
