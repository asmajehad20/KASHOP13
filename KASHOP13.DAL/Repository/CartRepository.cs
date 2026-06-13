using KASHOP13.DAL.Data;
using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }
    }
}
