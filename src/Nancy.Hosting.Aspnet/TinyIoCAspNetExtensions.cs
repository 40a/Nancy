﻿namespace Nancy.Hosting.Aspnet
{
    using System;
    using System.Web;
    using Nancy.TinyIoc;

    public class HttpContextLifetimeProvider : TinyIoCContainer.ITinyIoCObjectLifetimeProvider
    {
        private readonly string _KeyName = String.Format("TinyIoC.HttpContext.{0}", Guid.NewGuid());

        public object GetObject()
        {
            return HttpContext.Current.Items[_KeyName];
        }

        public void SetObject(object value)
        {
            HttpContext.Current.Items[_KeyName] = value;
        }

        public void ReleaseObject()
        {
            var item = GetObject() as IDisposable;

            if (item != null)
                item.Dispose();

            SetObject(null);
        }
    }

    public static class TinyIoCAspNetExtensions
    {
        public static TinyIoCContainer.RegisterOptions AsPerRequestSingleton(this TinyIoCContainer.RegisterOptions registerOptions)
        {
            return TinyIoCContainer.RegisterOptions.ToCustomLifetimeManager(registerOptions, new HttpContextLifetimeProvider(), "per request singleton");
        }

        public static TinyIoCContainer.MultiRegisterOptions AsPerRequestSingleton(this TinyIoCContainer.MultiRegisterOptions registerOptions)
        {
            return TinyIoCContainer.MultiRegisterOptions.ToCustomLifetimeManager(registerOptions, new HttpContextLifetimeProvider(), "per request singleton");
        }
    }
}
