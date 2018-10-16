using System;
using System.ComponentModel.DataAnnotations;

namespace Deposit.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; protected set; }
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