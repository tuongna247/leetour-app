using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_ProfilesMap : EntityTypeConfiguration<vw_aspnet_Profiles>
	{
		public vw_aspnet_ProfilesMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(vw_aspnet_Profiles), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType4<Guid, DateTime>).GetMethod(".ctor", new Type[] { typeof(u003cUserIdu003ej__TPar), typeof(u003cLastUpdatedDateu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType4<Guid, DateTime>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Profiles).GetMethod("get_UserId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Profiles).GetMethod("get_LastUpdatedDate").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType4<Guid, DateTime>).GetMethod("get_UserId").MethodHandle, typeof(<>f__AnonymousType4<Guid, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType4<Guid, DateTime>).GetMethod("get_LastUpdatedDate").MethodHandle, typeof(<>f__AnonymousType4<Guid, DateTime>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			base.ToTable("vw_aspnet_Profiles");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Profiles), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_Profiles, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Profiles).GetMethod("get_UserId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("UserId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Profiles), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Profiles, DateTime>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Profiles).GetMethod("get_LastUpdatedDate").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("LastUpdatedDate");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Profiles), "t");
			base.Property<int>(Expression.Lambda<Func<vw_aspnet_Profiles, int?>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Profiles).GetMethod("get_DataSize").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("DataSize");
		}
	}
}