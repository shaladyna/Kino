using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application
{
   public interface ISaveLoad<T>
    {
        void Save(string path, T data);
        T Load(string path);
    }
}
