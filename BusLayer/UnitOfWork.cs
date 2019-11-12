using BusLayer.DataServices;
using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer
{
   public class UnitOfWork :IDisposable
    {
        private AppIdentityDbContext db = new AppIdentityDbContext();

        internal AppIdentityDbContext context {
            get {
                return db;
            }
        }

        public void Save() {
            db.SaveChanges();
        }

        private bool dispose = false;

        protected virtual void Disposed(bool disposing) {
            if (!dispose) {
                if (disposing) {
                    db.Dispose();
                }
            }
            dispose = true;
            }

        public void Dispose()
        {
            Disposed(true);
            GC.SuppressFinalize(this);
        }
    }
}
