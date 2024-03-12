using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Repository
{
    public class ClientRepository : IRepository<Client>
    {
        private Psychological_CenterEntities db;

        public ClientRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Client> GetAll()
        {
            return db.Clients;
        }
        public Client Get(int id)
        {
            return db.Clients.Find(id);
        }
        public void Create(Client client)
        {
            db.Clients.Add(client);
        }
        public void Update(Client client)
        {
            db.Entry(client).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Client client = db.Clients.Find(id);
            if (client != null)
                db.Clients.Remove(client);
        }
    }
}
