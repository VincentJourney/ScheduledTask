using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Model
{
    public class ResponseModel<T>
    {
        public ResponseModel()
        {
            Result = new Result();
        }
        public Result Result = new Result();
        public T Data;
    }
    public class Result
    {

        public Result()
        {
            HasError = true;
            ErrorCode = "";
            ErrorMessage = "";
        }
        public bool HasError; //是否有错误
        public string ErrorCode; //错误代码
        public string ErrorMessage; //错误信息
    }
}
