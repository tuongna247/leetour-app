using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_ApplicationsMap : EntityTypeConfiguration<vw_aspnet_Applications>
	{
		public vw_aspnet_ApplicationsMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType2<string, string, Guid>).GetMethod(".ctor", new Type[] { typeof(u003cApplicationNameu003ej__TPar), typeof(u003cLoweredApplicationNameu003ej__TPar), typeof(u003cApplicationIdu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType2<string, string, Guid>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_ApplicationName").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_LoweredApplicationName").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_ApplicationId").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType2<string, string, Guid>).GetMethod("get_ApplicationName").MethodHandle, typeof(<>f__AnonymousType2<string, string, Guid>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType2<string, string, Guid>).GetMethod("get_LoweredApplicationName").MethodHandle, typeof(<>f__AnonymousType2<string, string, Guid>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType2<string, string, Guid>).GetMethod("get_ApplicationId").MethodHandle, typeof(<>f__AnonymousType2<string, string, Guid>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Applications, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_ApplicationName").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Applications, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_LoweredApplicationName").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Applications, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_Description").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasMaxLength(new int?(256));
			base.ToTable("vw_aspnet_Applications");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Applications, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_ApplicationName").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("ApplicationName");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Applications, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_LoweredApplicationName").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("LoweredApplicationName");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_Applications, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_ApplicationId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("ApplicationId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Applications), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Applications, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Applications).GetMethod("get_Description").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("Description");
		}
	}
}