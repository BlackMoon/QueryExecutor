using System;
using System.Data;
using System.Data.Entity;

namespace queryExecutor.DbManager
{
    public interface IDbManager : IDisposable
    {
        string ConnectionString { get; set; }

        IDbConnection DbConnection { get; }

        DbContext DbContext { get; }
      
        IDbTransaction Transaction { get; }
        IDataReader DataReader { get; }
        IDbCommand DbCommand { get; }
        IDbDataParameter[] DbParameters { get; }
        void AddParameter(IDbDataParameter dataParameter);
        IDbDataParameter AddParameter(string name, object value);
        IDbDataParameter AddParameter(string name, object value, ParameterDirection direction);
        IDbDataParameter AddParameter(string name, object value, ParameterDirection direction, int size);
        void Open();
        void Open(string connectionString);
        void OpenWithNewPassword(string newPassword);
        void BeginTransaction();
        void CommitTransaction();
        IDataReader ExecuteReader(CommandType commandType, string commandText);
        DataSet ExecuteDataSet(CommandType commandType, string commandText);
        object ExecuteScalar(CommandType commandType, string commandText);
        int ExecuteNonQuery(CommandType commandType, string commandText);
        void CloseReader();
        void Close();
    }
}
