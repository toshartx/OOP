using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR5.Import
{
    interface IFileReadStrategy
    {
        bool CanHandleThis(string path);
        void Read(string path);
    }

    class JsonReadStrategy : IFileReadStrategy
    {
        public bool CanHandleThis(string path)
        {
            throw new NotImplementedException();
        }

        public void Read(string path)
        {
            throw new NotImplementedException();
        }
    }
}
