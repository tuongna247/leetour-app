using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_WebPartState_SharedMap : EntityTypeConfiguration<vw_aspnet_WebPartState_Shared>
	{
		public vw_aspnet_WebPartState_SharedMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Shared), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType9<Guid, DateTime>).GetMethod(".ctor", new Type[] { typeof(u003cPathIdu003ej__TPar), typeof(u003cLastUpdatedDateu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType9<Guid, DateTime>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Shared).GetMethod("get_PathId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Shared).GetMethod("get_LastUpdatedDate").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType9<Guid, DateTime>).GetMethod("get_PathId").MethodHandle, typeof(<>f__AnonymousType9<Guid, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType9<Guid, DateTime>).GetMethod("get_LastUpdatedDate").MethodHandle, typeof(<>f__AnonymousType9<Guid, DateTime>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			base.ToTable("vw_aspnet_WebPartState_Shared");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Shared), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_WebPartState_Shared, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Shared).GetMethod("get_PathId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("PathId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Shared), "t");
			base.Property<int>(Expression.Lambda<Func<vw_aspnet_WebPartState_Shared, int?>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Shared).GetMethod("get_DataSize").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("DataSize");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Shared), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_WebPartState_Shared, DateTime>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Shared).GetMethod("get_LastUpdatedDate").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("LastUpdatedDate");
		}
	}
}