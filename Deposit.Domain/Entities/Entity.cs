using System;

namespace Deposit.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; }
        public bool IsDeleted { get; protected set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
        }

        public virtual void Delete()
        {
            IsDeleted = true;
        }
    }
}