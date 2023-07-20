using System;
using Unity.VisualScripting;

namespace Assets.Scratch
{
    public class PersistenceModel
    {
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

        private Guid _userId;
        public Guid UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                SaveChanges();
            }
        }

        private Guid _playerId;
        public Guid PlayerId
        {
            get => _playerId;
            set
            {
                _playerId = value;
                SaveChanges();
            }
        }

        private Guid _gameId;
        public Guid GameId
        {
            get => _gameId;
            set
            {
                _gameId = value;
                SaveChanges();
            }
        }

        private static PersistenceModel _instance;
        public static PersistenceModel Instance
        {
            get
            {
                _instance ??= PersistenceAccess.LoadPersistentOrCreate<PersistenceModel>();
                return _instance;
            }
        }

        public void SaveChanges()
        {
            PersistenceAccess.SaveData(this);
        }
    }

    public class PersistenceReducer
    {
        // Wrap properties over PersistenceModel.Instance properties
        public static string Token
        {
            get { return PersistenceModel.Instance.Token; }
            set { PersistenceModel.Instance.Token = value; }
        }

        public static Guid UserId
        {
            get { return PersistenceModel.Instance.UserId; }
            set { PersistenceModel.Instance.UserId = value; }
        }

        public static Guid PlayerId
        {
            get { return PersistenceModel.Instance.PlayerId; }
            set { PersistenceModel.Instance.PlayerId = value; }
        }

        public static Guid GameId
        {
            get { return PersistenceModel.Instance.GameId; }
            set { PersistenceModel.Instance.GameId = value; }
        }
    }

}
