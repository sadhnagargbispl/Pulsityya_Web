using Shopinv.Interface;
using Shopinv.Repoistory;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Aarogyabhumi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<I_Banner, R_Banner>();
            container.RegisterType<I_Category, R_Category>();
            container.RegisterType<I_Login, R_Login>();
            container.RegisterType<I_OrderReport, R_OrderReport>();
            container.RegisterType<I_PayMode, R_PayMode>();
            container.RegisterType<I_Product, R_Product>();
            container.RegisterType<I_register, R_register>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}