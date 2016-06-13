using OSharp.Core.Dependency;
using OSharp.Data.Entity;
using OSharp.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OSharp.Utility.Extensions;
using OSharp.Core.Data;
using OSharp.Demo.Models.Identity;
using Microsoft.AspNet.Identity;
using OSharp.Demo.Identity;

namespace OSharp.Web.Demo
{
    public partial class Default : System.Web.UI.Page
    {
        protected readonly ILogger Logger;
        public Default()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        public IServiceProvider ServiceProvider { get; set; }

        public IDbContextTypeResolver ContextTypeResolver { get; set; }


        public IIocResolver IocResolver { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            const string format = "{0}: {1}";
            List<string> lines = new List<string>()
            {
                format.FormatWith("ServiceProvider", ServiceProvider.GetHashCode()),
                format.FormatWith("DefaultDbContext", ServiceProvider.GetService<DefaultDbContext>().GetHashCode()),
                format.FormatWith("DefaultDbContext", ServiceProvider.GetService<DefaultDbContext>().GetHashCode()),
                format.FormatWith("IRepository<User,int>", ServiceProvider.GetService<IRepository<User,int>>().GetHashCode()),
                format.FormatWith("IRepository<User,int>", ServiceProvider.GetService<IRepository<User,int>>().GetHashCode()),
                format.FormatWith("UserManager", ServiceProvider.GetService<UserManager<User,int>>().GetHashCode()),
                format.FormatWith("UserManager", ServiceProvider.GetService<UserManager>().GetHashCode()),
            };
            Response.Write(lines.ExpandAndToString("<br>"));
        }
    }
}