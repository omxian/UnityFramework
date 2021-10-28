

namespace Snapshot
{
    public class InvalidNode : SnapshotNodeBase
    {
        public override SnapshotNodeData SnapShot()
        {
            return SnapshotNodeData.Invalid;
        }
    }
}
