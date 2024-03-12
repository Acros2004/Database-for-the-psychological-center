using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Interfaces
{
    public interface IRepositoryString<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(string value);
        void Create(T item);
        void Update(T item);
        void Delete(string id);

    }
}
