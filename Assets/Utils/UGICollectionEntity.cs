using Assets.OtherCharacters;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Utils // Used to compare equality with A GUID.
{
    public class UGICollectionEntity<TUGI, TAccessScript, TModel>
        where TUGI : InstanceWrapper<TAccessScript>, IEntity
        where TModel : IEntity 
    {
        public ReadOnlyCollection<TUGI> UGIs => _uGIs.AsReadOnly();

        private Func<TModel, TUGI> UGIBuilderImplementation;

        private List<TUGI> _uGIs = new();

        public UGICollectionEntity()
        {
        }
        public UGICollectionEntity(Func<TModel, TUGI> builderImplementation) // (1) Parent fixe (2) Creation invariable a partir du modele
        {
            this.UGIBuilderImplementation = builderImplementation;
        }

        public void RemoveAndDestroy(TUGI ugi)
        {
            var ugiInList = _uGIs.First(x => object.ReferenceEquals(x, ugi));
            ugiInList.UnityInstance.SelfDestroy();
            _uGIs.Remove(ugiInList);
        }

        public void Clear()
        {
            _uGIs.ForEach(X => X.UnityInstance.SelfDestroy());
            _uGIs.Clear();
        }

        public bool Contains(TUGI ugi)
        {
            var ugiInList = _uGIs.FirstOrDefault(x => object.ReferenceEquals(x, ugi));
            return ugiInList is not null;
        }

        public void RemoveReference(TUGI ugi)
        {
            var ugiInList = _uGIs.First(x => object.ReferenceEquals(x, ugi));
            _uGIs.Remove(ugiInList);

        }

        public void RemoveMany(List<TUGI> remove)
        {
            foreach (var ugi in remove)
            {
                RemoveAndDestroy(ugi);
            }
        }

        public TUGI Add(TUGI ugi)
        {
            _uGIs.Add(ugi);
            return ugi;
        }

        public void AddMany(List<TUGI> ugis)
        {
            _uGIs.AddRange(ugis);
        }

        public void Build(TModel model)
        {
            if (this.UGIBuilderImplementation is null) throw new Exception("Tried to build a UGi without a build method.");
            var newUgi = UGIBuilderImplementation(model);
            Add(newUgi);
        }

        public void BuildMany(List<TModel> models)
        {
            foreach (var model in models)
            {
                Build(model);
            }
        }

        public List<TUGI> GetDisappearedUGis(List<TModel> updatedModels)
        {
            var playerInRoomIds = updatedModels.Select(x => x.Id).ToList();
            var disappearedUGIs = _uGIs.Where(x => !playerInRoomIds.Contains(x.Id)).ToList();
            return disappearedUGIs;
        }

        public List<TModel> GetAppearedModels(List<TModel> updatedModels)
        {
            var playerUGIIds = _uGIs.Select(x => x.Id).ToList();
            var appearedPlayers = updatedModels.Where(x => !playerUGIIds.Contains(x.Id)).ToList();
            return appearedPlayers;
        }

        /// <summary>
        /// From a list of ids, will build the missing UGI or delete it. 
        /// </summary>
        /// <param name="updatedModels"></param>
        /// <exception cref="Exception"></exception>
        public void RefreshFromDbModels(List<TModel> updatedModels)
        {
            if (this.UGIBuilderImplementation is null) throw new Exception("Tried to build a UGi without a build method.");

            var appearedPlayers = GetAppearedModels(updatedModels);
            if (appearedPlayers.Count is not 0)
            {
                this.BuildMany(appearedPlayers);
            }

            var disappearedPlayers = GetDisappearedUGis(updatedModels);
            if (disappearedPlayers.Count is not 0)
            {
                this.RemoveMany(disappearedPlayers);
            }
        }

        public void GuidCustomRefresh()
        {

        }
    }
}
