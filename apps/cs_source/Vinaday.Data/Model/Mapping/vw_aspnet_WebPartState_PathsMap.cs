using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_WebPartState_PathsMap : EntityTypeConfiguration<vw_aspnet_WebPartState_Paths>
	{
		public vw_aspnet_WebPartState_PathsMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Paths), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType8<Guid, Guid, string, string>).GetMethod(".ctor", new Type[] { typeof(u003cApplicationIdu003ej__TPar), typeof(u003cPathIdu003ej__TPar), typeof(u003cPathu003ej__TPar), typeof(u003cLoweredPathu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType8<Guid, Guid, string, string>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_ApplicationId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_PathId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_Path").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_LoweredPath").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType8<Guid, Guid, string, string>).GetMethod("get_ApplicationId").MethodHandle, typeof(<>f__AnonymousType8<Guid, Guid, string, string>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType8<Guid, Guid, string, string>).GetMethod("get_PathId").MethodHandle, typeof(<>f__AnonymousType8<Guid, Guid, string, string>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType8<Guid, Guid, string, string>).GetMethod("get_Path").MethodHandle, typeof(<>f__AnonymousType8<Guid, Guid, string, string>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType8<Guid, Guid, string, string>).GetMethod("get_LoweredPath").MethodHandle, typeof(<>f__AnonymousType8<Guid, Guid, string, string>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Paths), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_WebPartState_Paths, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_Path").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Paths), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_WebPartState_Paths, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_LoweredPath").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			base.ToTable("vw_aspnet_WebPartState_Paths");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Paths), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_WebPartState_Paths, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_ApplicationId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("ApplicationId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Paths), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_WebPartState_Paths, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_PathId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("PathId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Paths), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_WebPartState_Paths, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_Path").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("Path");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_WebPartState_Paths), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_WebPartState_Paths, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_WebPartState_Paths).GetMethod("get_LoweredPath").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("LoweredPath");
		}
	}
}