// -----------------------------------------------------------------------
//  <copyright file="MvcIocResolver.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-06 15:27</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using OSharp.Web.WebForm.Initialize;

using OSharp.Core.Dependency;
using Autofac.Integration.Web;
using Autofac;


namespace OSharp.Web.WebForm.Initialize
{
    /// <summary>
    /// MVC依赖注入对象解析器
    /// </summary>
    public class WebFormIocResolver : IIocResolver
    {
        static IContainerProvider _containerProvider;
        public static IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
            set { _containerProvider = value; }
        }

        /// <summary>
        /// 从全局容器中解析对象委托
        /// </summary>
        public static Func<Type, object> GlobalResolveFunc { private get; set; }

        /// <summary>
        /// 获取指定类型的实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            try
            {
                return ContainerProvider.RequestLifetime.Resolve<T>();
            }
            catch (Exception)
            {
                if (GlobalResolveFunc != null)
                {
                    return (T)GlobalResolveFunc(typeof(T));
                }
                return default(T);
            }
        }

        /// <summary>
        /// 获取指定类型的实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            try
            {
                return ContainerProvider.RequestLifetime.Resolve(type);
            }
            catch (Exception)
            {
                if (GlobalResolveFunc != null)
                {
                    return GlobalResolveFunc(type);
                }
                return null;
            }
            
        }

        /// <summary>
        /// 获取指定类型的所有实例
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public IEnumerable<T> Resolves<T>()
        {
            return ContainerProvider.RequestLifetime.ResolveOptional<IEnumerable<T>>();
        }

        /// <summary>
        /// 获取指定类型的所有实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public IEnumerable<object> Resolves(Type type)
        {
            Type typeToResolve = typeof(IEnumerable<>).MakeGenericType(type);
            Array array = ContainerProvider.RequestLifetime.ResolveOptional(typeToResolve) as Array;
            if (array != null)
            {
                return array.Cast<object>();
            }
            return new object[0];
        }
    }
}