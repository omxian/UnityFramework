using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snapshot
{
    public interface ISnapshotMgr 
    {
        void AddSnapshot(SnapshotType snapshotType);
        void RemoveSnapshot(SnapshotType snapshotType);
    }
}
