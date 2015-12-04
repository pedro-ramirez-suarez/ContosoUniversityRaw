using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Needletail.DataAccess;
using ConUniv.ViewModels;

namespace ConUniv.Repositories
{
    //Add Bussiness logic here
    public class InstructorViewModelRepository : IDisposable, IDataSource<InstructorViewModel, Guid> 
    {
        DBTableDataSourceBase<InstructorViewModel, Guid> dataSource;

        string ConnectionString { get; set; }

        string tableName;
        private string TableName 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(tableName))
                { 
                }
                return tableName;
            }
            set 
            {
                tableName = value;
            }
        }

        public InstructorViewModelRepository()
        {
            this.TableName = typeof(InstructorViewModel).Name;
            this.ConnectionString = "Default";
            InitializeDataSource();
        }

        public InstructorViewModelRepository(string connectionString)
        {
            this.TableName = typeof(InstructorViewModel).Name;
            this.ConnectionString = connectionString;
            InitializeDataSource();
        }

        public InstructorViewModelRepository(string connectionString,string tableName)
        {
            this.ConnectionString = connectionString;
            this.TableName = tableName;
            InitializeDataSource();
        }

        private void InitializeDataSource()
        {
            dataSource = new DBTableDataSourceBase<InstructorViewModel, Guid>(this.ConnectionString, this.TableName);
        }

        public bool Delete(object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return this.dataSource.Delete(where: where, filterType: filterType);
        }

        public bool Delete(object where)
        {
            return this.dataSource.Delete(where: where);
        }

        public bool DeleteEntity(InstructorViewModel item)
        {
            return this.dataSource.DeleteEntity(item: item);
        }

        public IEnumerable<InstructorViewModel> GetAll()
        {
            return this.dataSource.GetAll();
        }

        public IEnumerable<InstructorViewModel> GetAll(object orderBy)
        {
            return this.dataSource.GetAll(orderBy: orderBy);
        }

        public IEnumerable<InstructorViewModel> GetMany(string select, string where, string orderBy)
        {
            return this.dataSource.GetMany(select: select, where: where, orderBy: orderBy);
        }

        public IEnumerable<InstructorViewModel> GetMany(object where)
        {
            return this.dataSource.GetMany(where: where);
        }

        public IEnumerable<InstructorViewModel> GetMany(object where, object orderBy)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy);
        }

        public IEnumerable<InstructorViewModel> GetMany(object where, Needletail.DataAccess.Engines.FilterType filterType, object orderBy, int? topN)
        {
            return this.dataSource.GetMany(where: where, filterType: filterType, orderBy: orderBy, topN: topN);
        }

        public IEnumerable<InstructorViewModel> GetMany(object where, object orderBy, Needletail.DataAccess.Engines.FilterType filterType, int page, int pageSize)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy, filterType: filterType, page: page, pageSize: pageSize);
        }

        public IEnumerable<InstructorViewModel> GetMany(string where, string orderBy, Dictionary<string, object> args, int page, int pageSize)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy, args: args, page: page, pageSize: pageSize);
        }

        public IEnumerable<InstructorViewModel> GetMany(string where, string orderBy, Dictionary<string, object> args, int? topN)
        {
            return this.dataSource.GetMany(where: where, orderBy: orderBy, args: args, topN: topN);
        }

        public IEnumerable<Needletail.DataAccess.Entities.DynamicEntity> Join(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            return this.dataSource.Join(selectColumns: selectColumns, joinQuery: joinQuery, whereQuery: whereQuery, orderBy: orderBy, args: args);
        }

        public IEnumerable<T> JoinGetTyped<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            return this.dataSource.JoinGetTyped<T>(selectColumns: selectColumns, joinQuery: joinQuery, whereQuery: whereQuery, orderBy: orderBy, args: args);
        }

        public InstructorViewModel GetSingle(object where)
        {
            return this.dataSource.GetSingle(where: where);
        }

        public InstructorViewModel GetSingle(object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return this.dataSource.GetSingle(where: where, filterType: filterType);
        }

        public InstructorViewModel GetSingle(string where, Dictionary<string, object> args)
        {
            return this.dataSource.GetSingle(where: where, args: args);
        }

        public Guid Insert(InstructorViewModel newItem)
        {
            return this.dataSource.Insert(newItem: newItem);
        }

        public bool Update(object item)
        {
            return this.dataSource.Update(item: item);
        }

        public bool UpdateWithWhere(object values, object where, Needletail.DataAccess.Engines.FilterType filterType)
        {
            return this.dataSource.UpdateWithWhere(values: values, where: where, filterType: filterType);
        }

        public bool UpdateWithWhere(object values, object where)
        {
            return this.dataSource.UpdateWithWhere(values: values, where: where);
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> args)
        {
            this.dataSource.ExecuteNonQuery(query: query, args: args);
        }

        public T ExecuteScalar<T>(string query, Dictionary<string, object> args)
        {
            return this.dataSource.ExecuteScalar<T>(query: query, args: args);
        }

        public void ExecuteStoredProcedure(string name, object parameters)
        {
            this.dataSource.ExecuteStoredProcedure(name, parameters);
        }

        public IEnumerable<T> ExecuteStoredProcedureReturnRows<T>(string name, object parameters)
        {
            return this.ExecuteStoredProcedureReturnRows<T>(name, parameters);
        }

        public void Dispose()
        {
            this.dataSource.Dispose();
        }
    }
}
