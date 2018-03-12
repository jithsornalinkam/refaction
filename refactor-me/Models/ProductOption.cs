using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        //public ProductOption()
        //{
        //    Id = Guid.NewGuid();
        //    IsNew = true;
        //}
    }
}