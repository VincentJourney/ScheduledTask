using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Model
{
    public class RequestModel<T> where T : new()
    {
        public RequestModel()
        {
            Shared = new Shared();
            Data = new T();
        }
        public Shared Shared = new Shared();
        public T Data;
    }

    public class Shared
    {
        public string AppCode; //调用的终端名  POS 或者 WECHAT
        public string CorpCode;// 公司
        public string OrgCode; //机构

    }
}
