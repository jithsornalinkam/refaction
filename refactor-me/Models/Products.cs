using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{

    public class Products
    {
        public List<Product> Items { get; private set; }
        private ProductOptionsDAO productOptionsDAO = new ProductOptionsDAO();


        public Products()
        {
            Items = productOptionsDAO.GetAllProducts();
        }

        public Products(string name)
        {
            Items = productOptionsDAO.GetAllProductsByName(name);
        }

    }
}