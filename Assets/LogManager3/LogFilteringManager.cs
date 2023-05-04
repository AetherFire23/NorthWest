using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogManager3
{
    // Not a strictly necessary class but it is open for change if I every want to add ActionTypes
    // but also filter by date, lower, upper, etc.
    public class LogFilteringManager
    {
        private FilteringCriteria _filteringCriteria;

        private List<Log> _allLogs = new();

        private Guid _playerId = Guid.Empty;
        private Guid _roomId = Guid.Empty;
        public List<Log> GetUpToDateLogsWithCurrentFilter()
        {
            switch (_filteringCriteria)
            {
                case FilteringCriteria.RoomFilter:
                    {
                        var log = _allLogs.Where(x => x.RoomId.Equals(_roomId)).ToList();
                        return log;
                    }
                case FilteringCriteria.PlayerFilter:
                    {
                        var log = _allLogs.Where(x => x.TriggeringPlayerId.Equals(_playerId)).ToList();
                        return log;
                    }
                default: throw new NotImplementedException();
            }
        }

        public void SetPlayerFilter(Guid playerId)
        {
            _filteringCriteria = FilteringCriteria.PlayerFilter;
            _playerId = playerId;
        }

        public void SetRoomFilter(Guid roomId)
        {
            _filteringCriteria = FilteringCriteria.RoomFilter;
            _roomId = roomId;
        }
        public void AddRange(List<Log> log)
        {
            _allLogs.AddRange(log);
        }
    }
}
