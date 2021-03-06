﻿using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        /// <summary>Name</summary>
        [Required]
        public string Name { get; set; }
    }
}