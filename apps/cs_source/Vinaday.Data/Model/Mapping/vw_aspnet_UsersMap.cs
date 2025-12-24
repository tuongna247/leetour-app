using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class vw_aspnet_UsersMap : EntityTypeConfiguration<vw_aspnet_Users>
	{
		public vw_aspnet_UsersMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).GetMethod(".ctor", new Type[] { typeof(u003cApplicationIdu003ej__TPar), typeof(u003cUserIdu003ej__TPar), typeof(u003cUserNameu003ej__TPar), typeof(u003cLoweredUserNameu003ej__TPar), typeof(u003cIsAnonymousu003ej__TPar), typeof(u003cLastActivityDateu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_ApplicationId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_UserId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_UserName").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_LoweredUserName").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_IsAnonymous").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_LastActivityDate").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).GetMethod("get_ApplicationId").MethodHandle, typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).GetMethod("get_UserId").MethodHandle, typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).GetMethod("get_UserName").MethodHandle, typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).GetMethod("get_LoweredUserName").MethodHandle, typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).GetMethod("get_IsAnonymous").MethodHandle, typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).GetMethod("get_LastActivityDate").MethodHandle, typeof(<>f__AnonymousType7<Guid, Guid, string, string, bool, DateTime>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Users, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_UserName").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Users, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_LoweredUserName").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(256));
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Users, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_MobileAlias").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasMaxLength(new int?(16));
			base.ToTable("vw_aspnet_Users");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_Users, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_ApplicationId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("ApplicationId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property<Guid>(Expression.Lambda<Func<vw_aspnet_Users, Guid>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_UserId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("UserId");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Users, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_UserName").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("UserName");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Users, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_LoweredUserName").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("LoweredUserName");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Users, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_MobileAlias").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("MobileAlias");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property<bool>(Expression.Lambda<Func<vw_aspnet_Users, bool>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_IsAnonymous").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("IsAnonymous");
			parameterExpression = Expression.Parameter(typeof(vw_aspnet_Users), "t");
			base.Property(Expression.Lambda<Func<vw_aspnet_Users, DateTime>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(vw_aspnet_Users).GetMethod("get_LastActivityDate").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("LastActivityDate");
		}
	}
}