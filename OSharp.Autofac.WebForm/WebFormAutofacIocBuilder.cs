// -----------------------------------------------------------------------
//  <copyright file="WebFormAutofacIocBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-12 15:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Autofac;
using Autofac.Integration.Web;

using OSharp.Core.Dependency;
using OSharp.Core.Security;
using OSharp.Web.WebForm.Initialize;

namespace OSharp.Autofac.WebForm
{
    /// <summary>
    /// Mvc-Autofac依赖注入初始化类
    /// </summary>
    public class WebFormAutofacIocBuilder : IocBuilderBase
    {
        /// <summary>
        /// 初始化一个<see cref="WebFormAutofacIocBuilder"/>类型的新实例
        /// </summary>
        /// <param name="services">服务信息集合</param>
        public WebFormAutofacIocBuilder(IServiceCollection services)
            : base(services)
        { }

        /// <summary>
        /// 添加自定义服务映射
        /// </summary>
        /// <param name="services">服务信息集合</param>
        protected override void AddCustomTypes(IServiceCollection services)
        {
            services.AddInstance(this);
            services.AddSingleton<IIocResolver, WebFormIocResolver>();
            services.AddSingleton<IFunctionHandler, WebFormFunctionHandler>();
        }

        /// <summary>
        /// 构建服务并设置MVC平台的Resolver
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        /// <param name="assemblies">程序集集合</param>
        /// <returns>服务提供者</returns>
        protected override IServiceProvider BuildAndSetResolver(IServiceCollection services, Assembly[] assemblies)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
            IContainer container = builder.Build();
            
            
            var resolver = new ContainerProvider(container);
            WebFormIocResolver.ContainerProvider = resolver;
            //WebFormIocResolver.GlobalResolveFunc = t => resolver.ApplicationContainer.Resolve(t);
            return resolver.RequestLifetime.Resolve<IServiceProvider>();
        }
    }
}