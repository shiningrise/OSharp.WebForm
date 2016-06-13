// -----------------------------------------------------------------------
//  <copyright file="MvcFunctionHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-09 16:17</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using OSharp.Web.WebForm.Initialize;
using System.Web;

using OSharp.Core.Security;
using OSharp.Utility;
using OSharp.Utility.Extensions;
using OSharp.Web.WebForm.Properties;
using System.IO;
using System.Diagnostics;

namespace OSharp.Web.WebForm.Initialize
{
    /// <summary>
    /// MVC功能信息处理器
    /// </summary>
    public class WebFormFunctionHandler : FunctionHandlerBase<Function, Guid>
    {
        /// <summary>
        /// 获取 功能技术提供者，如Mvc/WebApi/SignalR等，用于区分功能来源，各技术更新功能时，只更新属于自己技术的功能
        /// </summary>
        protected override PlatformToken PlatformToken
        {
            get { return PlatformToken.Local; }
        }

        public WebFormFunctionHandler()
        {
            TypeFinder = new NullFunctionTypeFinder();
            MethodInfoFinder = new NullFunctionMethodInfoFinder();
        }

        #region WebForm用不到

        /// <summary>
        /// 重写以实现从类型信息创建功能信息
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <returns></returns>
        protected override Function GetFunction(Type type)
        {
            return null;
        }

        /// <summary>
        /// 重写以实现从方法信息创建功能信息
        /// </summary>
        /// <param name="method">功能信息</param>
        /// <returns></returns>
        protected override Function GetFunction(MethodInfo method)
        {
            return null;
        }

        
        #endregion

        /// <summary>
        /// 获取控制器类型所在的区域名称，无区域返回null
        /// </summary>
        protected override string GetArea(Type type)
        {
            return "WebForm";
        }
        
        /// <summary>
        /// 重写以实现是否忽略指定方法的功能信息
        /// </summary>
        /// <param name="action">要判断的功能信息</param>
        /// <param name="method">功能相关的方法信息</param>
        /// <param name="functions">已存在的功能信息集合</param>
        /// <returns></returns>
        protected override bool IsIgnoreMethod(Function action, MethodInfo method, IEnumerable<Function> functions)
        {
            return false;
            //bool flag = base.IsIgnoreMethod(action, method, functions);
            //return flag && method.HasAttribute<HttpPostAttribute>();
        }

        protected override Function[] GetFunctions(Type[] types)
        {
            var list = new List<Function>();
            var rootPath = GetRootURI();
            var directories = System.IO.Directory.GetDirectories(rootPath);
            foreach (var item in directories)
            {
                Find(list,Path.Combine(rootPath,item),1);
            }
            return list.ToArray();
        }

        private void Find(List<Function> list, string path,int level)
        {
            if (level == 1)
            {
                var files = System.IO.Directory.GetFiles(path, "*.aspx");
                foreach (var file in files)
                {
                    var array = path.Split('\\');
                    list.Add(new Function()
                    {
                        Name = Path.GetFileNameWithoutExtension(file),
                        Action = Path.GetFileNameWithoutExtension(file),
                        Controller = path.Substring(path.LastIndexOf('\\') + 1),
                        //Area = "",
                        FunctionType = FunctionType.Anonymouse,
                        PlatformToken = PlatformToken,
                        IsController = false,
                        IsAjax = false,
                        IsChild = false
                    });
                }
            }
            if (level == 2)
            {
                var files = System.IO.Directory.GetFiles(path, "*.aspx");
                foreach (var file in files)
                {
                    var array = path.Split('\\');
                    list.Add(new Function()
                    {
                        Name = Path.GetFileNameWithoutExtension(file),
                        Action = Path.GetFileNameWithoutExtension(file),
                        Controller = path.Substring(path.LastIndexOf('\\') + 1),
                        Area = array[array.Length - 1],
                        FunctionType = FunctionType.Anonymouse,
                        PlatformToken = PlatformToken,
                        IsController = false,
                        IsAjax = false,
                        IsChild = false
                    });
                }
            }
            var directories = System.IO.Directory.GetDirectories(path);
            foreach (var item in directories)
            {
                Find(list, Path.Combine(path, item), 2);
            }
        }

        /// <summary>
        /// 取得网站的根目录的URL
        /// </summary>
        /// <returns></returns>
        private string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;
                Req.CheckNotNull("WebFormFunctionHandler.GetRootURI 有误");
                AppPath = Req.MapPath("~/");
                Debug.Write(AppPath);
                return AppPath;
            }
            throw new InvalidProgramException("WebFormFunctionHandler.GetRootURI 有误");
        }
    
    }
}