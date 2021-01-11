using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Manager_API.Database
{
    public class DatabaseConnector
    {
        public string connectionString = "User Id=root;Host=127.0.0.1;Character Set = utf8; Database=event_management;Port=3306;Password=alo123123";
        IDbConnection dbConnection;

        public DatabaseConnector()
        {
            dbConnection = new MySqlConnection(connectionString);
        }

        //Method
        public IEnumerable<T> GetAllData<T>(string tableName)
        {
            string sql = $"Select * From {tableName}";
            var entities = dbConnection.Query<T>(sql).ToList();
            return entities;
        }

        public IEnumerable<T> GetManyDataByCommand<T>(string command)
        {
            return dbConnection.Query<T>(command).ToList();
        }

        public object GetOneDataByCommand<T>(string command)
        {
            return dbConnection.Query<T>(command).FirstOrDefault();
        }

        public int Insert<T>(T entity)
        {
            var tableName = typeof(T).Name;
            var storeName = $"Proc_Insert{tableName}";

            DynamicParameters param = new DynamicParameters();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                param.Add($"@{propertyName}", propertyValue);
            }
            var affects = dbConnection.Execute(storeName, param, commandType: CommandType.StoredProcedure);
            return affects;
        }



        public bool CheckExist<T1>(string property, object value)
        {
            var nameType = typeof(T1).Name;
            string sql = $"Select * From {nameType} where {property} = '{value}'";
            return dbConnection.Query<T1>(sql).Count() > 0;
        }

        public bool CheckExistUpdate<T1, T2>(string property, T2 value, string id)
        {
            var nameType = typeof(T1).Name;
            string sql = $"Select * From {nameType} where {property} = '{value}' and {nameType}Id != '{id}'";
            return dbConnection.Query<T1>(sql).Count() > 0;
        }

        public bool CheckLogin<T>(string user, string password)
        {//UserPassword và UserId
            var nameType = typeof(T).Name;
            string sql = $"Select * From {nameType} where {nameType}.UserId = '{user}' and {nameType}.UserPassword = '{password}'";
            return dbConnection.Query<T>(sql).Count() > 0;
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của đối tượng T có trường property mang giá trị value ( khác string) trong db
        /// </summary>
        /// <typeparam name="T1">Đối tượng cần kiểm tra</typeparam>
        /// <typeparam name="T2">Kiểu dữ liệu của thuộc tính cần kiểm tra</typeparam>
        /// <param name="property">Tên thuộc tính</param>
        /// <param name="value">Giá trị thuộc tính kiểu string</param>
        /// <returns>Trả về true nếu có tồn tại ít nhất 1 bản ghi thỏa mãn, false nếu không tồn tại bản ghi nào</returns>
        public bool CheckExistNotString<T1, T2>(string property, T2 value)
        {
            var nameType = typeof(T1).Name;
            var sql = $"Select * From {nameType} Where {property} = {value}";
            return dbConnection.Query<T1>(sql).Count() > 0;
        }

        /// <summary>
        /// Xóa 1 đối tượng theo Id
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng</typeparam>
        /// <param name="id">Id của đối tượng đó</param>
        /// <returns>số đối tượng xóa được</returns>


        public int DeleteById<T>(string id)
        {
            var tableName = typeof(T).Name;
            var sql = $"Delete From {tableName} Where {tableName}Id = '{id}'";
            return dbConnection.Execute(sql);
        }

        public int DeleteObj<T>(T value)
        {
            var tableName = typeof(T).Name;
            var sql = $"Delete From {tableName} Where {tableName}Id = '{value}'";
            return dbConnection.Execute(sql);
        }
        /// <summary>
        /// Cập nhật đối tượng
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng cần cập nhật</typeparam>
        /// <param name="entity">giá trị đối tượng cần cập nhật</param>
        /// <returns>số lượng đối tượng cập nhật thành công</returns>
        public int Update<T>(T entity)
        {
            var tableName = typeof(T).Name;
            var storeName = $"Proc_Update{tableName}";

            DynamicParameters param = new DynamicParameters();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                param.Add($"@{propertyName}", propertyValue);
            }
            return dbConnection.Execute(storeName, param, commandType: CommandType.StoredProcedure);
        }
    }
}
