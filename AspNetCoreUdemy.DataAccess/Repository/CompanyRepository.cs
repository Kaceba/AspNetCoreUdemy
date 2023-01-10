using AspNetCoreUdemy.DataAccess.Repository.IRepository;
using AspNetCoreUdemy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreUdemy.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {

        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Company obj)
        {
            _db.Company.Update(obj);
        }
    }
}
