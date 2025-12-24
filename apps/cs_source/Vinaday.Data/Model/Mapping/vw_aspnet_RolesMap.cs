using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_RolesMap : EntityTypeConfiguration<vw_aspnet_Roles>
	{
		public vw_aspnet_RolesMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType5<Guid, Guid, string, string>).GetMethod(".ctor", new Type[] { typeof(u003cApplicationIdu003ej__TPar), typeof(u003cRoleIdu003ej__TPar), typeof(u003cRoleNameu003ej__TPar), typeof(u003cLoweredRoleNameu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType5<Guid, Guid, string, string>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_ApplicationId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_RoleId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_RoleName").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_LoweredRoleName").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType5<Guid, Guid, string, string>).GetMethod("get_ApplicationId").MethodHandle, typeof(<>f__AnonymousType5<Guid, Guid, string, string>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType5<Guid, Guid, string, string>).GetMethod("get_RoleId").MethodHandle, typeof(<>f__AnonymousType5<Guid, Guid, string, string>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType5<Guid, Guid, string, string>).GetMethod("get_RoleName").MethodHandle, typeof(<>f__AnonymousType5<Guid, Guid, string, string>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType5<Guid, Guid, string, string>).GetMethod("get_LoweredRoleName").MethodHandle, typeof(<>f__AnonymousType5<Guid, Guid, string, string>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Roles, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_RoleName").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Roles, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_LoweredRoleName").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Roles, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_Description").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasMaxLength(new int?(256));
			base.ToTable("vw_aspnet_Roles");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_Roles, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_ApplicationId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("ApplicationId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_Roles, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_RoleId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("RoleId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Roles, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_RoleName").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("RoleName");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Roles, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_LoweredRoleName").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("LoweredRoleName");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Roles), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Roles, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Roles).GetMethod("get_Description").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("Description");
		}
	}
}