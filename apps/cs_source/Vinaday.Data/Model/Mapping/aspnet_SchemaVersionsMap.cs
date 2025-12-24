using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Vinaday.Data.Models;

namespace Vinaday.Data.Models.Mapping
{
	public class aspnet_SchemaVersionsMap : EntityTypeConfiguration<aspnet_SchemaVersions>
	{
		public aspnet_SchemaVersionsMap()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(aspnet_SchemaVersions), "t");
			ConstructorInfo methodFromHandle = (ConstructorInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType1<string, string>).GetMethod(".ctor", new Type[] { typeof(u003cFeatureu003ej__TPar), typeof(u003cCompatibleSchemaVersionu003ej__TPar) }).MethodHandle, typeof(<>f__AnonymousType1<string, string>).TypeHandle);
			Expression[] expressionArray = new Expression[] { Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(aspnet_SchemaVersions).GetMethod("get_Feature").MethodHandle)), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(aspnet_SchemaVersions).GetMethod("get_CompatibleSchemaVersion").MethodHandle)) };
			MemberInfo[] memberInfoArray = new MemberInfo[] { (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType1<string, string>).GetMethod("get_Feature").MethodHandle, typeof(<>f__AnonymousType1<string, string>).TypeHandle), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(<>f__AnonymousType1<string, string>).GetMethod("get_CompatibleSchemaVersion").MethodHandle, typeof(<>f__AnonymousType1<string, string>).TypeHandle) };
			base.HasKey(Expression.Lambda(Expression.New(methodFromHandle, (IEnumerable<Expression>)expressionArray, memberInfoArray), new ParameterExpression[] { parameterExpression }));
			parameterExpression = Expression.Parameter(typeof(aspnet_SchemaVersions), "t");
			base.Property(Expression.Lambda<Func<aspnet_SchemaVersions, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(aspnet_SchemaVersions).GetMethod("get_Feature").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(128));
			parameterExpression = Expression.Parameter(typeof(aspnet_SchemaVersions), "t");
			base.Property(Expression.Lambda<Func<aspnet_SchemaVersions, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(aspnet_SchemaVersions).GetMethod("get_CompatibleSchemaVersion").MethodHandle)), new ParameterExpression[] { parameterExpression })).IsRequired().HasMaxLength(new int?(128));
			base.ToTable("aspnet_SchemaVersions");
			parameterExpression = Expression.Parameter(typeof(aspnet_SchemaVersions), "t");
			base.Property(Expression.Lambda<Func<aspnet_SchemaVersions, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(aspnet_SchemaVersions).GetMethod("get_Feature").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("Feature");
			parameterExpression = Expression.Parameter(typeof(aspnet_SchemaVersions), "t");
			base.Property(Expression.Lambda<Func<aspnet_SchemaVersions, string>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(aspnet_SchemaVersions).GetMethod("get_CompatibleSchemaVersion").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("CompatibleSchemaVersion");
			parameterExpression = Expression.Parameter(typeof(aspnet_SchemaVersions), "t");
			base.Property<bool>(Expression.Lambda<Func<aspnet_SchemaVersions, bool>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(aspnet_SchemaVersions).GetMethod("get_IsCurrentVersion").MethodHandle)), new ParameterExpression[] { parameterExpression })).HasColumnName("IsCurrentVersion");
		}
	}
}