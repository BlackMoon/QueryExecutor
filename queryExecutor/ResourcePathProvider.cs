using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace queryExecutor
{
    internal class ResourceFile : VirtualFile
    {
        private readonly string _resourceName;

        private Assembly _resourceAssembly;

        /// <summary>
        /// Resource assembly
        /// </summary>
        private Assembly ResourceAssembly => _resourceAssembly ?? (_resourceAssembly = Assembly.GetExecutingAssembly());

        public ResourceFile(string virtualPath, Assembly resourceAssembly, string resourceName) : base(virtualPath)
        {
            _resourceAssembly = resourceAssembly;
            _resourceName = resourceName;
        }

        /// <summary>
        /// Override Open method to load resource files of assembly.
        /// </summary>
        /// <returns></returns>
        public override Stream Open()
        {
            return ResourceAssembly.GetManifestResourceStream(_resourceName) ?? new MemoryStream();
        }
    }

    public class ResourcePathProvider : VirtualPathProvider
    {
        private Assembly _resourceAssembly;

        /// <summary>
        /// Resource assembly
        /// </summary>
        private Assembly ResourceAssembly => _resourceAssembly ?? (_resourceAssembly = Assembly.GetExecutingAssembly());

        public ResourcePathProvider(Assembly resourceAssembly)
        {
            _resourceAssembly = resourceAssembly;
        }

        private string ToNamespace(string virtualPath)
        {
            string name = VirtualPathUtility.ToAbsolute(virtualPath);
            if (name[name.Length - 1] == '/')
                name = name.Substring(0, name.Length - 1);

            return ResourceAssembly.GetName().Name + name.Replace("/", ".");
        }

        /// <summary>
        /// Make a judgment that application find path contains specifical resource name.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        private string GetResourceName(string virtualPath)
        {
            string name = ToNamespace(virtualPath);

            string[] resources = ResourceAssembly.GetManifestResourceNames();
            return resources.SingleOrDefault(r => name.Equals(r, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// If we can find this virtual path, return true.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public override bool FileExists(string virtualPath)
        {
            string resourceName = GetResourceName(virtualPath);
            return !string.IsNullOrEmpty(resourceName) || base.FileExists(virtualPath);
        }

        /// <summary>
        /// Use custom VirtualFile class to load assembly resources.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            string resourceName = GetResourceName(virtualPath);
            if (!string.IsNullOrEmpty(resourceName))
                return new ResourceFile(virtualPath, ResourceAssembly, resourceName);

            return base.GetFile(virtualPath);
        }

        /// <summary>
        /// Return null when application use virtual file path.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="virtualPathDependencies"></param>
        /// <param name="utcStart"></param>
        /// <returns></returns>
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            string resourceName = GetResourceName(virtualPath);
            if (!string.IsNullOrEmpty(resourceName))
                return null;

            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
    }
}