using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scratch
{
    public class PersistenceModel // makes it possible to use it in all http shit throughout scenes. 
    {
        private static PersistenceModel _instance;

        private string _token;
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                SaveChanges();
            }
        }

        public static PersistenceModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = PersistenceAccess.LoadPersistentOrCreate<PersistenceModel>();
                }
                return _instance;
            }
        }

        public void SaveChanges()
        {
            PersistenceAccess.SaveData(this);
        }
    }
}
