using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    public interface ICartStore
    {
        Cart Cart { get; set; }
    }
}
