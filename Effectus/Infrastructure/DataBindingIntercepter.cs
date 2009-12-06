namespace Effectus.Infrastructure
{
	using System;
	using Castle.Core.Interceptor;
	using NHibernate;

	public class DataBindingIntercepter : EmptyInterceptor
	{
		public ISessionFactory SessionFactory { set; get; }

		public override object Instantiate(string clazz, EntityMode entityMode, object id)
		{
			if(entityMode == EntityMode.Poco)
			{
				Type type = Type.GetType(clazz);
				if (type != null)
				{
					var instance= DataBindingFactory.Create(type);
					SessionFactory.GetClassMetadata(clazz).SetIdentifier(instance,id, entityMode);
					return instance;
				}
			}
			return base.Instantiate(clazz, entityMode, id);
		}

		public override string GetEntityName(object entity)
		{
			var markerInterface = entity as DataBindingFactory.IMarkerInterface;
			if (markerInterface != null)
				return markerInterface.TypeName;
			return base.GetEntityName(entity);
		}
	}
}