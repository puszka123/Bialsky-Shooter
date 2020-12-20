using System;

namespace BialskyShooter.ItemSystem
{
    public interface IItem
    {
        Guid GetId();
        Item GetItem();
    }
}
