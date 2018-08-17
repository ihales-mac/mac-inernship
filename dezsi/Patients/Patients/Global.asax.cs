using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Patients.App_Start;
using Patients.Controllers;
using Patients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Patients
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Autofac.IContainer container = AutofacConfig.BuildContainer();
            container.Resolve<PatientsController>();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver =
 new AutofacWebApiDependencyResolver(container);

            


        }
    }

}