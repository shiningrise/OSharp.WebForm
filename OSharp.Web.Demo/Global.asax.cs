using Autofac.Integration.Web;
using OSharp.Web.WebForm.Initialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace OSharp.Web.Demo
{
    public class Global : System.Web.HttpApplication, IContainerProviderAccessor
    {
        public IContainerProvider ContainerProvider
        {
            get
            {
                return WebFormIocResolver.ContainerProvider;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
        }
    }
}