using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AS.Dapper.Base
{
    public class DapperTransaction : IDisposable
    {
        public IDbConnection _dbConnection { get; set; }
        public IDbTransaction _dbTransaction { get; set; }

        public DapperTransaction(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _dbTransaction = _dbConnection.BeginTransaction();
        }
        public void Dispose()
        {
            //_dbConnection.Dispose();
            _dbTransaction.Dispose();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
            // _dbConnection.Dispose();
        }
        public void RollBack()
        {
            _dbTransaction.Rollback();
            _dbConnection.Close();
        }
    }
}
