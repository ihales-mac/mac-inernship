using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Patients.Controllers;
using Patients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Patients.App_Start
{
    public class AutofacConfig
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<PatientService>().As<IPatientService>().InstancePerLifetimeScope();
 
      


            /*
      
            .WithParameter(new ResolvedParameter(
            (pi, ctx) => pi.ParameterType == typeof(IPatientService),
            (pi, ctx) => ctx.ResolveKeyed<IPatientService>(pi.Name))).InstancePerLifetimeScope()
            
             */
            ;
            return builder.Build();
        }
    }
}