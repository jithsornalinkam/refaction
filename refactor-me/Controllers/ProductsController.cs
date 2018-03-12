using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using System.Collections.Generic;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private ProductOptionsDAO productOptionsDAO = new ProductOptionsDAO();
        [Route]
        [HttpGet]
        public Products GetAll()
        {
            return new Products();
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            return new Products(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = productOptionsDAO.GetProduct(id);
            if (product.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return product;
        }

        // POST: api/Products
        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            product.Id = Guid.NewGuid();
            productOptionsDAO.ProductSaveOrUpdate(product);
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            product.Id = id;
            productOptionsDAO.ProductSaveOrUpdate(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            productOptionsDAO.DeleteProduct(id);
        }


        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            return productOptionsDAO.GetProductOption(productId, id);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Id = Guid.NewGuid();
            productOptionsDAO.ProductOptionSaveOrUpdate(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            option.Id = id;
            productOptionsDAO.ProductOptionSaveOrUpdate(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid productId, Guid id)
        {
            productOptionsDAO.DeleteProductOption(productId, id);
        }
    }
}
