using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
    public class ProductOptionsDAO
    {

        #region Handling Product actions


        public void ProductSaveOrUpdate(Product prod)
        {
            var conn = Helpers.NewConnection();
            SqlCommand cmd = null;

            cmd = new SqlCommand("update product set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", prod.Id);
            cmd.Parameters.AddWithValue("@Name", prod.Name);
            cmd.Parameters.AddWithValue("@Description", prod.Description);
            cmd.Parameters.AddWithValue("@Price", prod.Price);
            cmd.Parameters.AddWithValue("@DeliveryPrice", prod.DeliveryPrice);
            conn.Open();
            int success = cmd.ExecuteNonQuery();
            if (success != 1)
            {
                cmd = new SqlCommand("insert into product(id, name, description, price, deliveryprice) values(@Id, @Name, @Description, @Price, @DeliveryPrice)", conn);
                cmd.Parameters.AddWithValue("@Id", prod.Id);
                cmd.Parameters.AddWithValue("@Name", prod.Name);
                cmd.Parameters.AddWithValue("@Description", prod.Description);
                cmd.Parameters.AddWithValue("@Price", prod.Price);
                cmd.Parameters.AddWithValue("@DeliveryPrice", prod.DeliveryPrice);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public List<Product> GetAllProducts()
        {
            List<Product> lstProd = new List<Product>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select * from product", conn);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Product tProd = new Product();

                tProd.Id = Guid.Parse(rdr["Id"].ToString());
                tProd.Name = rdr["Name"].ToString();
                tProd.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                tProd.Price = decimal.Parse(rdr["Price"].ToString());
                tProd.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                lstProd.Add(tProd);
            }
            conn.Close();
            return lstProd;
        }

        public Product GetProduct(Guid id)
        {
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select * from product where id = @id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            Product tProd = new Product();
            if (rdr.Read())
            {
                tProd.Id = Guid.Parse(rdr["Id"].ToString());
                tProd.Name = rdr["Name"].ToString();
                tProd.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                tProd.Price = decimal.Parse(rdr["Price"].ToString());
                tProd.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                tProd.IsNew = false;
            }
            else
            {
                tProd.IsNew = true;
            }
            conn.Close();
            return tProd;
        }

        public List<Product> GetAllProductsByName(string name)
        {
            List<Product> lstProd = new List<Product>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select * from product where lower(name) like @name", conn);
            cmd.Parameters.AddWithValue("@name", "%" + name.ToLower() + "%");
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Product tProd = new Product();
                tProd.Id = Guid.Parse(rdr["Id"].ToString());
                tProd.Name = rdr["Name"].ToString();
                tProd.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                tProd.Price = decimal.Parse(rdr["Price"].ToString());
                tProd.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                lstProd.Add(tProd);
            }
            conn.Close();
            return lstProd;
        }

        public void DeleteProduct(Guid id)
        {
            var conn = Helpers.NewConnection();
            conn.Open();
            SqlTransaction sqlTran = conn.BeginTransaction();
            SqlCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.Transaction = sqlTran;
            try
            {

                cmd.CommandText = "delete from productoption where productid=@id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "delete from product where id = @prodId";
                cmd.Parameters.AddWithValue("@prodId", id);
                cmd.ExecuteNonQuery();
                sqlTran.Commit();
            }
            catch
            {
                try
                {
                    sqlTran.Rollback();
                }
                catch { }
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region Product Option

        public void ProductOptionSaveOrUpdate(ProductOption prodOpt)
        {
            var conn = Helpers.NewConnection();
            SqlCommand cmd = new SqlCommand("update productoption set name = @Name, description = @Description where id = @Id and productid=@ProductId", conn);
            cmd.Parameters.AddWithValue("@ProductId", prodOpt.ProductId);
            cmd.Parameters.AddWithValue("@Id", prodOpt.Id);
            cmd.Parameters.AddWithValue("@Name", prodOpt.Name);
            cmd.Parameters.AddWithValue("@Description", prodOpt.Description);
            conn.Open();
            int success = cmd.ExecuteNonQuery();

            if (success != 1)
            {
                cmd = new SqlCommand("insert into productoption(id, productid, name, description) values(@Id, @ProductId, @Name, @Description)", conn);

                cmd.Parameters.AddWithValue("@ProductId", prodOpt.ProductId);
                cmd.Parameters.AddWithValue("@Id", prodOpt.Id);
                cmd.Parameters.AddWithValue("@Name", prodOpt.Name);
                cmd.Parameters.AddWithValue("@Description", prodOpt.Description);
                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        public List<ProductOption> GetAllProductOptions()
        {
            List<ProductOption> lstProdOpt = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select * from ProductOption", conn);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ProductOption tProdOpt = new ProductOption();
                tProdOpt.Id = Guid.Parse(rdr["Id"].ToString());
                tProdOpt.Name = rdr["Name"].ToString();
                tProdOpt.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                tProdOpt.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                lstProdOpt.Add(tProdOpt);
            }
            conn.Close();
            return lstProdOpt;
        }


        public ProductOption GetProductOption(Guid id)
        {
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select * from productoption where id = @id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            ProductOption tProdOpt = new ProductOption();
            if (rdr.Read())
            {
                tProdOpt.Id = Guid.Parse(rdr["Id"].ToString());
                tProdOpt.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                tProdOpt.Name = rdr["Name"].ToString();
                tProdOpt.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
            }
            conn.Close();
            return tProdOpt;
        }

        public List<ProductOption> GetProductOptionByProductId(Guid productId)
        {
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select * from productoption where productid=@productid", conn);
            cmd.Parameters.AddWithValue("@productid", productId);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            List<ProductOption> tProdOptLst = new List<ProductOption>();
            while (rdr.Read())
            {
                ProductOption tProdOpt = new ProductOption();
                tProdOpt.Id = Guid.Parse(rdr["Id"].ToString());
                tProdOpt.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                tProdOpt.Name = rdr["Name"].ToString();
                tProdOpt.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                tProdOptLst.Add(tProdOpt);

            }
            conn.Close();
            return tProdOptLst;
        }


        public ProductOption GetProductOption(Guid productId, Guid id)
        {
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand("select * from productoption where id = @id and productid=@productid", conn);
            cmd.Parameters.AddWithValue("@productid", productId);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            ProductOption tProdOpt = new ProductOption();
            if (rdr.Read())
            {
                tProdOpt.Id = Guid.Parse(rdr["Id"].ToString());
                tProdOpt.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                tProdOpt.Name = rdr["Name"].ToString();
                tProdOpt.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
            }
            conn.Close();
            return tProdOpt;
        }


        public void DeleteProductOption(Guid productId, Guid id)
        {
            var conn = Helpers.NewConnection();
            SqlCommand cmd = new SqlCommand("delete from productoption where productid=@productId and id=@id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@productId", productId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        #endregion
    }
}