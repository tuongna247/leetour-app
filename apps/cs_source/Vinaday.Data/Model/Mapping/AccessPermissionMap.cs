using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class AccessPermissionMap : EntityTypeConfiguration<AccessPermission>
	{
		public AccessPermissionMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).GetMethod(".ctor", new Type[] { typeof(u003cIdu003ej__TPar), typeof(u003cEntityNameu003ej__TPar), typeof(u003cEntityIdu003ej__TPar), typeof(u003cTypeu003ej__TPar), typeof(u003cCreatedDateu003ej__TPar), typeof(u003cModifiedDateu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_Id").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_EntityName").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_EntityId").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_Type").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_CreatedDate").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_ModifiedDate").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).GetMethod("get_Id").MethodHandle, typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).GetMethod("get_EntityName").MethodHandle, typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).GetMethod("get_EntityId").MethodHandle, typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).GetMethod("get_Type").MethodHandle, typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).GetMethod("get_CreatedDate").MethodHandle, typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).GetMethod("get_ModifiedDate").MethodHandle, typeof(<>f__AnonymousType0<int, string, int, int, DateTime, DateTime>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property<int>(Expression.Lambda<Func<AccessPermission, int>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_Id").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity));
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property(Expression.Lambda<Func<AccessPermission, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_EntityName").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(200));
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property<int>(Expression.Lambda<Func<AccessPermission, int>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_EntityId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.None));
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property<int>(Expression.Lambda<Func<AccessPermission, int>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_Type").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.None));
			base.ToTable("AccessPermissions");
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property<int>(Expression.Lambda<Func<AccessPermission, int>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_Id").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("Id");
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property(Expression.Lambda<Func<AccessPermission, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_EntityName").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("EntityName");
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property<int>(Expression.Lambda<Func<AccessPermission, int>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_EntityId").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("EntityId");
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property<int>(Expression.Lambda<Func<AccessPermission, int>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_Type").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("Type");
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property(Expression.Lambda<Func<AccessPermission, DateTime>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_CreatedDate").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("CreatedDate");
			parameterExpression = Expression.Parameter(typeof(AccessPermission), "t");
			base.Property(Expression.Lambda<Func<AccessPermission, DateTime>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AccessPermission).GetMethod("get_ModifiedDate").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("ModifiedDate");
		}
	}
}