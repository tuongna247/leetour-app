using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_UsersInRolesMap : EntityTypeConfiguration<vw_aspnet_UsersInRoles>
	{
		public vw_aspnet_UsersInRolesMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(vw_aspnet_UsersInRoles), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType6<Guid, Guid>).GetMethod(".ctor", new Type[] { typeof(u003cUserIdu003ej__TPar), typeof(u003cRoleIdu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType6<Guid, Guid>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_UsersInRoles).GetMethod("get_UserId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_UsersInRoles).GetMethod("get_RoleId").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType6<Guid, Guid>).GetMethod("get_UserId").MethodHandle, typeof(<>f__AnonymousType6<Guid, Guid>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType6<Guid, Guid>).GetMethod("get_RoleId").MethodHandle, typeof(<>f__AnonymousType6<Guid, Guid>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			base.ToTable("vw_aspnet_UsersInRoles");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_UsersInRoles), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_UsersInRoles, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_UsersInRoles).GetMethod("get_UserId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("UserId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_UsersInRoles), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_UsersInRoles, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_UsersInRoles).GetMethod("get_RoleId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("RoleId");
		}
	}
}