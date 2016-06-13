using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using OSharp.Core.Caching;
using OSharp.Core.Dependency;
using OSharp.Logging.Log4Net;
using OSharp.Data.Entity;
using OSharp.AutoMapper;
using OSharp.Demo;
using OSharp.Demo.Services;
using OSharp.Core.Security;
using OSharp.Autofac.WebForm;
using OSharp.Web.WebForm.Initialize;

[assembly: OwinStartup(typeof(OSharp.Web.Demo.Startup))]

namespace OSharp.Web.Demo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ICacheProvider provider = new RuntimeMemoryCacheProvider();
            CacheManager.SetProvider(provider, CacheLevel.First);

            IServicesBuilder builder = new ServicesBuilder();
            IServiceCollection services = builder.Build();
            services.AddLog4NetServices();
            services.AddDataServices();
            services.AddAutoMapperServices();
            services.AddOAuthServices();

            services.AddDemoServices(app);

            IIocBuilder IocBuilder = new WebFormAutofacIocBuilder(services);
            app.UseOsharpWebForm(IocBuilder);

            //IIocBuilder mvcIocBuilder = new MvcAutofacIocBuilder(services);
            //app.UseOsharpMvc(mvcIocBuilder);
            //IIocBuilder apiIocBuilder = new WebApiAutofacIocBuilder(services);
            //app.UseOsharpWebApi(apiIocBuilder);
            //app.UseOsharpSignalR(new SignalRAutofacIocBuilder(services));

            //app.ConfigureOAuth(apiIocBuilder.ServiceProvider);
            //app.ConfigureWebApi();
            //app.ConfigureSignalR();
        }
    }
}
