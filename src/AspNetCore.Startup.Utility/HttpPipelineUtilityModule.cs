using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility
{
    public class HttpPipelineUtilityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<>().As<ICharacterRepository>();
        }
    }
}
