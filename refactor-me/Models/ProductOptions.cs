using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }
        private ProductOptionsDAO productOptionsDAO = new ProductOptionsDAO();

        public ProductOptions(Guid id)
        {
            Items = productOptionsDAO.GetProductOptionByProductId(id);
        }

        public ProductOptions()
        {
            Items = productOptionsDAO.GetAllProductOptions();
        }
    }
}